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
    [RoutePrefix("api/addresstypes")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class AddressTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/addresstypes
        [AllowAnonymous]
        public IQueryable<AddressType> GetAddressTypes()
        {
            return db.AddressTypes;
        }

        // GET: api/addresstypes/5
        [AllowAnonymous]
        [ResponseType(typeof(AddressType))]
        public async Task<IHttpActionResult> GetAddressType(int id)
        {
            AddressType addressType = await db.AddressTypes.FindAsync(id);
            if (addressType == null)
            {
                ModelState.AddModelError("Message", "Address type not found!");
                return BadRequest(ModelState); ;
            }

            return Ok(addressType);
        }


        // PUT: api/addresstypes/PutAddressType?addressTypeid = 11
        [ResponseType(typeof(void))]   
        [Route("PutAddressType")]      
        public async Task<IHttpActionResult> PutAddressType(int addressTypeId, AddressType addressType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The address type details are not valid!");
                return BadRequest(ModelState);
            }

            if (addressTypeId != addressType.addressTypeId)
            {
                ModelState.AddModelError("Message", "The address type is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(addressType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressTypeExists(addressTypeId))
                {
                    ModelState.AddModelError("Message", "Address type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            AddressType recspps = await db.AddressTypes.Where(cor => cor.addressTypeId == addressTypeId).FirstAsync();
            return Ok<AddressType>(recspps);
        }


        // POST: api/addresstypes/PostAddressType
        [ResponseType(typeof(AddressType))]     
        [Route("PostAddressType")]     
        public async Task<IHttpActionResult> PostAddressType(AddressType addressType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The address type details are not valid!");
                return BadRequest(ModelState);
            }

  
            try
            {
                db.AddressTypes.Add(addressType);
                await db.SaveChangesAsync();

                AddressType lastpc = await db.AddressTypes.OrderByDescending(pc => pc.addressTypeId).FirstAsync();
                return Ok<AddressType>(lastpc);
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Message", "Error during saving your address type!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/addresstypes/DeleteAddressType?addressTypeId =1
        [ResponseType(typeof(AddressType))]       
        [Route("DeleteAddressType")]
        public async Task<IHttpActionResult> DeleteAddressType(int addressTypeId)
        {
            AddressType addressType = await db.AddressTypes.FindAsync(addressTypeId);
            if (addressType == null)
            {
                ModelState.AddModelError("Message", "Address type not found!");
                return BadRequest(ModelState);
            }

            db.AddressTypes.Remove(addressType);
            await db.SaveChangesAsync();

            return Ok(addressType);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressTypeExists(int id)
        {
            return db.AddressTypes.Count(e => e.addressTypeId == id) > 0;
        }
    }
}