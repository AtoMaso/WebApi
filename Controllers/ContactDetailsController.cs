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
    public class ContactDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ContactDetails
        public IHttpActionResult GetContactDetails()
        {
            try
            {
                List<ContactDetailsDTO> dtoList = new List<ContactDetailsDTO>();
                foreach (ContactDetails cd in db.ContactDetails)
                {
                    ContactDetailsDTO cddto = new ContactDetailsDTO();

                    cddto.contactDetailsId = cd.contactDetailsId;
                    cddto.traderId = cd.traderId;  
                    // phones dto here
                    // social network dto here                

                    dtoList.Add(cddto);
                }
                return Ok(dtoList);
            }
            catch(Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all contact details!");
                return BadRequest(ModelState);
            }         
        }

        // GET: api/ContactDetails/5
        [ResponseType(typeof(ContactDetails))]
        public async Task<IHttpActionResult> GetContactDetails(int id)
        {
            ContactDetails contactDetails = await db.ContactDetails.FindAsync(id);         
            if (contactDetails == null)
            {
                return NotFound();
            }

            try
            {
                ContactDetailsDTO dto = new ContactDetailsDTO();
                dto.contactDetailsId = contactDetails.contactDetailsId;
                dto.traderId = contactDetails.traderId;
                // phones dto here
                // social network dto here          
                return Ok(dto);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the contact details!");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/ContactDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContactDetails(int id, ContactDetails contactDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactDetails.contactDetailsId)
            {
                return BadRequest();
            }

            db.Entry(contactDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactDetailsExists(id))
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

        // POST: api/ContactDetails
        [ResponseType(typeof(ContactDetails))]
        public async Task<IHttpActionResult> PostContactDetails(ContactDetails contactDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContactDetails.Add(contactDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contactDetails.contactDetailsId }, contactDetails);
        }

        // DELETE: api/ContactDetails/5
        [ResponseType(typeof(ContactDetails))]
        public async Task<IHttpActionResult> DeleteContactDetails(int id)
        {
            ContactDetails contactDetails = await db.ContactDetails.FindAsync(id);
            if (contactDetails == null)
            {
                return NotFound();
            }

            db.ContactDetails.Remove(contactDetails);
            await db.SaveChangesAsync();

            return Ok(contactDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactDetailsExists(int id)
        {
            return db.ContactDetails.Count(e => e.contactDetailsId == id) > 0;
        }
    }
}