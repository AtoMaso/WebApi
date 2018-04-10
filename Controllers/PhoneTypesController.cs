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
    [RoutePrefix("api/phonetypes")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class PhoneTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/phonetypes
        [AllowAnonymous]
        public IQueryable<PhoneType> GetPhoneTypes()
        {
            return db.PhoneTypes;
        }


        // GET: api/phonetypes/5
        [AllowAnonymous]
        [ResponseType(typeof(PhoneType))]
        public async Task<IHttpActionResult> GetPhoneTypes(int id)
        {
            PhoneType phoneTypes = await db.PhoneTypes.FindAsync(id);
            if (phoneTypes == null)
            {
                ModelState.AddModelError("Message", "Phone type not found!");
                return BadRequest(ModelState);
            }

            return Ok(phoneTypes);
        }


        // PUT: api/phonetypes/PutPhoneType?phoneTypeId=5
        [ResponseType(typeof(void))]      
        [Route("PutPhoneType")]     
        public async Task<IHttpActionResult> PutPhoneType(int phoneTypeId, PhoneType phoneTypes)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The phone type details are not valid!");
                return BadRequest(ModelState);
            }

            if (phoneTypeId != phoneTypes.phoneTypeId)
            {
                ModelState.AddModelError("Message", "The phone type id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(phoneTypes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneTypesExists(phoneTypeId))
                {
                    ModelState.AddModelError("Message", "Phone type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            PhoneType recspps = await db.PhoneTypes.Where(cor => cor.phoneTypeId == phoneTypeId).FirstAsync();
            return Ok<PhoneType>(recspps);
        }


        // POST: api/phonetypes/PostPhoneType
        [ResponseType(typeof(PhoneType))]    
        [Route("PostPhoneType")]       
        public async Task<IHttpActionResult> PostPhoneType([FromBody] PhoneType phoneTypes)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The phone type details are not valid!");
                return BadRequest(ModelState); ;
            }
           

            try
            {
                db.PhoneTypes.Add(phoneTypes);
                await db.SaveChangesAsync();

                PhoneType lastpc = await db.PhoneTypes.OrderByDescending(pc => pc.phoneTypeId).FirstAsync();
                return Ok<PhoneType>(lastpc); 
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your phone type!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/phonetypes/DeletePhoneType?phoneTypeId=5
        [ResponseType(typeof(PhoneType))]
        [Route("DeletePhoneType")]
        public async Task<IHttpActionResult> DeletePhoneType(int phoneTypeId)
        {
            PhoneType phoneTypes = await db.PhoneTypes.FindAsync(phoneTypeId);
            if (phoneTypes == null)
            {
                ModelState.AddModelError("Message", "Phone type not found!");
                return BadRequest(ModelState);
            }

            db.PhoneTypes.Remove(phoneTypes);
            await db.SaveChangesAsync();

            return Ok(phoneTypes);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneTypesExists(int id)
        {
            return db.PhoneTypes.Count(e => e.phoneTypeId == id) > 0;
        }
    }
}