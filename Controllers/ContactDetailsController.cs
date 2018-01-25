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
        private SocialNetworksController snctr = new SocialNetworksController();
        private PhonesController phctr = new PhonesController();
        private EmailsController emctr = new EmailsController();

        // GET: api/contactdetails
        public List<ContactDetailsDTO> GetContactDetails()
        {
            try
            {
                List<ContactDetailsDTO> dtoList = new List<ContactDetailsDTO>();
                foreach (ContactDetails contactdetails in db.ContactDetails)
                {
                    ContactDetailsDTO cddto = new ContactDetailsDTO();

                    cddto.contactDetailsId = contactdetails.contactDetailsId;
                    cddto.traderId = contactdetails.traderId;
                    cddto.Emails = emctr.GetEmailsByContactDetailsId(contactdetails.contactDetailsId);
                    cddto.Phones = phctr.GetPhonesByContactId(contactdetails.contactDetailsId);
                    cddto.SocialNetworks = snctr.GetSocialNetworksByContactId(contactdetails.contactDetailsId);                   

                    dtoList.Add(cddto);
                }
                return dtoList;
            }
            catch(Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all contact details!");
                return null; // BadRequest(ModelState);
            }         
        }



        // GET: api/contactdetails?traderId=5 -- by the traderId 
        public ContactDetailsDTO GetContactDetailsByTraderId(string traderId)
        {
            try
            {              
                foreach (ContactDetails contactdetails in db.ContactDetails)
                {
                    
                    if (contactdetails.traderId == traderId)
                    {
                        ContactDetailsDTO cddto = new ContactDetailsDTO();

                        cddto.contactDetailsId = contactdetails.contactDetailsId;
                        cddto.traderId = contactdetails.traderId;
                        cddto.Emails = emctr.GetEmailsByContactDetailsId(contactdetails.contactDetailsId);
                        cddto.Phones = phctr.GetPhonesByContactId(contactdetails.contactDetailsId);
                        cddto.SocialNetworks = snctr.GetSocialNetworksByContactId(contactdetails.contactDetailsId);
                       
                        return cddto;            
                    }                                                       
                }
                return null;                
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all contact details!");
                return null; // BadRequest(ModelState);
            }
        }



        // GET: api/ContactDetails/5 by the contact details id
        [ResponseType(typeof(ContactDetails))]
        public async Task<IHttpActionResult> GetContactDetails(int id)
        {
            ContactDetails contactdetails = await db.ContactDetails.FindAsync(id);         
            if (contactdetails == null)
            {
                return NotFound();
            }

            try
            {
                ContactDetailsDTO cddto = new ContactDetailsDTO();

                cddto.contactDetailsId = contactdetails.contactDetailsId;
                cddto.traderId = contactdetails.traderId;
                cddto.Emails = emctr.GetEmailsByContactDetailsId(contactdetails.contactDetailsId);
                cddto.Phones = phctr.GetPhonesByContactId(contactdetails.contactDetailsId);
                cddto.SocialNetworks = snctr.GetSocialNetworksByContactId(contactdetails.contactDetailsId);
                return Ok(cddto);
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