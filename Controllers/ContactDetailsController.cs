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
using System.Web.Http.Results;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/contactdetails")]
    public class ContactDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SocialNetworksController snctr = new SocialNetworksController();
        private PhonesController phctr = new PhonesController();
        private EmailsController emctr = new EmailsController();

        // GET: api/contactdetails     
        public IHttpActionResult GetContactDetails()
        {
            try
            {
                List<ContactDetailsDTO> dtoList = new List<ContactDetailsDTO>();
                foreach (ContactDetails contactdetails in db.ContactDetails)
                {
                    ContactDetailsDTO cddto = new ContactDetailsDTO();

                    cddto.contactDetailsId = contactdetails.contactDetailsId;
                    cddto.traderId = contactdetails.traderId;
                    cddto.emails = ((OkNegotiatedContentResult<List<EmailDTO>>)emctr.GetEmailsByContactDetailsId(contactdetails.contactDetailsId)).Content;
                    cddto.phones = ((OkNegotiatedContentResult<List<PhoneDTO>>)phctr.GetPhonesByContactId(contactdetails.contactDetailsId)).Content;
                    cddto.socialNetworks = ((OkNegotiatedContentResult<List<SocialNetworkDTO>>)snctr.GetSocialNetworksByContactId(contactdetails.contactDetailsId)).Content;

                    dtoList.Add(cddto);
                }
                return Ok<List<ContactDetailsDTO>>(dtoList);
            }
            catch(Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all contact details!");
                return BadRequest(ModelState);
            }         
        }


        // GET: api/contactdetails?traderId=5 -- by the traderId    
        [AllowAnonymous]
        [Route("GetContactDetailsByTraderId")]            
        public IHttpActionResult GetContactDetailsByTraderId(string traderId)
        {

            if (!IsValidGUID(traderId))
            {
                ModelState.AddModelError("Message", "The user does not exist in the system!");
                return BadRequest(ModelState);
            }
            try
            {
                ContactDetails contactdetails = db.ContactDetails.First(cd => cd.traderId == traderId);
             
                ContactDetailsDTO cddto = new ContactDetailsDTO();

                cddto.contactDetailsId = contactdetails.contactDetailsId;
                cddto.traderId = contactdetails.traderId;
                cddto.emails = ((OkNegotiatedContentResult<List<EmailDTO>>)emctr.GetEmailsByContactDetailsId(contactdetails.contactDetailsId)).Content;
                cddto.phones = ((OkNegotiatedContentResult<List<PhoneDTO>>)phctr.GetPhonesByContactId(contactdetails.contactDetailsId)).Content;
                cddto.socialNetworks = ((OkNegotiatedContentResult<List<SocialNetworkDTO>>)snctr.GetSocialNetworksByContactId(contactdetails.contactDetailsId)).Content;

                return Ok<ContactDetailsDTO>(cddto);           
            }
            catch (Exception)
            {                           
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all contact details!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/ContactDetails/5 by the contact details id       
        [ResponseType(typeof(ContactDetails))]     
        public async Task<IHttpActionResult> GetContactDetails(int id)
        {
            ContactDetails contactdetails = await db.ContactDetails.FindAsync(id);         
            if (contactdetails == null)
            {
                ModelState.AddModelError("Message", "Conta ct details not found!");
                return BadRequest(ModelState);
            }

            try
            {
                ContactDetailsDTO cddto = new ContactDetailsDTO();

                cddto.contactDetailsId = contactdetails.contactDetailsId;
                cddto.traderId = contactdetails.traderId;
                cddto.emails = ((OkNegotiatedContentResult<List<EmailDTO>>)emctr.GetEmailsByContactDetailsId(contactdetails.contactDetailsId)).Content;
                cddto.phones = ((OkNegotiatedContentResult<List<PhoneDTO>>)phctr.GetPhonesByContactId(contactdetails.contactDetailsId)).Content;
                cddto.socialNetworks = ((OkNegotiatedContentResult<List<SocialNetworkDTO>>)snctr.GetSocialNetworksByContactId(contactdetails.contactDetailsId)).Content;

                return Ok(cddto);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the contact details!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/ContactDetails/5
        [ResponseType(typeof(void))]        
        public async Task<IHttpActionResult> PutContactDetails(int id, ContactDetails contactDetails)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The contact details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != contactDetails.contactDetailsId)
            {
                ModelState.AddModelError("Message", "The contact details id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Contact details not found!");
                    return BadRequest(ModelState);
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
                ModelState.AddModelError("Message", "The contact details are not valid!");
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
                ModelState.AddModelError("Message", "Contact details not found!");
                return BadRequest(ModelState);
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


        public static bool IsValidGUID(string s)
        {
            string pattern = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            return System.Text.RegularExpressions.Regex.IsMatch(s, pattern, System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}