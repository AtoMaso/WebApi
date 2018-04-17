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
    public class ForgotPasswordsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ForgotPasswords
        public IQueryable<ForgotPassword> GetForgotPasswords()
        {
            return db.ForgotPasswords;
        }


        // GET: api/ForgotPasswords/5
        [ResponseType(typeof(ForgotPassword))]
        public ForgotPassword GetForgotPasswordByUserid(string userId)
        {
            ForgotPassword forgotPassword = db.ForgotPasswords.Find(userId);
            if (forgotPassword == null)
            {
                return null;
            }
            return forgotPassword;
        }


        // PUT: api/ForgotPasswords/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutForgotPassword(string id, ForgotPassword forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forgotPassword.userId)
            {
                return BadRequest();
            }

            db.Entry(forgotPassword).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForgotPasswordExists(id))
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


        // POST: api/ForgotPasswords
        [ResponseType(typeof(ForgotPassword))]
        public async Task<IHttpActionResult> PostForgotPassword(ForgotPassword forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ForgotPasswords.Add(forgotPassword);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ForgotPasswordExists(forgotPassword.userId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = forgotPassword.userId }, forgotPassword);
        }


        // DELETE: api/ForgotPasswords/5
        [ResponseType(typeof(ForgotPassword))]
        public async Task<IHttpActionResult> DeleteForgotPassword(string id)
        {
            ForgotPassword forgotPassword = await db.ForgotPasswords.FindAsync(id);
            if (forgotPassword == null)
            {
                return NotFound();
            }

            db.ForgotPasswords.Remove(forgotPassword);
            await db.SaveChangesAsync();

            return Ok(forgotPassword);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool ForgotPasswordExists(string id)
        {
            return db.ForgotPasswords.Count(e => e.userId == id) > 0;
        }
    }
}