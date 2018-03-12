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
    [RoutePrefix("api/emails")]
    public class EmailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Emails
        public IHttpActionResult GetEmails()
        {
            try
            {
                List<EmailDTO> dtoList = new List<EmailDTO>();
                foreach (Email email in db.Emails)
                {
                    EmailDTO emdto = new EmailDTO();

                    emdto.id = email.id;
                    emdto.account = email.account;
                    emdto.preferredFlag = email.preferredFlag;
                    emdto.emailTypeId = email.emailTypeId;
                    emdto.emailType = db.EmailTypes.First(emty => emty.emailTypeId == email.emailTypeId).emailType;                  
                    emdto.traderId = email.traderId;

                    dtoList.Add(emdto);
                }
                return Ok<List<EmailDTO>>(dtoList);
            }
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all emails!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Emails/GetEmailsByTraderId?traderId=""     
        [Route("GetEmailsByTraderId")]
        public IHttpActionResult GetEmailsByTraderId(string traderId)
        {
            try
            {
                List<EmailDTO> dtoList = new List<EmailDTO>();
                foreach (Email email in db.Emails)
                {
                    if(email.traderId == traderId)
                    {
                        EmailDTO emdto = new EmailDTO();

                        emdto.id = email.id;
                        emdto.account = email.account;
                        emdto.preferredFlag = email.preferredFlag;
                        emdto.emailTypeId = email.emailTypeId;
                        emdto.emailType = db.EmailTypes.First(emty => emty.emailTypeId == email.emailTypeId).emailType;
                        emdto.traderId = email.traderId;

                        dtoList.Add(emdto);
                    }                                     
                }
                return Ok<List<EmailDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting email by trader id!");
                return BadRequest(ModelState);
            }
        }



        // GET: api/emails/GetPreferredEmail?traderId = "xx" &preferredFlag="Yes"
        [AllowAnonymous]   //  this is used on the trader details view not logged in trader
        [Route("GetPreferredEmail")]  
        public IHttpActionResult GetPreferredEmail(string traderId, string preferredFlag)
        {

            try
            {
                var eml = db.Emails.FirstOrDefault(sn => sn.traderId == traderId && sn.preferredFlag == preferredFlag);
                if (eml != null) { 
                    EmailDTO sndto = new EmailDTO();
                    sndto.id = eml.id;
                    sndto.account = eml.account;
                    sndto.preferredFlag = eml.preferredFlag;
                    sndto.emailTypeId = eml.emailTypeId;
                    sndto.emailType = db.EmailTypes.First(em => em.emailTypeId == eml.emailTypeId).emailType;
                    sndto.traderId = eml.traderId;

                    return Ok<EmailDTO>(sndto);
                }
                return Ok<Email>(new Email());                                     
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting preferred email by trader id!");
                return BadRequest(ModelState);
            }
        }

        // GET: api/Emails/5
        [ResponseType(typeof(Email))]
        public async Task<IHttpActionResult> GetEmail(int id)
        {
            Email email = await db.Emails.FindAsync(id);
            if (email == null)
            {
                ModelState.AddModelError("Message", "Email not found!");
                return BadRequest(ModelState);
            }

            try
            {
                EmailDTO emdto = new EmailDTO();

                emdto.id = email.id;
                emdto.account = email.account;
                emdto.preferredFlag = email.preferredFlag;
                emdto.emailTypeId = email.emailTypeId;
                emdto.emailType = db.EmailTypes.First(emty => emty.emailTypeId == email.emailTypeId).emailType;
                emdto.traderId = email.traderId;

                return Ok(emdto);
            }
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone by email id!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/Emails/PutEmail?id=5
        [ResponseType(typeof(void))]
        [HttpPut]
        [AcceptVerbs("PUT")]
        [Route("PutEmail")]
        public async Task<IHttpActionResult> PutEmail(int id, Email email)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The email details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != email.id)
            {
                ModelState.AddModelError("Message", "The emailid is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(email).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailExists(id))
                {
                    ModelState.AddModelError("Message", "Email not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Emails/PostEmail
        [ResponseType(typeof(Email))]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("PostEmail")]
        public async Task<IHttpActionResult> PostEmail(Email email)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The email details are not valid!");
                return BadRequest(ModelState);
            }

            db.Emails.Add(email);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = email.id }, email);
        }

        // DELETE: api/Emails/DeleteEmail/5
        [ResponseType(typeof(Email))]
        [Route("DeleteEmail")]
        public async Task<IHttpActionResult> DeleteEmail(int id)
        {
            Email email = await db.Emails.FindAsync(id);
            if (email == null)
            {
                ModelState.AddModelError("Message", "Email not found!");
                return BadRequest(ModelState);
            }

            db.Emails.Remove(email);
            await db.SaveChangesAsync();

            return Ok(email);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailExists(int id)
        {
            return db.Emails.Count(e => e.id == id) > 0;
        }
    }
}