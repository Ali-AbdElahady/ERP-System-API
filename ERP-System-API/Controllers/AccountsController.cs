using AutoMapper;
using ERP_System.Core.AuthServices.Services;
using ERP_System.Core.Entities;
using ERP_System.Service.DTO.AuthDtos;
using ERP_System.Service.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_System_API.Controllers
{
    public class AccountsController : APIBaseController
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ITokenServices _tokenServices;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager , ITokenServices tokenServices, IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenServices = tokenServices;
			_mapper = mapper;
		}
        // Register
        [HttpPost("Register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			if (CheckEmailExists(model.Email).Result.Value) { 
				return BadRequest(new ApiResponse(400,"This Email Is Already Exist.")); 
			}
			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split('@')[0],
				PhoneNumber = model.PhoneNumber
			};
			var Result = await _userManager.CreateAsync(user,model.Password);
			if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
			return new UserDto()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				Tokken = await _tokenServices.CreateTokenAsync(user, _userManager)
			};
		}

		// Login
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null) return Unauthorized(new ApiResponse(401));
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
			if(!result.Succeeded) return Unauthorized(new ApiResponse(401));
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Tokken = await _tokenServices.CreateTokenAsync(user,_userManager)
			});
		}

		// Get Current User
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("GetCurrentUser")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var Email = User.FindFirstValue(ClaimTypes.Email);
			if (Email == null) return Unauthorized(new ApiResponse(401,"Unauthorized"));
			var user = await _userManager.FindByEmailAsync(Email);
			if(user is null) return NotFound(new ApiResponse(404, "can not found this user"));
			return new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Tokken = await _tokenServices.CreateTokenAsync(user, _userManager)
			};

		}

		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		//[HttpGet("Address")]
		//public async Task<ActionResult<Address>> GetCurrentUserAddress()
		//{
		//	//var Email = User.FindFirstValue(ClaimTypes.Email);
		//	//var user = userManager.FindByEmailAsync(Email);
		//	// Because of User have navigational property the privious way will not work
		//	// Then we will use the Extension method on userManager
		//	var user = await userManager.FindUserWithAddressAsync(User);
		//	var MappedAddress = mapper.Map<Address,AddressDto>(user.Address);
		//	return Ok(MappedAddress);
		//}

		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		//[HttpPut("Address")]
		//public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
		//{
		//	var user = await userManager.FindUserWithAddressAsync(User);
		//	if(user is null) return Unauthorized(new ApiResponse(401));
		//	var address = mapper.Map<AddressDto, Address>(UpdatedAddress);
		//	// beacuse of it create a new object with new id then it will remove the old address and add anthor one
		//	// so we create equal the old Id with The new Id
		//	address.Id = user.Address.Id;
		//	user.Address = address;
		//	var result = await userManager.UpdateAsync(user);
		//	if (!result.Succeeded) return BadRequest(new ApiResponse(401));
		//	return Ok(UpdatedAddress);
		//}

		[HttpGet("eamilExists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}
	}
}
