using ERP_System.Core.Entities;
using ERP_System.Core.Specification.EmployeeSpecs;
using ERP_System.Service.DTO;
using ERP_System.Service.Helpers;

namespace ERP_System.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<Pagination<EmployeeDTO>> GetAllEmployeesAsync(EmployeeSpecParams _params);
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> CreateEmployeeAsync(CreateUpdateEmployeeDto employeeDto);
        Task<bool> UpdateEmployeeAsync(int id, CreateUpdateEmployeeDto employeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
