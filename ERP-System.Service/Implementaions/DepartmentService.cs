using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System.Core.Repositories;
using ERP_System.Repository;
using ERP_System.Service.DTO;
using ERP_System.Service.Helpers;
using ERP_System.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Service.Implementaions
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DepartmentDto> CreateDepartmentAsync(CreateUpdateDepartmentDto createUpdateDepartmentDto)
        {
            var department = _mapper.Map<Department>(createUpdateDepartmentDto);
            await _unitOfWork.Repository<Department>().AddAsync(department);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return false;

            _unitOfWork.Repository<Department>().Delete(department);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Pagination<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();
            var mappedDepartment = _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
            var pagedDepartments = new Pagination<DepartmentDto>(5,0,mappedDepartment,5);
             return pagedDepartments;
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<bool> UpdateDepartmentAsync(int id, CreateUpdateDepartmentDto createUpdateDepartmentDto)
        {
            var existingDepartment = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (existingDepartment == null) return false;

            _mapper.Map(createUpdateDepartmentDto, existingDepartment);
            _unitOfWork.Repository<Department>().Update(existingDepartment);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
