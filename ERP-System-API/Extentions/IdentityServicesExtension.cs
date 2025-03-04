using ERP_System.Core.AuthServices.Services;
using ERP_System.Core.Entities;
using ERP_System.Repository.Identity;
using ERP_System.Service.AuthServices;
using ERP_System.Service.EmailServices;
using ERP_System.Service.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ERP_System_API.Extentions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration) 
        {
            #region Allow email service Dependency Injection
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            #endregion


            // Allow Dependency injection for Seeding Admin
            services.AddScoped<IUserSeed, UsersSeed>();

            // Add Identity Configurations
            services.AddIdentity<ApplicationUser, IdentityRole>(
                    // for Two-Factor Authentication - 2FA
                    options =>
                    {
                        options.SignIn.RequireConfirmedEmail = true; // يفضل تأكيد البريد قبل 2FA
                        options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                    }
                ).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();


            // Allow Dependency injection for TokenServices to create token with jwt
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
            return services;
        }
    }
}
