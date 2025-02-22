using ERP_System.Core.AuthServices.Services;
using ERP_System.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERP_System.Service.AuthServices
{
	public class TokenServices : ITokenServices
	{
		private readonly IConfiguration _configuration;

		public TokenServices(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			// Payload (Register Claims[Sub,Lat] Or Private Claims[Name])
			// 1. Private Claims [User Defined] 

			var AuthClaims = new List<Claim>()
			{
				// using constructor that take (type , value)
				new Claim(ClaimTypes.GivenName , user.DisplayName),
				new Claim(ClaimTypes.Email, user.Email)
				// every time you increase claims the increaption will be more difficult  
			};
			//to get user roll we will use userManager
			var UserRoles = await userManager.GetRolesAsync(user);
            foreach (var Role in UserRoles)
            {
				AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));


			var Token = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"], 
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
				claims:AuthClaims,
				signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
				); 
			return new JwtSecurityTokenHandler().WriteToken(Token);

        }
	}
}
