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
    public class EmailTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EmailTypes
        public IQueryable<EmailType> GetEmailTypes()
        {
            return db.EmailTypes;
        }

        // GET: api/EmailTypes/5
        [ResponseType(typeof(EmailType))]
        public async Task<IHttpActionResult> GetEmailType(int id)
        {
            EmailType emailType = await db.EmailTypes.FindAsync(id);
            if (emailType == null)
            {
                ModelState.AddModelError("Message", "Email type not found!");
                return BadRequest(ModelState);
            }

            return Ok(emailType);
        }

        // PUT: api/EmailTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmailType(int id, EmailType emailType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailType.typeId)
            {
                return BadRequest();
            }

            db.Entry(emailType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailTypeExists(id))
                {
                    ModelState.AddModelError("Message", "Email type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EmailTypes
        [ResponseType(typeof(EmailType))]
        public async Task<IHttpActionResult> PostEmailType(EmailType emailType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmailTypes.Add(emailType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = emailType.typeId }, emailType);
        }

        // DELETE: api/EmailTypes/5
        [ResponseType(typeof(EmailType))]
        public async Task<IHttpActionResult> DeleteEmailType(int id)
        {
            EmailType emailType = await db.EmailTypes.FindAsync(id);
            if (emailType == null)
            {
                ModelState.AddModelError("Message", "Email type not found!");
                return BadRequest(ModelState);
            }

            db.EmailTypes.Remove(emailType);
            await db.SaveChangesAsync();

            return Ok(emailType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailTypeExists(int id)
        {
            return db.EmailTypes.Count(e => e.typeId == id) > 0;
        }
    }
}