using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/socialnetworktypes")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class SocialNetworkTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/socialnetworktypes
        [AllowAnonymous]
        public IQueryable<SocialNetworkType> GetSocialNetworkTypes()
        {
            return db.SocialNetworkTypes;
        }


        // GET: api/socialnetworktypes/5
        [AllowAnonymous]
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


        // PUT: api/socialnetworktypes/PutSocialNetworkType?socialTypeId=5
        [ResponseType(typeof(void))]     
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

            SocialNetworkType recspps = await db.SocialNetworkTypes.Where(cor => cor.socialTypeId == socialTypeId).FirstAsync();
            return Ok<SocialNetworkType>(recspps);
        }


        // POST: api/socialnetworktypes/PostSocialNetworkType
        [ResponseType(typeof(SocialNetworkType))]    
        [Route("PostSocialNetworkType")]     
        public async Task<IHttpActionResult> PostSocialNetworkType([FromBody] SocialNetworkType socialNetworkType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The social network type details are not valid!");
                return BadRequest(ModelState);
            }
         
            try
            {
                db.SocialNetworkTypes.Add(socialNetworkType);
                await db.SaveChangesAsync();

                SocialNetworkType lastpc = await db.SocialNetworkTypes.OrderByDescending(pc => pc.socialTypeId).FirstAsync();
                return Ok<SocialNetworkType>(lastpc); ;
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your social network type!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/socialnetworktypes/DeleteSocialNetworkType?socialTypeId=5
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