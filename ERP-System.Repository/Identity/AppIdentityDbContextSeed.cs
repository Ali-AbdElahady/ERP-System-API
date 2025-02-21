using ERP_System.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ERP_System.Repository.Identity
{
	public static class AppIdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
		{
			if(!userManager.Users.Any()) 
			{
				var User = new ApplicationUser()
				{
					DisplayName = "Ali Ahmed",
					Email = "Aliahmed@gmail.com",
					UserName = "AliAhmed",
					PhoneNumber = "1234567890",
				};
				await userManager.CreateAsync(User,"Pa$$w0rd");
			}
		}
	}
}
