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
    public class PersonAllowedToDriveController : ControllerBase
    {
        private readonly DBContext _context;

        public PersonAllowedToDriveController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<PersonAllowedToDrive> Get()
        {
            return _context.PersonAllowedToDrives.ToList();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<PersonAllowedToDrive> GetPersonAllowedToDrive(int id)
        {
            PersonAllowedToDrive personAllowedToDrive = _context.PersonAllowedToDrives.Find(id);
            if (personAllowedToDrive == null)
            {
                return NotFound();
            }
            return personAllowedToDrive;
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post(PersonAllowedToDrive personAllowedToDrive)
        {
            if (personAllowedToDrive.ID == 0)
            {
                _context.PersonAllowedToDrives.Add(personAllowedToDrive);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetPersonAllowedToDrive), new { id = personAllowedToDrive.ID }, personAllowedToDrive);
            }
            else
            {
                PersonAllowedToDrive personAllowedToDriveFromBase = _context.PersonAllowedToDrives.Find(personAllowedToDrive.ID);
                if (personAllowedToDriveFromBase == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.PersonAllowedToDrives.Add(personAllowedToDrive);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.PersonAllowedToDrives ON; ");
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.PersonAllowedToDrives OFF; ");
                        transaction.Commit();
                    }
                    return CreatedAtAction(nameof(GetPersonAllowedToDrive), new { id = personAllowedToDrive.ID }, personAllowedToDrive);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult<PersonAllowedToDrive> PutPersonAllowedToDrive(int id, PersonAllowedToDrive personAllowedToDrive)
        {
            if (id != personAllowedToDrive.ID)
            {
                return BadRequest();
            }

            if (!_context.PersonAllowedToDrives.Any(c => c.ID == id))
                return NotFound();

            _context.Entry(personAllowedToDrive).State = EntityState.Modified;

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
            PersonAllowedToDrive personAllowedToDriveFromBase = _context.PersonAllowedToDrives.Find(id);
            if (personAllowedToDriveFromBase != null)
            {
                _context.PersonAllowedToDrives.Remove(personAllowedToDriveFromBase);
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
