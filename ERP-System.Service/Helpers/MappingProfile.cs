using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System.Service.DTO;

namespace ERP_System.Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ForMember(dest=>dest.Id,opt=>opt.Ignore()).ReverseMap();
            CreateMap<Department, DepartmentDto>().ForMember(dest=>dest.DepartmentId,opt=>opt.Ignore()).ReverseMap();
        }
    }
}
