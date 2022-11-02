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
    public class CarController : ControllerBase
    {
        private readonly DBContext _context;

        public CarController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Car> Get()
        {
            return _context.Cars.ToList();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<Car> GetCar(int id)
        {
            Car car = _context.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post(Car car)
        {
            if (car.ID == 0)
            {
                _context.Cars.Add(car);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetCar), new { id = car.ID }, car);
            }
            else
            {
                Car carFromBase = _context.Cars.Find(car.ID);
                if (carFromBase == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Cars.Add(car);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Cars ON; ");
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Cars OFF; ");
                        transaction.Commit();
                    }
                    return CreatedAtAction(nameof(GetCar), new { id = car.ID }, car);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Car> PutCar(int id, Car car)
        {
            if (id != car.ID)
            {
                return BadRequest();
            }

            if (!_context.Cars.Any(c => c.ID == id))
                return NotFound();

            _context.Entry(car).State = EntityState.Modified;

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
            Car carFromBase = _context.Cars.Find(id);
            if (carFromBase != null)
            {
                _context.Cars.Remove(carFromBase);
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
