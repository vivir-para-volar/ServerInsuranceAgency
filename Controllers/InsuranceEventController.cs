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
    public class InsuranceEventController : ControllerBase
    {
        private readonly DBContext _context;

        public InsuranceEventController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<InsuranceEvent> Get()
        {
            return _context.InsuranceEvents.ToList();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<InsuranceEvent> GetInsuranceEvent(int id)
        {
            InsuranceEvent insuranceEvent = _context.InsuranceEvents.Find(id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }
            return insuranceEvent;
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post(InsuranceEvent insuranceEvent)
        {
            if (insuranceEvent.ID == 0)
            {
                _context.InsuranceEvents.Add(insuranceEvent);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetInsuranceEvent), new { id = insuranceEvent.ID }, insuranceEvent);
            }
            else
            {
                InsuranceEvent insuranceEventFromBase = _context.InsuranceEvents.Find(insuranceEvent.ID);
                if (insuranceEventFromBase == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.InsuranceEvents.Add(insuranceEvent);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.InsuranceEvents ON; ");
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.InsuranceEvents OFF; ");
                        transaction.Commit();
                    }
                    return CreatedAtAction(nameof(GetInsuranceEvent), new { id = insuranceEvent.ID }, insuranceEvent);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult<InsuranceEvent> PutInsuranceEvent(int id, InsuranceEvent insuranceEvent)
        {
            if (id != insuranceEvent.ID)
            {
                return BadRequest();
            }

            if (!_context.InsuranceEvents.Any(c => c.ID == id))
                return NotFound();

            _context.Entry(insuranceEvent).State = EntityState.Modified;

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
            InsuranceEvent insuranceEventFromBase = _context.InsuranceEvents.Find(id);
            if (insuranceEventFromBase != null)
            {
                _context.InsuranceEvents.Remove(insuranceEventFromBase);
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
