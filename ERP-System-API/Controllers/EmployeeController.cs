using AutoMapper;
using ERP_System.Core.Entities;
using ERP_System.Core.Repositories;
using ERP_System_API.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public IMapper _mapper { get; }

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<Employee>
        [HttpGet]
        public async Task<ActionResult<Employee>> GetEmployees()
        {   
            return Ok(await _unitOfWork.Repository<Employee>().GetAllAsync());
        }

        // GET api/<Employee>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDto);
        }

        // POST api/<Employee>
        [HttpPost]
        public async Task<IActionResult> AddEmplyee([FromBody] EmployeeDTO value)
        {
            if (value == null)
            {
                return BadRequest("Invalid employee data.");
            }
            var employee = _mapper.Map<Employee>(value);
            await _unitOfWork.Repository<Employee>().AddAsync(employee);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, value);
        }

        // PUT api/<Employee>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDTO value)
        {
            if(value == null)
            {
                return BadRequest("Invalid employee data.");

            }
            var existingEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            _mapper.Map(value, existingEmployee);
            _unitOfWork.Repository<Employee>().Update(existingEmployee); 
            await _unitOfWork.CompleteAsync(); // Save changes to the database
            return Ok(new { message = "Employee updated successfully." });

        }

        // DELETE api/<Employee>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee ID.");
            }

            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            _unitOfWork.Repository<Employee>().Delete(employee);
            await _unitOfWork.CompleteAsync(); // Save changes to the database

            return Ok(new { message = "Employee deleted successfully." });
        }
    }
}
