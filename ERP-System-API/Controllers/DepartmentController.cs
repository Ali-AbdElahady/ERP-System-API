using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Core.Entities;
using ERP_System.Repository.Data;
using AutoMapper;
using ERP_System.Service.DTO;
using ERP_System.Service.Errors;
using ERP_System.Service.Interfaces;
using ERP_System.Service.Helpers;

namespace ERP_System_API.Controllers
{
    public class DepartmentController : APIBaseController
    {
        private readonly IDepartmentService _departmentService;

        public IMapper _mapper { get; }

        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }
        // GET: api/<Department>
        [HttpGet]
        public async Task<ActionResult<Pagination<DepartmentDto>>> GetDepatments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        // GET api/<Department>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();
            return Ok(department);
        }

        // POST api/<Department>
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] CreateUpdateDepartmentDto value)
        {
            if (value == null)
            {
                return BadRequest(new ApiResponse(400, "there is a problem with your Data"));
            }
            var department = await _departmentService.CreateDepartmentAsync(value);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentId }, department);

        }

        // PUT api/<Department>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] CreateUpdateDepartmentDto value)
        {
            if (value == null)
            {
                return BadRequest(new ApiResponse(400, "there is a problem with your Data"));
            }
            var success = await _departmentService.UpdateDepartmentAsync(id, value);
            if (!success) return NotFound();
            return Ok(new { message = "Department updated successfully." });

        }

        // DELETE api/<Department>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _departmentService.DeleteDepartmentAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Department deleted successfully." });
        }
    }
}
