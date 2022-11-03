using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;

        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await  _fullStackDbContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpPost]
         public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {

            // Create a new ID for this employee. want to create it our self


            employeeRequest.Id = Guid.NewGuid();

            await _fullStackDbContext.Employees.AddAsync(employeeRequest);
            await _fullStackDbContext.SaveChangesAsync();

            return Ok(employeeRequest);

        }




        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
            var emplyee = await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (emplyee == null)
            {
                return NotFound();
            }

            return Ok(emplyee);

        }

        [HttpPut]
        [Route ("{id:Guid}")]

        public async Task<IActionResult> UpdateEmployee([FromRoute]Guid id, Employee updateEmployeeRequest)
        {

            var employee = await _fullStackDbContext.Employees.FindAsync(id);

            if (id == null) 
            {
                return NotFound();

            }

            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Salary = updateEmployeeRequest.Salary;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Department = updateEmployeeRequest.Department;

            await _fullStackDbContext.SaveChangesAsync();

            return Ok(employee);


        }



        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {


            var employee = await _fullStackDbContext.Employees.FindAsync(id);

            if (id == null)
            {
                return NotFound();

            }

            _fullStackDbContext.Employees.Remove(employee);

            await _fullStackDbContext.SaveChangesAsync();

            return Ok(employee);


        }


    }
}
