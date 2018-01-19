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
    public class SecurityAnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SecurityAnswers
        public IQueryable<SecurityAnswer> GetSecurityAnswers()
        {
            return db.SecurityAnswers;
        }

        // GET: api/SecurityAnswers/5
        [ResponseType(typeof(SecurityAnswer))]
        public async Task<IHttpActionResult> GetSecurityAnswer(int id)
        {
            SecurityAnswer securityAnswer = await db.SecurityAnswers.FindAsync(id);
            if (securityAnswer == null)
            {
                return NotFound();
            }

            return Ok(securityAnswer);
        }

        // PUT: api/SecurityAnswers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSecurityAnswer(int id, SecurityAnswer securityAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != securityAnswer.answerId)
            {
                return BadRequest();
            }

            db.Entry(securityAnswer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityAnswerExists(id))
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

        // POST: api/SecurityAnswers
        [ResponseType(typeof(SecurityAnswer))]
        public async Task<IHttpActionResult> PostSecurityAnswer(SecurityAnswer securityAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SecurityAnswers.Add(securityAnswer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = securityAnswer.answerId }, securityAnswer);
        }

        // DELETE: api/SecurityAnswers/5
        [ResponseType(typeof(SecurityAnswer))]
        public async Task<IHttpActionResult> DeleteSecurityAnswer(int id)
        {
            SecurityAnswer securityAnswer = await db.SecurityAnswers.FindAsync(id);
            if (securityAnswer == null)
            {
                return NotFound();
            }

            db.SecurityAnswers.Remove(securityAnswer);
            await db.SaveChangesAsync();

            return Ok(securityAnswer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SecurityAnswerExists(int id)
        {
            return db.SecurityAnswers.Count(e => e.answerId == id) > 0;
        }
    }
}