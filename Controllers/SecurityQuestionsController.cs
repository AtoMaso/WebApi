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
    public class SecurityQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SecurityQuestions
        public IQueryable<SecurityQuestion> GetSecurityQuestions()
        {
            return db.SecurityQuestions;
        }

        // GET: api/SecurityQuestions/5
        [ResponseType(typeof(SecurityQuestion))]
        public async Task<IHttpActionResult> GetSecurityQuestion(int id)
        {
            SecurityQuestion securityQuestion = await db.SecurityQuestions.FindAsync(id);
            if (securityQuestion == null)
            {
                ModelState.AddModelError("Message", "Security question not found!");
                return BadRequest(ModelState);
            }

            return Ok(securityQuestion);
        }

        // PUT: api/SecurityQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSecurityQuestion(int id, SecurityQuestion securityQuestion)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The security question details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != securityQuestion.questionId)
            {
                ModelState.AddModelError("Message", "The security question id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(securityQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityQuestionExists(id))
                {
                    ModelState.AddModelError("Message", "Security question not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SecurityQuestions
        [ResponseType(typeof(SecurityQuestion))]
        public async Task<IHttpActionResult> PostSecurityQuestion(SecurityQuestion securityQuestion)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The security question details are not valid!");
                return BadRequest(ModelState);
            }

            db.SecurityQuestions.Add(securityQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = securityQuestion.questionId }, securityQuestion);
        }

        // DELETE: api/SecurityQuestions/5
        [ResponseType(typeof(SecurityQuestion))]
        public async Task<IHttpActionResult> DeleteSecurityQuestion(int id)
        {
            SecurityQuestion securityQuestion = await db.SecurityQuestions.FindAsync(id);
            if (securityQuestion == null)
            {
                ModelState.AddModelError("Message", "Security question not found!");
                return BadRequest(ModelState);
            }

            db.SecurityQuestions.Remove(securityQuestion);
            await db.SaveChangesAsync();

            return Ok(securityQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SecurityQuestionExists(int id)
        {
            return db.SecurityQuestions.Count(e => e.questionId == id) > 0;
        }
    }
}