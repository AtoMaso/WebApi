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

        // PUT: api/EmailTypes/PutEmailType?emailTypeId=5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [Route("PutEmailType")]
        public async Task<IHttpActionResult> PutEmailType(int emailTypeId, EmailType emailType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The email type details are not valid!");
                return BadRequest(ModelState);
            }

            if (emailTypeId != emailType.emailTypeId)
            {
                ModelState.AddModelError("Message", "The email type id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(emailType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailTypeExists(emailTypeId))
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


        // POST: api/EmailTypes/PostEmailType
        [ResponseType(typeof(EmailType))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostEmailType")]
        public async Task<IHttpActionResult> PostEmailType([FromBody] EmailType emailType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The email type details are not valid!");
                return BadRequest(ModelState);
            }

            db.EmailTypes.Add(emailType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = emailType.emailTypeId }, emailType);
        }

        // DELETE: api/EmailTypes/DeleteEmailType?emailTypeId=5
        [ResponseType(typeof(EmailType))]
        [Route("DeleteEmailType")]
        public async Task<IHttpActionResult> DeleteEmailType(int emailTypeId)
        {
            EmailType emailType = await db.EmailTypes.FindAsync(emailTypeId);
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
            return db.EmailTypes.Count(e => e.emailTypeId == id) > 0;
        }
    }
}