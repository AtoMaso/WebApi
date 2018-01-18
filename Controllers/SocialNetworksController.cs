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
    public class SocialNetworksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SocialNetworks
        public IQueryable<SocialNetwork> GetSocialNetworks()
        {
            return db.SocialNetworks;
        }

        // GET: api/SocialNetworks/5
        [ResponseType(typeof(SocialNetwork))]
        public async Task<IHttpActionResult> GetSocialNetwork(int id)
        {
            SocialNetwork socialNetwork = await db.SocialNetworks.FindAsync(id);
            if (socialNetwork == null)
            {
                return NotFound();
            }

            return Ok(socialNetwork);
        }

        // PUT: api/SocialNetworks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSocialNetwork(int id, SocialNetwork socialNetwork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != socialNetwork.socialNetworkId)
            {
                return BadRequest();
            }

            db.Entry(socialNetwork).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialNetworkExists(id))
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

        // POST: api/SocialNetworks
        [ResponseType(typeof(SocialNetwork))]
        public async Task<IHttpActionResult> PostSocialNetwork(SocialNetwork socialNetwork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SocialNetworks.Add(socialNetwork);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = socialNetwork.socialNetworkId }, socialNetwork);
        }

        // DELETE: api/SocialNetworks/5
        [ResponseType(typeof(SocialNetwork))]
        public async Task<IHttpActionResult> DeleteSocialNetwork(int id)
        {
            SocialNetwork socialNetwork = await db.SocialNetworks.FindAsync(id);
            if (socialNetwork == null)
            {
                return NotFound();
            }

            db.SocialNetworks.Remove(socialNetwork);
            await db.SaveChangesAsync();

            return Ok(socialNetwork);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SocialNetworkExists(int id)
        {
            return db.SocialNetworks.Count(e => e.socialNetworkId == id) > 0;
        }
    }
}