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
    public class SecurityDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SecurityAnswersController sactr = new SecurityAnswersController(); 
        
        // GET: api/securitydetails
        public List<SecurityDetailsDTO> GetSecurityDetails()
        {
            try
            {
                List<SecurityDetailsDTO> dtoList = new List<SecurityDetailsDTO>();

                foreach (SecurityDetails securitydetails in db.SecurityDetails)
                {
                    SecurityDetailsDTO scdto = new SecurityDetailsDTO();

                    scdto.securityDetailsId = securitydetails.securityDetailsId;
                    scdto.traderId = securitydetails.traderId;
                    scdto.password = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).PasswordHash;
                    scdto.userName = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).UserName;
                    scdto.email = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).Email;
                    scdto.securityAnswers = sactr.GetSecurityAnswersBySecurityId(securitydetails.securityDetailsId);
                   

                    // add the peersonal details to thee list
                    dtoList.Add(scdto);
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all personal details!");
                return null; // BadRequest(ModelState);
            }
        }



        // GET: api/securitydetails?traderId=5 - get the security details by traderId
        public SecurityDetailsDTO GetSecurityDetailsByTraderId(string traderId)
        {
            try
            {
                //List<SecurityDetailsDTO> dtoList = new List<SecurityDetailsDTO>();

                foreach (SecurityDetails securitydetails in db.SecurityDetails)
                {
                    if(securitydetails.traderId == traderId)
                    {
                        SecurityDetailsDTO scdto = new SecurityDetailsDTO();

                        scdto.securityDetailsId = securitydetails.securityDetailsId;
                        scdto.traderId = securitydetails.traderId;
                        scdto.password = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).PasswordHash;
                        scdto.userName = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).UserName;
                        scdto.email = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).Email;

                        scdto.securityAnswers = sactr.GetSecurityAnswersBySecurityId(securitydetails.securityDetailsId);

                        return scdto;
                    }
                   
                    // add the peersonal details to thee list
                    //dtoList.Add(scdto);
                }
                //return dtoList;
                return null;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all personal details!");
                return null; // BadRequest(ModelState);
            }
        }

        
        // GET: api/securitydetails/5   single security details by security details id
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> GetSecurityDetails(int id)
        {
            SecurityDetails securitydetails = await db.SecurityDetails.FindAsync(id);
            if (securitydetails == null)
            {
                return NotFound();
            }

            try
            {
                SecurityDetailsDTO scdto = new SecurityDetailsDTO();

                scdto.securityDetailsId = securitydetails.securityDetailsId;
                scdto.traderId = securitydetails.traderId;
                scdto.password = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).PasswordHash;
                scdto.userName = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).UserName;
                scdto.email = db.Users.FirstOrDefault(us => us.Id == securitydetails.traderId).Email;
                scdto.securityAnswers = sactr.GetSecurityAnswersBySecurityId(securitydetails.securityDetailsId);

                return Ok(scdto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the phone details!");
                return BadRequest(ModelState);
            }           
        }


        // PUT: api/SecurityDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSecurityDetails(int id, SecurityDetails securityDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != securityDetails.securityDetailsId)
            {
                return BadRequest();
            }

            db.Entry(securityDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityDetailsExists(id))
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


        // POST: api/SecurityDetails
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> PostSecurityDetails(SecurityDetails securityDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SecurityDetails.Add(securityDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = securityDetails.securityDetailsId }, securityDetails);
        }

        // DELETE: api/SecurityDetails/5
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> DeleteSecurityDetails(int id)
        {
            SecurityDetails securityDetails = await db.SecurityDetails.FindAsync(id);
            if (securityDetails == null)
            {
                return NotFound();
            }

            db.SecurityDetails.Remove(securityDetails);
            await db.SaveChangesAsync();

            return Ok(securityDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SecurityDetailsExists(int id)
        {
            return db.SecurityDetails.Count(e => e.securityDetailsId == id) > 0;
        }
    }
}