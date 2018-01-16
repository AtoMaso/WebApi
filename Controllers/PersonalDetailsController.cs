using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class PersonalDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PersonalDetails
        public IQueryable<PersonalDetails> GetPersonalDetails()
        {
            return db.PersonalDetails;
        }

        // GET: api/PersonalDetails/5
        [ResponseType(typeof(PersonalDetails))]
        public async Task<IHttpActionResult> GetPersonalDetails(int id)
        {
            PersonalDetails personalDetails = await db.PersonalDetails.FindAsync(id);
            if (personalDetails == null)
            {
                return NotFound();
            }

            return Ok(personalDetails);
        }

        // PUT: api/PersonalDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonalDetails(int id, PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personalDetails.pdId)
            {
                return BadRequest();
            }

            db.Entry(personalDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PersonalDetails
        [ResponseType(typeof(PersonalDetails))]
        public async Task<IHttpActionResult> PostPersonalDetails(PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PersonalDetails.Add(personalDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = personalDetails.pdId }, personalDetails);
        }

        // DELETE: api/PersonalDetails/5
        [ResponseType(typeof(PersonalDetails))]
        public async Task<IHttpActionResult> DeletePersonalDetails(int id)
        {
            PersonalDetails personalDetails = await db.PersonalDetails.FindAsync(id);
            if (personalDetails == null)
            {
                return NotFound();
            }

            db.PersonalDetails.Remove(personalDetails);
            await db.SaveChangesAsync();

            return Ok(personalDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonalDetailsExists(int id)
        {
            return db.PersonalDetails.Count(e => e.pdId == id) > 0;
        }
    }
}