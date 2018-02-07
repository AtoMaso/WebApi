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
                List<AddressDTO> dtoList = new List<AddressDTO>();
                foreach (Address address in db.Addresses)
                {
                    AddressDTO adddto = new AddressDTO();
                    adddto.addressId = address.addressId;
                    adddto.addressNumber = address.addressNumber;
                    adddto.addressStreet = address.addressStreet;
                    adddto.addressSuburb = address.addressSuburb;
                    adddto.addressCity = address.addressCity;
                    adddto.addressPostcode = address.addressPostcode;
                    adddto.addressState = address.addressState;               
                    adddto.addressCountry = address.addressCountry;
                    adddto.addressTypeId = address.addressTypeId;
                    adddto.addressTypeDescription = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressTypeDescription;
                    adddto.personalDetailsId = address.personalDetailsId;

                    dtoList.Add(adddto);
                }
                return Ok<List<AddressDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/addresses?personaldetailsId=5  - this is personalDetailsId
        public IHttpActionResult GetAddressesByPersonalId(int personalDetailsId)
        {
            try
            {              
                List<AddressDTO> dtoList = new List<AddressDTO>();
                foreach (Address address in db.Addresses)
                {
                    if (address.personalDetailsId == personalDetailsId)
                    { 
                        AddressDTO adddto = new AddressDTO();
                        adddto.addressId = address.addressId;
                        adddto.addressNumber = address.addressNumber;
                        adddto.addressStreet = address.addressStreet;
                        adddto.addressSuburb = address.addressSuburb;
                        adddto.addressCity = address.addressCity;
                        adddto.addressPostcode = address.addressPostcode;
                        adddto.addressState = address.addressState;
                        adddto.addressCountry = address.addressCountry;
                        adddto.addressTypeId = address.addressTypeId;
                        adddto.addressTypeDescription = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressTypeDescription;
                        adddto.personalDetailsId = address.personalDetailsId;

                        dtoList.Add(adddto);
                    }                  
                }
                return Ok<List<AddressDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string error = exc.Message;             
                ModelState.AddModelError("Message", "An unexpected error occured during getting the addresses by personal details id!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/addresses/5 this is addressid
        [ResponseType(typeof(AddressDTO))]
        public async Task<IHttpActionResult> GetAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            try
            {                          
                AddressDTO adddto = new AddressDTO();
                adddto.addressId = address.addressId;
                adddto.addressNumber = address.addressNumber;
                adddto.addressStreet = address.addressStreet;
                adddto.addressSuburb = address.addressSuburb;
                adddto.addressCity = address.addressCity;
                adddto.addressPostcode = address.addressPostcode;               
                adddto.addressState = address.addressState;
                adddto.addressCountry = address.addressCountry;
                adddto.addressTypeId = address.addressTypeId;
                adddto.addressTypeDescription = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressTypeDescription;
                adddto.personalDetailsId = address.personalDetailsId;

                 return Ok(adddto);                          
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the address by address id!");
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