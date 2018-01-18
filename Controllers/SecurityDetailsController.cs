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
    public class SecurityDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SecurityDetails
        public IQueryable<SecurityDetails> GetSecurityDetails()
        {
            return db.SecurityDetails;
        }

        // GET: api/SecurityDetails/5
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> GetSecurityDetails(int id)
        {
            SecurityDetails securityDetails = await db.SecurityDetails.FindAsync(id);
            if (securityDetails == null)
            {
                return NotFound();
            }

            return Ok(securityDetails);
        }

        // PUT: api/SecurityDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSecurityDetails(int id, SecurityDetails securityDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != securityDetails.securityDetailsId)
            {
                return BadRequest();
            }

            db.Entry(securityDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityDetailsExists(id))
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

        // POST: api/SecurityDetails
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> PostSecurityDetails(SecurityDetails securityDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SecurityDetails.Add(securityDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = securityDetails.securityDetailsId }, securityDetails);
        }

        // DELETE: api/SecurityDetails/5
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> DeleteSecurityDetails(int id)
        {
            SecurityDetails securityDetails = await db.SecurityDetails.FindAsync(id);
            if (securityDetails == null)
            {
                return NotFound();
            }

            db.SecurityDetails.Remove(securityDetails);
            await db.SaveChangesAsync();

            return Ok(securityDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SecurityDetailsExists(int id)
        {
            return db.SecurityDetails.Count(e => e.securityDetailsId == id) > 0;
        }
    }
}