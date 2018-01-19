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
    public class AddressesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/addresses
        public IHttpActionResult GetAddresses()
        {
            try
            {
                var addressdto = from a in db.Addresses
                    select new AddressDTO()
                    {
                        addressId = a.addressId,
                        addressNumber = a.addressNumber,
                        addressStreet = a.addressStreet,
                        addressSuburb = a.addressSuburb,
                        addressCity = a.addressCity,
                        addressCountry = a.addressCountry,
                        addressState= a.addressState,
                        addressPostcode = a.addressPostcode,
                        addressType = a.addressType,
                        personalDetailsId = a.personalDetailsId
                    };
                return Ok(addressdto);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // log the exc
                ModelState.AddModelError("Trade", "An unexpected error occured during getting all trades!");
                return BadRequest(ModelState);
            }          
        }

        // GET: api/addresses/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> GetAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            try
            {                          
                AddressDTO dto = new AddressDTO();
                dto.addressId = address.addressId;
                dto.addressNumber = address.addressNumber;
                dto.addressStreet = address.addressStreet;
                dto.addressSuburb = address.addressSuburb;
                dto.addressPostcode = address.addressPostcode;
                dto.addressCity = address.addressCity ;
                dto.addressState = address.addressState;
                dto.addressCountry = address.addressCountry;
                dto.addressType = address.addressType;
                dto.personalDetailsId = address.personalDetailsId;
                 return Ok(dto);                          
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the trader!");
                return BadRequest(ModelState);
            }
        }



        // PUT: api/addresses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(int id, Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.addressId)
            {
                return BadRequest();
            }

            db.Entry(address).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Addresses
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> PostAddress(Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(address);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = address.addressId }, address);
        }

        // DELETE: api/Addresses/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> DeleteAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            db.Addresses.Remove(address);
            await db.SaveChangesAsync();

            return Ok(address);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int id)
        {
            return db.Addresses.Count(e => e.addressId == id) > 0;
        }
    }
}