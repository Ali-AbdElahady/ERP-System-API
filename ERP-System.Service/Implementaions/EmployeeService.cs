using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System.Core.Repositories;
using ERP_System.Service.DTO;
using ERP_System.Service.Errors;
using ERP_System.Service.Helpers;
using ERP_System.Service.Interfaces;

namespace ERP_System.Service.Implementaions
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<EmployeeDTO> CreateEmployeeAsync(CreateUpdateEmployeeDto createUpdateEmployeeDto)
        {
            var employee = _mapper.Map<Employee>(createUpdateEmployeeDto);
            await _unitOfWork.Repository<Employee>().AddAsync(employee);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<EmployeeDTO>(employee);

        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (employee == null) return false;

            _unitOfWork.Repository<Employee>().Delete(employee);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Pagination<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>().GetAllAsync();
            var mapedEmployees = _mapper.Map<IReadOnlyList<EmployeeDTO>>(employees);
            var pagedEmployees = new Pagination<EmployeeDTO>(5, 0, mapedEmployees, 5);
            return pagedEmployees;
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(int id, CreateUpdateEmployeeDto createUpdateEmployeeDto)
        {
            var existingEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (existingEmployee == null) return false;

            _mapper.Map(createUpdateEmployeeDto, existingEmployee);
            _unitOfWork.Repository<Employee>().Update(existingEmployee);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
