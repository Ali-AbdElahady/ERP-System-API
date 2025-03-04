using ERP_System.Core.Repositories;
using ERP_System.Repository;
using ERP_System.Service.EmailServices;
using ERP_System.Service.Errors;
using ERP_System.Service.Helpers;
using ERP_System.Service.Implementaions;
using ERP_System.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

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


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    // ModelState => Dic [keyValuePair]
                    // Key => Name Of Param
                    // Value => Errors
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(e => e.ErrorMessage);
                    var validationErrorRespons = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorRespons);
                };
            });
            return services;
        }
    }
}
