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

    [Authorize]
    [RoutePrefix("api/phones")]
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
                    phdto.preferredFlag = phone.preferredFlag;
                    phdto.phoneTypeId = phone.phoneTypeId;
                    phdto.phoneType = db.PhoneTypes.First(pt => pt.phoneTypeId == phone.phoneTypeId).phoneType;
                    phdto.traderId = phone.traderId;

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


       
        // GET: localhost:5700/api/phones/GetPhonesByTraderId?traderId="s" - by contactDetailsId
        [Route("GetPhonesByTraderId")]
        public IHttpActionResult GetPhonesByTraderId(string traderId)
        {
            try
            {
                List<PhoneDTO> dtoList = new List<PhoneDTO>();
                foreach (Phone phone in db.Phones)
                {
                    if(phone.traderId == traderId)
                    {
                        PhoneDTO phdto = new PhoneDTO();

                        phdto.id = phone.id;
                        phdto.number = phone.number;
                        phdto.cityCode = phone.cityCode;
                        phdto.countryCode = phone.countryCode;
                        phdto.preferredFlag = phone.preferredFlag;
                        phdto.phoneTypeId = phone.phoneTypeId;
                        phdto.phoneType = db.PhoneTypes.First(pt => pt.phoneTypeId == phone.phoneTypeId).phoneType;
                        phdto.traderId = phone.traderId;

                        dtoList.Add(phdto);
                    }
                  
                }
                return Ok<List<PhoneDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting phones by trader id!");
                return BadRequest(ModelState);
            }
        }

        // GET: api/emails/GetPreferredPhone?traderId = "xx" &preferredFlag="Yes"
        [AllowAnonymous]
        [Route("GetPreferredPhone")]
        public IHttpActionResult GetPreferredPhone(string traderId, string preferredFlag)
        {

            try
            {
                var phn = db.Phones.FirstOrDefault(sn => sn.traderId == traderId && sn.preferredFlag == preferredFlag);
                if (phn != null)
                {
                    PhoneDTO phdto = new PhoneDTO();
                    phdto.id = phn.id;
                    phdto.number = phn.number;
                    phdto.cityCode = phn.cityCode;
                    phdto.countryCode = phn.countryCode;
                    phdto.preferredFlag = phn.preferredFlag;
                    phdto.phoneTypeId = phn.phoneTypeId;
                    phdto.phoneType = db.PhoneTypes.First(ph => ph.phoneTypeId == ph.phoneTypeId).phoneType;
                    phdto.traderId = phn.traderId;

                    return Ok<PhoneDTO>(phdto);
                }
                return Ok<Phone>(new Phone());
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting preferred phone by trader id!");
                return BadRequest(ModelState);
            }
        }


        // GET: localhost:5700/api/phones/5
        [ResponseType(typeof(Phone))]
        public async Task<IHttpActionResult> GetPhone(int id)
        {
            Phone phone = await db.Phones.FindAsync(id);           
            if (phone == null)
            {
                ModelState.AddModelError("Message", "Phone not found!");
                return BadRequest(ModelState);
            }

            try
            {
                PhoneDTO phdto = new PhoneDTO();

                phdto.id = phone.id;
                phdto.number = phone.number;
                phdto.cityCode = phone.cityCode;
                phdto.countryCode = phone.countryCode;
                phdto.preferredFlag = phone.preferredFlag;
                phdto.phoneTypeId = phone.phoneTypeId;
                phdto.phoneType = db.PhoneTypes.First(pt => pt.phoneTypeId == phone.phoneTypeId).phoneType;
                phdto.traderId = phone.traderId;

                return Ok(phdto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone by phone Id!");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Phones/PuPhone?id=5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [Route("PutPhone")]
        public async Task<IHttpActionResult> PutPhone(int id, Phone phone)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The phone details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != phone.id)
            {
                ModelState.AddModelError("Message", "The phone id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Phone not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            Phone lastPhone = db.Phones.Where(u => u.id  == id).First();

            PhoneDTO adddto = new PhoneDTO();
            adddto.id = lastPhone.id;
            adddto.number = lastPhone.number;
            adddto.cityCode = lastPhone.cityCode;
            adddto.countryCode = lastPhone.countryCode;
            adddto.preferredFlag = lastPhone.preferredFlag;
            adddto.phoneTypeId = lastPhone.phoneTypeId;
            adddto.phoneType = db.PhoneTypes.First(adt => adt.phoneTypeId == lastPhone.phoneTypeId).phoneType;
            adddto.traderId = lastPhone.traderId;

            return Ok<PhoneDTO>(adddto);
        }

        // POST: api/Phones/PostPhone
        [ResponseType(typeof(Phone))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostPhone")]
        public async Task<IHttpActionResult> PostPhone([FromBody] Phone phone)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The phone details are not valid!");
                return BadRequest(ModelState);
            }

            db.Phones.Add(phone);
            await db.SaveChangesAsync();
          
            Phone lastPhone = await db.Phones.OrderByDescending(u => u.id).FirstAsync();

            PhoneDTO adddto = new PhoneDTO();
            adddto.id = lastPhone.id;
            adddto.number = lastPhone.number;            
            adddto.cityCode = lastPhone.cityCode;         
            adddto.countryCode = lastPhone.countryCode;
            adddto.preferredFlag = lastPhone.preferredFlag;
            adddto.phoneTypeId = lastPhone.phoneTypeId;
            adddto.phoneType = db.PhoneTypes.First(adt => adt.phoneTypeId == lastPhone.phoneTypeId).phoneType;
            adddto.traderId = lastPhone.traderId;

            return Ok<PhoneDTO>(adddto);

        }



        // DELETE: api/Phones/DeletePhone?id=5
        [ResponseType(typeof(Phone))]
        [Route("DeletePhone")]
        public async Task<IHttpActionResult> DeletePhone(int id)
        {
            Phone phone = await db.Phones.FindAsync(id);
            if (phone == null)
            {
                ModelState.AddModelError("Message", "Phone not found!");
                return BadRequest(ModelState);
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