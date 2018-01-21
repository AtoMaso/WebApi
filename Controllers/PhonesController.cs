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
    public class PhonesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: localhost:5700/api/phones
        public List<PhoneDTO> GetPhones()
        {
            try
            {
                List<PhoneDTO> dtoList = new List<PhoneDTO>();
                foreach (Phone phone in db.Phones)
                {
                    PhoneDTO phdto = new PhoneDTO();

                    phdto.phoneId = phone.phoneId;
                    phdto.phoneNumber = phone.phoneNumber;
                    phdto.phoneCityCode = phone.phoneCityCode;
                    phdto.phoneCountryCode = phone.phoneCountryCode;
                    phdto.phoneTypeId = phone.phoneTypeId;
                    phdto.phoneTypeDescription = db.PhoneTypes.FirstOrDefault(pt => pt.phoneTypeId == phone.phoneTypeId).phoneTypeDescription;
                    phdto.contactDetailsId = phone.contactDetailsId;                     

                    dtoList.Add(phdto);
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all phones!");
                return null;// BadRequest(ModelState);
            }
        }


        // GET: localhost:5700/api/phones?contactDetailsId=5 - by contactDetailsId
        public List<PhoneDTO> GetPhonesByContactId(int contactDetailsId)
        {
            try
            {
                List<PhoneDTO> dtoList = new List<PhoneDTO>();
                foreach (Phone phone in db.Phones)
                {
                    if(phone.contactDetailsId == contactDetailsId)
                    {
                        PhoneDTO phdto = new PhoneDTO();

                        phdto.phoneId = phone.phoneId;
                        phdto.phoneNumber = phone.phoneNumber;
                        phdto.phoneCityCode = phone.phoneCityCode;
                        phdto.phoneCountryCode = phone.phoneCountryCode;
                        phdto.phoneTypeId = phone.phoneTypeId;
                        phdto.phoneTypeDescription = db.PhoneTypes.FirstOrDefault(pt => pt.phoneTypeId == phone.phoneTypeId).phoneTypeDescription;
                        phdto.contactDetailsId = phone.contactDetailsId;

                        dtoList.Add(phdto);
                    }
                  
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting phones by contact details id!");
                return null;// BadRequest(ModelState);
            }
        }

        // GET: localhost:5700/api/phones/id by phoneId
        [ResponseType(typeof(Phone))]
        public async Task<IHttpActionResult> GetPhone(int id)
        {
            Phone phone = await db.Phones.FindAsync(id);           
            if (phone == null)
            {
                return NotFound();
            }

            try
            {
                PhoneDTO phdto = new PhoneDTO();
                phdto.phoneId = phone.phoneId;
                phdto.phoneNumber = phone.phoneNumber;
                phdto.phoneCityCode = phone.phoneCityCode;
                phdto.phoneCountryCode = phone.phoneCountryCode;
                phdto.phoneTypeId = phone.phoneTypeId;
                phdto.phoneTypeDescription = db.PhoneTypes.FirstOrDefault(pt => pt.phoneTypeId == phone.phoneTypeId).phoneTypeDescription;
                phdto.contactDetailsId = phone.contactDetailsId;
                  
                return Ok(phdto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the phone by phone Id!");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Phones/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhone(int id, Phone phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != phone.phoneId)
            {
                return BadRequest();
            }

            db.Entry(phone).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneExists(id))
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

        // POST: api/Phones
        [ResponseType(typeof(Phone))]
        public async Task<IHttpActionResult> PostPhone(Phone phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Phones.Add(phone);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = phone.phoneId }, phone);
        }

        // DELETE: api/Phones/5
        [ResponseType(typeof(Phone))]
        public async Task<IHttpActionResult> DeletePhone(int id)
        {
            Phone phone = await db.Phones.FindAsync(id);
            if (phone == null)
            {
                return NotFound();
            }

            db.Phones.Remove(phone);
            await db.SaveChangesAsync();

            return Ok(phone);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneExists(int id)
        {
            return db.Phones.Count(e => e.phoneId == id) > 0;
        }
    }
}