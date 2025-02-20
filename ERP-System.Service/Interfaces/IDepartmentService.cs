using ERP_System.Core.Entities;
using ERP_System.Core.Repositories;
using ERP_System.Service.DTO;
using ERP_System.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Service.Interfaces
{
    public interface IDepartmentService
    {
        Task<Pagination<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> GetDepartmentByIdAsync(int id);
        Task<DepartmentDto> CreateDepartmentAsync(CreateUpdateDepartmentDto departmentDto);
        Task<bool> UpdateDepartmentAsync(int id, CreateUpdateDepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
