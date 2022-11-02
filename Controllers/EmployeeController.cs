using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DBContext _context;

        public EmployeeController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post(Employee employee)
        {
            if (employee.ID == 0)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
            }
            else
            {
                Employee employeeFromBase = _context.Employees.Find(employee.ID);
                if (employeeFromBase == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Employees.Add(employee);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Employees ON; ");
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Employees OFF; ");
                        transaction.Commit();
                    }
                    return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Employee> PutEmployee(int id, Employee employee)
        {
            if (id != employee.ID)
            {
                return BadRequest();
            }

            if (!_context.Employees.Any(c => c.ID == id))
                return NotFound();

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Employee employeeFromBase = _context.Employees.Find(id);
            if (employeeFromBase != null)
            {
                _context.Employees.Remove(employeeFromBase);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            else
            {
                return BadRequest();
                //или
                //return NotFound();
            }
        }
    }
}
