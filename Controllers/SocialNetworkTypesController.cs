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
    [Authorize]
    [RoutePrefix("api/socialnetworktypes")]
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
                ModelState.AddModelError("Message", "Social Network type not found!");
                return BadRequest(ModelState);
            }

            return Ok(socialNetworkType);
        }

        // PUT: api/SocialNetworkTypes/PutSocialNetworkType?socialTypeId=5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("PutSocialNetworkType")]
        public async Task<IHttpActionResult> PutSocialNetworkType(int socialTypeId, SocialNetworkType socialNetworkType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The social network type details are not valid!");
                return BadRequest(ModelState);
            }

            if (socialTypeId != socialNetworkType.socialTypeId)
            {
                ModelState.AddModelError("Message", "The social network type id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(socialNetworkType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialNetworkTypeExists(socialTypeId))
                {
                    ModelState.AddModelError("Message", "Social Network type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/SocialNetworkTypes/PostSocialNetworkType
        [ResponseType(typeof(SocialNetworkType))]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("PostSocialNetworkType")]
        public async Task<IHttpActionResult> PostSocialNetworkType([FromBody] SocialNetworkType socialNetworkType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The social network type details are not valid!");
                return BadRequest(ModelState);
            }

            db.SocialNetworkTypes.Add(socialNetworkType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = socialNetworkType.socialTypeId }, socialNetworkType);
        }


        // DELETE: api/SocialNetworkTypes/DeleteSocialNetworkType?socialTypeId=5
        [ResponseType(typeof(SocialNetworkType))]
        [Route("DeleteSocialNetworkType")]
        public async Task<IHttpActionResult> DeleteSocialNetworkType(int socialTypeId)
        {
            SocialNetworkType socialNetworkType = await db.SocialNetworkTypes.FindAsync(socialTypeId);
            if (socialNetworkType == null)
            {
                ModelState.AddModelError("Message", "Social Network type not found!");
                return BadRequest(ModelState);
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
            return db.SocialNetworkTypes.Count(e => e.socialTypeId == id) > 0;
        }
    }
}