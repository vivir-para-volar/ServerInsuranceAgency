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
    public class PolicyholderController : ControllerBase
    {
        private readonly DBContext _context;

        public PolicyholderController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Policyholder> Get()
        {
            return _context.Policyholders.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Policyholder> GetPolicyholder(int id)
        {
            Policyholder policyholder = _context.Policyholders.Find(id);
            if (policyholder == null)
            {
                return NotFound();
            }
            return policyholder;
        }

        [HttpPost]
        public ActionResult Post(Policyholder policyholder)
        {
            if (policyholder.ID == 0)
            {
                _context.Policyholders.Add(policyholder);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetPolicyholder), new { id = policyholder.ID }, policyholder);
            }
            else
            {
                Policyholder policyholderFromBase = _context.Policyholders.Find(policyholder.ID);
                if (policyholderFromBase == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Policyholders.Add(policyholder);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Policyholders ON; ");
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Policyholders OFF; ");
                        transaction.Commit();
                    }
                    return CreatedAtAction(nameof(GetPolicyholder), new { id = policyholder.ID }, policyholder);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Policyholder> PutPolicyholder(int id, Policyholder policyholder)
        {
            if (id != policyholder.ID)
            {
                return BadRequest();
            }

            if (!_context.Policyholders.Any(c => c.ID == id))
                return NotFound();

            _context.Entry(policyholder).State = EntityState.Modified;

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
            Policyholder policyholderFromBase = _context.Policyholders.Find(id);
            if (policyholderFromBase != null)
            {
                _context.Policyholders.Remove(policyholderFromBase);
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
