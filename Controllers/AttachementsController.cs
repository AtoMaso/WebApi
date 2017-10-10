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
    public class AttachementsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Attachements
        public IQueryable<Attachement> GetAttachements()
        {
            return db.Attachements;
        }

        // GET: api/Attachements/5
        [ResponseType(typeof(Attachement))]
        public async Task<IHttpActionResult> GetAttachement(long id)
        {
            Attachement attachement = await db.Attachements.FindAsync(id);
            if (attachement == null)
            {
                return NotFound();
            }

            return Ok(attachement);
        }

        // PUT: api/Attachements/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAttachement(long id, Attachement attachement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attachement.AttachementId)
            {
                return BadRequest();
            }

            db.Entry(attachement).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttachementExists(id))
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

        // POST: api/Attachements
        [ResponseType(typeof(Attachement))]
        public async Task<IHttpActionResult> PostAttachement(Attachement attachement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attachements.Add(attachement);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = attachement.AttachementId }, attachement);
        }

        // DELETE: api/Attachements/5
        [ResponseType(typeof(Attachement))]
        public async Task<IHttpActionResult> DeleteAttachement(long id)
        {
            Attachement attachement = await db.Attachements.FindAsync(id);
            if (attachement == null)
            {
                return NotFound();
            }

            db.Attachements.Remove(attachement);
            await db.SaveChangesAsync();

            return Ok(attachement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttachementExists(long id)
        {
            return db.Attachements.Count(e => e.AttachementId == id) > 0;
        }
    }
}