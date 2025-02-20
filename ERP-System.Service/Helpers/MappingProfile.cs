using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System.Service.DTO;

namespace ERP_System.Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Employee, CreateUpdateEmployeeDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, CreateUpdateDepartmentDto>().ReverseMap();
        }
    }
}
