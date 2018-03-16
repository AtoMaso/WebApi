using System;
using System.Collections.Generic;
using System.Data;
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
    [RoutePrefix("api/addresses")]
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
                    adddto.id = address.id;
                    adddto.number = address.number;
                    adddto.unit = address.unit;
                    adddto.pobox = address.pobox;
                    adddto.street = address.street;
                    adddto.suburb = address.suburb;
                    adddto.city = address.city;
                    adddto.postcode = address.postcode;
                    adddto.state = address.state;               
                    adddto.country = address.country;
                    adddto.preferredFlag= address.preferredFlag;
                    adddto.addressTypeId = address.addressTypeId;
                    adddto.addressType = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressType;
                    adddto.traderId = address.traderId;

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


        // GET: api/addresses/GetAddressesByTraderId?traderId=xss  - this is personalDetailsId           
        [Route("GetAddressesByTraderId")]
        public IHttpActionResult GetAddressesByTraderId(string traderId)
        {
            try
            {              
                List<AddressDTO> dtoList = new List<AddressDTO>();
                foreach (Address address in db.Addresses.Where(ad => ad.traderId == traderId))
                {                
                    AddressDTO adddto = new AddressDTO();
                    adddto.id = address.id;
                    adddto.number = address.number;
                    adddto.unit = address.unit;
                    adddto.pobox = address.pobox;
                    adddto.street = address.street;
                    adddto.suburb = address.suburb;
                    adddto.city = address.city;
                    adddto.postcode = address.postcode;
                    adddto.state = address.state;
                    adddto.country = address.country;
                    adddto.preferredFlag = address.preferredFlag;
                    adddto.addressTypeId = address.addressTypeId;
                    adddto.addressType = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressType;
                    adddto.traderId = address.traderId;

                    dtoList.Add(adddto);                                   
                }
                return Ok<List<AddressDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string error = exc.Message;             
                ModelState.AddModelError("Message", "An unexpected error occured during getting the addresses by trader id!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/addresses?GetPreferredAddress?traderId="xa"&preferredFlag="Yes"
        [AllowAnonymous]   //  this is used on the trader details view not logged in trader
        [Route("GetPreferredAddress")]  
        public IHttpActionResult GetPreferredAddress(string traderId, string preferredFlag)
        {
            try
            {
                var address = db.Addresses.FirstOrDefault(ad => ad.traderId == traderId && ad.preferredFlag == preferredFlag);
                if (address != null)
                {                
                    AddressDTO adddto = new AddressDTO();
                    adddto.id = address.id;
                    adddto.number = address.number;
                    adddto.unit = address.unit;
                    adddto.pobox = address.pobox;
                    adddto.street = address.street;
                    adddto.suburb = address.suburb;
                    adddto.city = address.city;
                    adddto.postcode = address.postcode;
                    adddto.state = address.state;
                    adddto.country = address.country;
                    adddto.preferredFlag = address.preferredFlag;
                    adddto.addressTypeId = address.addressTypeId;
                    adddto.addressType = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressType;
                    adddto.traderId = address.traderId;
                    return Ok<AddressDTO>(adddto);                                                    
                }
                return Ok<Address>(new Address());
            }
            catch (Exception exc)
            {
                string error = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error occured during getting the addresses by trader id!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/addresses/GetAddress?id=4 
        [ResponseType(typeof(AddressDTO))]
        [Route("GetAddress")]
        public async Task<IHttpActionResult> GetAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                ModelState.AddModelError("Message", "Address not found!");
                return BadRequest(ModelState);
            }

            try
            {                          
                AddressDTO adddto = new AddressDTO();
                adddto.id = address.id;
                adddto.number = address.number;
                adddto.unit = address.unit;
                adddto.pobox = address.pobox;
                adddto.street = address.street;
                adddto.suburb = address.suburb;
                adddto.city = address.city;
                adddto.postcode = address.postcode;
                adddto.state = address.state;
                adddto.country = address.country;
                adddto.preferredFlag = address.preferredFlag;
                adddto.addressTypeId = address.addressTypeId;
                adddto.addressType = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == address.addressTypeId).addressType;
                adddto.traderId = address.traderId;

                return Ok(adddto);                          
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the address by address id!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/addresses/PutAddress?id=4
        [ResponseType(typeof(void))]
        [HttpPut]
        [AcceptVerbs("PUT")]
        [Route("PutAddress")]
        public async Task<IHttpActionResult> PutAddress(int id, Address address)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The address details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != address.id)
            {
                ModelState.AddModelError("Message", "The address id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Address not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            Address updateAddress = db.Addresses.Where(u => u.id == id).First();
            AddressDTO adddto = new AddressDTO();
            adddto.id = updateAddress.id;
            adddto.number = updateAddress.number;
            adddto.unit = updateAddress.unit;
            adddto.pobox = updateAddress.pobox;
            adddto.street = updateAddress.street;
            adddto.suburb = updateAddress.suburb;
            adddto.city = updateAddress.city;
            adddto.postcode = updateAddress.postcode;
            adddto.state = updateAddress.state;
            adddto.country = updateAddress.country;
            adddto.preferredFlag = updateAddress.preferredFlag;
            adddto.addressTypeId = updateAddress.addressTypeId;
            adddto.addressType = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == updateAddress.addressTypeId).addressType;
            adddto.traderId = updateAddress.traderId;

            return Ok<AddressDTO>(adddto);
        }


        // POST: api/Addresses/PostAddress
        [ResponseType(typeof(Address))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostAddress")]
        public async Task<IHttpActionResult> PostAddress([FromBody] Address address)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The address details are not valid!");
                return BadRequest(ModelState);
            }

            db.Addresses.Add(address);
            await db.SaveChangesAsync();

            Address lastAddress = await db.Addresses.OrderByDescending(u => u.id).FirstOrDefaultAsync();

            AddressDTO adddto = new AddressDTO();
            adddto.id = lastAddress.id;
            adddto.number = lastAddress.number;
            adddto.unit = lastAddress.unit;
            adddto.street = lastAddress.street;
            adddto.suburb = lastAddress.suburb;
            adddto.city = lastAddress.city;
            adddto.postcode = lastAddress.postcode;
            adddto.state = lastAddress.state;
            adddto.country = lastAddress.country;
            adddto.preferredFlag = lastAddress.preferredFlag;
            adddto.addressTypeId = lastAddress.addressTypeId;
            adddto.addressType = db.AddressTypes.FirstOrDefault(adt => adt.addressTypeId == lastAddress.addressTypeId).addressType;
            adddto.traderId = lastAddress.traderId;

            return Ok<AddressDTO>(adddto);
        }


        // DELETE: api/Addresses/DeleteAddress?id=4
        [ResponseType(typeof(Address))]
        [Route("DeleteAddress")]
        public async Task<IHttpActionResult> DeleteAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                ModelState.AddModelError("Message", "Address not found!");
                return BadRequest(ModelState);
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
            return db.Addresses.Count(e => e.id == id) > 0;
        }
    }
}