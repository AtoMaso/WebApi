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
    public class SocialNetworkTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SocialNetworkTypes
        public IQueryable<SocialNetworkType> GetSocialNetworkTypes()
        {
            return db.SocialNetworkTypes;
        }

        // GET: api/SocialNetworkTypes/5
        [ResponseType(typeof(SocialNetworkType))]
        public async Task<IHttpActionResult> GetSocialNetworkType(int id)
        {
            SocialNetworkType socialNetworkType = await db.SocialNetworkTypes.FindAsync(id);
            if (socialNetworkType == null)
            {
                return NotFound();
            }

            return Ok(socialNetworkType);
        }

        // PUT: api/SocialNetworkTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSocialNetworkType(int id, SocialNetworkType socialNetworkType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != socialNetworkType.typeId)
            {
                return BadRequest();
            }

            db.Entry(socialNetworkType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialNetworkTypeExists(id))
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

        // POST: api/SocialNetworkTypes
        [ResponseType(typeof(SocialNetworkType))]
        public async Task<IHttpActionResult> PostSocialNetworkType(SocialNetworkType socialNetworkType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SocialNetworkTypes.Add(socialNetworkType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = socialNetworkType.typeId }, socialNetworkType);
        }

        // DELETE: api/SocialNetworkTypes/5
        [ResponseType(typeof(SocialNetworkType))]
        public async Task<IHttpActionResult> DeleteSocialNetworkType(int id)
        {
            SocialNetworkType socialNetworkType = await db.SocialNetworkTypes.FindAsync(id);
            if (socialNetworkType == null)
            {
                return NotFound();
            }

            db.SocialNetworkTypes.Remove(socialNetworkType);
            await db.SaveChangesAsync();

            return Ok(socialNetworkType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SocialNetworkTypeExists(int id)
        {
            return db.SocialNetworkTypes.Count(e => e.typeId == id) > 0;
        }
    }
}