using ERP_System.Core.Entities;
using ERP_System.Core.Repositories;
using ERP_System.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Service.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> GetDepartmentByIdAsync(int id);
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<bool> UpdateDepartmentAsync(int id, DepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
