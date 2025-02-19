using ERP_System.Core.Repositories;
using ERP_System.Repository;
using ERP_System.Service.Helpers;
using ERP_System.Service.Implementaions;
using ERP_System.Service.Interfaces;

namespace ERP_System_API.Extentions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));
            services.AddScoped(typeof(IDepartmentService), typeof(DepartmentService));
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
