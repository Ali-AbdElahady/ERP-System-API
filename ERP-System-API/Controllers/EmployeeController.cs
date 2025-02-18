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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Employee>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
