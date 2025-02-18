using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System_API.DTO;

namespace ERP_System_API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ForMember(dest=>dest.Id,opt=>opt.Ignore()).ReverseMap();
        }
    }
}
