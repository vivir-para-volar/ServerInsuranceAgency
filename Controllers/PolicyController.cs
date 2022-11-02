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
    public class PolicyController : ControllerBase
    {
        private readonly DBContext _context;

        public PolicyController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Policy> Get()
        {
            return _context.Policies.ToList();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<Policy> GetPolicy(int id)
        {
            Policy policy = _context.Policies.Find(id);
            if (policy == null)
            {
                return NotFound();
            }
            return policy;
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post(Policy policy)
        {
            List<PersonAllowedToDrive> listPersons = policy.PersonsAllowedToDrive.ToList();
            policy.PersonsAllowedToDrive = null;

            if (policy.ID == 0)
            {
                _context.Policies.Add(policy);
                _context.SaveChanges();

                foreach (var item in listPersons)
                {
                    PersonAllowedToDrive person = _context.PersonAllowedToDrives.Find(item.ID);
                    if (person == null)
                        return BadRequest();

                    person.Policies.Add(policy);
                }
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetPolicy), new { id = policy.ID }, policy);
            }
            else
            {
                Policy policyFromBase = _context.Policies.Find(policy.ID);
                if (policyFromBase == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Policies.Add(policy);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Policies ON; ");
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT DBWebAPIDemo.dbo.Policies OFF; ");
                        transaction.Commit();
                    }

                    foreach (var item in listPersons)
                    {
                        PersonAllowedToDrive person = _context.PersonAllowedToDrives.Find(item.ID);
                        if (person == null)
                            return BadRequest();

                        person.Policies.Add(policy);
                    }
                    _context.SaveChanges();

                    return CreatedAtAction(nameof(GetPolicy), new { id = policy.ID }, policy);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Policy> PutPolicy(int id, Policy policy)
        {
            if (id != policy.ID)
            {
                return BadRequest();
            }

            if (!_context.Policies.Any(c => c.ID == id))
                return NotFound();

            List<PersonAllowedToDrive> listPersons = policy.PersonsAllowedToDrive.ToList();
            policy.PersonsAllowedToDrive = null;

            _context.Entry(policy).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (listPersons != null)
            {
                List<PersonAllowedToDrive> listOldPerson = _context.Policies.Include(p => p.PersonsAllowedToDrive)
                                                                            .First(p => p.ID == id)
                                                                            .PersonsAllowedToDrive.ToList();

                var commons = listPersons.Select(p => p.ID).Intersect(listOldPerson.Select(p => p.ID)).ToList();
                listPersons.RemoveAll(p => commons.Contains(p.ID));
                listOldPerson.RemoveAll(p => commons.Contains(p.ID));

                foreach (var person in listOldPerson)
                {
                    policy.PersonsAllowedToDrive.Remove(person);
                }
                _context.SaveChanges();

                foreach (var person in listPersons)
                {
                    policy.PersonsAllowedToDrive.Add(person);
                }

                _context.SaveChanges();
            }

            return NoContent();
        }

        [HttpPut("PersonAllowedToDrive/{id}")]
        public ActionResult<Policy> PutPolicyPersonAllowedToDrive(int id, Policy policy)
        {
            if (id != policy.ID)
            {
                return BadRequest();
            }

            if (!_context.Policies.Any(c => c.ID == id))
                return NotFound();


            if (policy.PersonsAllowedToDrive != null)
            {
                List<PersonAllowedToDrive> listPersons = policy.PersonsAllowedToDrive.ToList();
                policy.PersonsAllowedToDrive.Clear();
                _context.SaveChanges();

                foreach (var person in listPersons)
                {
                    person.Policies.Add(policy);
                    policy.PersonsAllowedToDrive.Add(person);
                }

                _context.SaveChanges();
            }
            else
            {
                policy.PersonsAllowedToDrive = null;
                _context.SaveChanges();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Policy policyFromBase = _context.Policies.Find(id);
            if (policyFromBase != null)
            {
                _context.Policies.Remove(policyFromBase);
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
