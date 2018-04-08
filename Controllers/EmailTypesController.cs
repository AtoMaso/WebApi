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
    [RoutePrefix("api/emailtypes")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class EmailTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/emailtypes
        [AllowAnonymous]
        public IQueryable<EmailType> GetEmailTypes()
        {
            return db.EmailTypes;
        }

        // GET: api/emailtypes/5
        [AllowAnonymous]
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

        // PUT: api/emailtypes/PutEmailType?emailTypeId=5
        [ResponseType(typeof(void))] 
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

            EmailType recspps = await db.EmailTypes.Where(cor => cor.emailTypeId == emailTypeId).FirstAsync();
            return Ok<EmailType>(recspps);
        }


        // POST: api/emailtypes/PostEmailType
        [ResponseType(typeof(EmailType))]    
        [Route("PostEmailType")]     
        public async Task<IHttpActionResult> PostEmailType([FromBody] EmailType emailType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The email type details are not valid!");
                return BadRequest(ModelState);
            }
       
            try
            {
                db.EmailTypes.Add(emailType);
                await db.SaveChangesAsync();

                EmailType lastpc = await db.EmailTypes.OrderByDescending(pc => pc.emailTypeId).FirstAsync();
                return Ok<EmailType>(lastpc); ;
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your email type!");
                return BadRequest(ModelState);
            }       
        }


        // DELETE: api/emailtypes/DeleteEmailType?emailTypeId=5
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