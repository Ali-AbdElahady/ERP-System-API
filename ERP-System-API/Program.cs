
using ERP_System.Core.Entities;
using ERP_System.Repository.Data;
using ERP_System.Repository.Identity;
using ERP_System.Service.AuthServices;
using ERP_System_API.Extentions;
using ERP_System_API.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdenityConnection"));
            });
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });

            builder.Services.AddSwaggerWithJwtAuthentication();
            builder.Services.AddAplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            #region Update-datebase
            //StoreContext dbContext = new StoreContext(); //invalid
            //await dbContext.Database.MigrateAsync();
            using var Scope = app.Services.CreateScope();
            // Group Of Services LifeTime Scoped
            var Services = Scope.ServiceProvider;
            // Services Its Self
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                // Ask CLR For Creating Object From DbContext Explicitly
                var dbContext = Services.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();

                // Ask CLR For Creating Object From AppIdentityDbContext Explicitly
                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();

                // Ask CLR For Creating Object From UserManager<ApplicationUser> Explicitly to Seed data
                //var UserManagerDbContext = Services.GetRequiredService<UserManager<ApplicationUser>>();
                //await StoreContextSeed.SeedAsync(dbContext);
                var dbInitial = Services.GetRequiredService<IUserSeed>();
                await dbInitial.Initialize();
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Appling The Migration");
            }

            #endregion

            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
