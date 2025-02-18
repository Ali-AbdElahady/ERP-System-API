
using ERP_System.Repository.Data;
using ERP_System_API.Extentions;
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


            builder.Services.AddAplicationServices();

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
                //var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                //await IdentityDbContext.Database.MigrateAsync();

                // Ask CLR For Creating Object From UserManager<AppUser> Explicitly to Seed data
                //var UserManagerDbContext = Services.GetRequiredService<UserManager<AppUser>>();
                //await AppIdentityDbContextSeed.SeedUserAsync(UserManagerDbContext);

                //await StoreContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Appling The Migration");
            }

            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
