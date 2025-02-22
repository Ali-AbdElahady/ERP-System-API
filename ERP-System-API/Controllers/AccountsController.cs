using AutoMapper;
using ERP_System.Core.AuthServices.Services;
using ERP_System.Core.Entities;
using ERP_System.Service.DTO.AuthDtos;
using ERP_System.Service.EmailServices;
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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

		public AccountsController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager ,
			ITokenServices tokenServices,
			IEmailService emailService,
			IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenServices = tokenServices;
            _emailService = emailService;
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

		[HttpGet("eamilExists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}

        // Update User Profile
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto model)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return NotFound(new ApiResponse(404, "User not found"));

            // Check if email is being changed and already exists
            if (model.Email != user.Email && await _userManager.FindByEmailAsync(model.Email)  is not null)
            {
                return BadRequest(new ApiResponse(400, "This email is already in use."));
            }

            user.DisplayName = model.DisplayName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.UserName = model.Email.Split('@')[0]; // Keep username aligned with email

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Error updating user"));

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Tokken = await _tokenServices.CreateTokenAsync(user, _userManager)
            };
        }

        // Request Password Reset (Generate Token)
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return BadRequest(new ApiResponse(400, "User not found"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailService.SendResetLinkAsync(user.Email, token);

			return Ok(new { Message = "Password reset link has been sent to your email." });
        }

        // Reset Password
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return BadRequest(new ApiResponse(400, "Invalid email"));

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Invalid token or password reset failed"));

            return Ok(new { Message = "Password has been reset successfully." });
        }

    }
}
