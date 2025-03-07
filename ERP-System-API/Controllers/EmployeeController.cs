﻿using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System.Core.Repositories;
using ERP_System.Core.Specification.EmployeeSpecs;
using ERP_System.Service.DTO;
using ERP_System.Service.Errors;
using ERP_System.Service.Helpers;
using ERP_System.Service.Implementaions;
using ERP_System.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ERP_System_API.Controllers
{
    public class EmployeeController : APIBaseController
    {
        private readonly IEmployeeService _employeeService;

        public IMapper _mapper { get; }

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        // GET: api/<Employee>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<Pagination<EmployeeDTO>>> GetEmployees([FromQuery] EmployeeSpecParams _params)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(_params);
            return Ok(employees);
        }

        // GET api/<Employee>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        // POST api/<Employee>
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] CreateUpdateEmployeeDto value)
        {
            if (value == null)
            {
                return BadRequest(new ApiResponse(400, "there is a problem with your Data"));
            }
            var employee = await _employeeService.CreateEmployeeAsync(value);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);

        }

        // PUT api/<Employee>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateUpdateEmployeeDto value)
        {
            if (value == null)
            {
                return BadRequest(new ApiResponse(400, "there is a problem with your Data"));
            }
            var success = await _employeeService.UpdateEmployeeAsync(id, value);
            if (!success) return NotFound();
            return Ok(new { message = "Employee updated successfully." });

        }

        // DELETE api/<Employee>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Employee deleted successfully." });
        }
    }
}
