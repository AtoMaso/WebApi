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
    public class AddressTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AddressTypes
        public IQueryable<AddressType> GetAddressTypes()
        {
            return db.AddressTypes;
        }

        // GET: api/AddressTypes/5
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

        // PUT: api/AddressTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddressType(int id, AddressType addressType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The address type details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != addressType.typeId)
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
                if (!AddressTypeExists(id))
                {
                    ModelState.AddModelError("Message", "Address type not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AddressTypes
        [ResponseType(typeof(AddressType))]
        public async Task<IHttpActionResult> PostAddressType(AddressType addressType)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The address type details are not valid!");
                return BadRequest(ModelState);
            }

            db.AddressTypes.Add(addressType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = addressType.typeId }, addressType);
        }

        // DELETE: api/AddressTypes/5
        [ResponseType(typeof(AddressType))]
        public async Task<IHttpActionResult> DeleteAddressType(int id)
        {
            AddressType addressType = await db.AddressTypes.FindAsync(id);
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
            return db.AddressTypes.Count(e => e.typeId == id) > 0;
        }
    }
}