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
        public IHttpActionResult GetPhones()
        {
            try
            {
                List<PhoneDTO> dtoList = new List<PhoneDTO>();
                foreach (Phone phone in db.Phones)
                {
                    PhoneDTO phdto = new PhoneDTO();

                    phdto.id = phone.id;
                    phdto.number = phone.number;
                    phdto.cityCode = phone.cityCode;
                    phdto.countryCode = phone.countryCode;
                    phdto.preferred = phone.preferred;
                    phdto.typeId = phone.typeId;
                    phdto.typeDescription = db.PhoneTypes.FirstOrDefault(pt => pt.typeId == phone.typeId).typeDescription;
                    phdto.contactDetailsId = phone.contactDetailsId;                     

                    dtoList.Add(phdto);
                }
                return Ok<List<PhoneDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all phones!");
                return BadRequest(ModelState);
            }
        }


        // GET: localhost:5700/api/phones?contactDetailsId=5 - by contactDetailsId
        public IHttpActionResult GetPhonesByContactId(int contactDetailsId)
        {
            try
            {
                List<PhoneDTO> dtoList = new List<PhoneDTO>();
                foreach (Phone phone in db.Phones)
                {
                    if(phone.contactDetailsId == contactDetailsId)
                    {
                        PhoneDTO phdto = new PhoneDTO();

                        phdto.id = phone.id;
                        phdto.number = phone.number;
                        phdto.cityCode = phone.cityCode;
                        phdto.countryCode = phone.countryCode;
                        phdto.preferred = phone.preferred;
                        phdto.typeId = phone.typeId;
                        phdto.typeDescription = db.PhoneTypes.FirstOrDefault(pt => pt.typeId == phone.typeId).typeDescription;
                        phdto.contactDetailsId = phone.contactDetailsId;

                        dtoList.Add(phdto);
                    }
                  
                }
                return Ok<List<PhoneDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting phones by contact details id!");
                return BadRequest(ModelState);
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

                phdto.id = phone.id;
                phdto.number = phone.number;
                phdto.cityCode = phone.cityCode;
                phdto.countryCode = phone.countryCode;
                phdto.preferred = phone.preferred;
                phdto.typeId = phone.typeId;
                phdto.typeDescription = db.PhoneTypes.FirstOrDefault(pt => pt.typeId == phone.typeId).typeDescription;
                phdto.contactDetailsId = phone.contactDetailsId;

                return Ok(phdto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone by phone Id!");
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

            if (id != phone.id)
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

            return CreatedAtRoute("DefaultApi", new { id = phone.id }, phone);
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
            return db.Phones.Count(e => e.id == id) > 0;
        }
    }
}