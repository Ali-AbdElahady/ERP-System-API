using ERP_System.Core.Entities;
using ERP_System.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ERP_System.Service.AuthServices
{
    public class UsersSeed : IUserSeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UsersSeed(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task Initialize()
        {
            if (!await _roleManager.RoleExistsAsync(WebSiteRoles.WebSite_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.WebSite_Admin));

                await _userManager.CreateAsync(new ApplicationUser()
                {
                    DisplayName = _configuration["AdminData:DisplayName"],
                    Email = _configuration["AdminData:Email"],
                    UserName = _configuration["AdminData:UserName"],
                    PhoneNumber = _configuration["AdminData:DisplayNPhoneNumberame"]
                }, _configuration["AdminData:Password"]);
                //var AdminUser = _dbContext.ApplicationUsers.FirstOrDefault(x => x.Email == "Dc_ALi@gmail.com");
                var AdminUser = await _userManager.FindByEmailAsync(_configuration["AdminData:Email"]);

                if (AdminUser != null)
                {
                    await _userManager.AddToRoleAsync(AdminUser, WebSiteRoles.WebSite_Admin);
                }
                //if (_userManager.Users.Count() == 1)
                //{
                //    var DoctorsData = File.ReadAllText("../Hospital.DAL/DataSeed/Doctors.json");
                //    var Doctors = JsonSerializer.Deserialize<List<ApplicationUser>>(DoctorsData);

                //    if (Doctors?.Count > 0)
                //    {
                //        foreach (var Doctor in Doctors)
                //        {
                //            var User = new ApplicationUser()
                //            {
                //                FName = Doctor.FName,
                //                LName = Doctor.LName,
                //                Email = Doctor.Email,
                //                UserName = UsernameGenerator.GenerateUniqueUsername(Doctor.FName, Doctor.LName),
                //                PhoneNumber = Doctor.PhoneNumber,
                //                Department_ID = Doctor.Department_ID,
                //                Specialization_ID = Doctor.Specialization_ID,
                //            };
                //            await _userManager.CreateAsync(User, "Pa$$w0rdDoctor");
                //            var DoctorData = _dbContext.ApplicationUsers
                //                .FirstOrDefault(x => x.UserName == User.UserName);
                //            await _userManager.AddToRoleAsync(DoctorData, WebSiteRoles.WebSite_Doctor);
                //        }
                //    }

                
                //}
            }
        }
    }
}
