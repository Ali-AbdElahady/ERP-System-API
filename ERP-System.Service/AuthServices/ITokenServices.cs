using ERP_System.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ERP_System.Core.AuthServices.Services
{
	public interface ITokenServices
	{
		Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
	}
}
