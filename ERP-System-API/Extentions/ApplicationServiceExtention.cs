using ERP_System.Core.Repositories;
using ERP_System.Repository;
using ERP_System_API.Helpers;

namespace ERP_System_API.Extentions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
