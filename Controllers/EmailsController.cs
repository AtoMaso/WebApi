﻿using System;
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
                    emdto.preferred = email.preferred;
                    emdto.typeId = email.typeId;
                    emdto.typeDescription = db.EmailTypes.FirstOrDefault(emty => emty.typeId == email.typeId).typeDescription;                  
                    emdto.contactDetailsId = email.contactDetailsId;

                    dtoList.Add(emdto);
                }
                return Ok<List<EmailDTO>>(dtoList);
            }
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all phones!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Emails?contactDetailsId = 5
        public IHttpActionResult GetEmailsByContactDetailsId(int contactDetailsId)
        {
            try
            {
                List<EmailDTO> dtoList = new List<EmailDTO>();
                foreach (Email email in db.Emails)
                {
                    if(email.contactDetailsId == contactDetailsId)
                    {
                        EmailDTO emdto = new EmailDTO();

                        emdto.id = email.id;
                        emdto.account = email.account;
                        emdto.preferred = email.preferred;
                        emdto.typeId = email.typeId;
                        emdto.typeDescription = db.EmailTypes.FirstOrDefault(emty => emty.typeId == email.typeId).typeDescription;
                        emdto.contactDetailsId = email.contactDetailsId;

                        dtoList.Add(emdto);
                    }                                     
                }
                return Ok<List<EmailDTO>>(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all phones!");
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
                emdto.preferred = email.preferred;
                emdto.typeId = email.typeId;
                emdto.typeDescription = db.EmailTypes.FirstOrDefault(emty => emty.typeId == email.typeId).typeDescription;
                emdto.contactDetailsId = email.contactDetailsId;               

                return Ok(emdto);
            }
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone by phone Id!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/Emails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmail(int id, Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != email.id)
            {
                return BadRequest();
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

        // POST: api/Emails
        [ResponseType(typeof(Email))]
        public async Task<IHttpActionResult> PostEmail(Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Emails.Add(email);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = email.id }, email);
        }

        // DELETE: api/Emails/5
        [ResponseType(typeof(Email))]
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