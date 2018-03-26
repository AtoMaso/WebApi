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
    [RoutePrefix("api/securitydetails")]
    public class SecurityDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
     
        // GET: api/securitydetails
        public IHttpActionResult GetSecurityDetails()
        {
            try
            {
                List<SecurityDetailsDTO> dtoList = new List<SecurityDetailsDTO>();

                foreach (SecurityDetails securitydetails in db.SecurityDetails)
                {
                    SecurityDetailsDTO scdto = new SecurityDetailsDTO();

                    scdto.securityDetailsId = securitydetails.securityDetailsId;
                    scdto.traderId = securitydetails.traderId;
                    scdto.password = db.Users.First(us => us.Id == securitydetails.traderId).PasswordHash;
                    scdto.userName = db.Users.First(us => us.Id == securitydetails.traderId).UserName;
                    scdto.email = db.Users.First(us => us.Id == securitydetails.traderId).Email;                  
                                    
                    dtoList.Add(scdto);
                }
                return Ok<List<SecurityDetailsDTO>>(dtoList);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all personal details!");
                return BadRequest(ModelState);
            }
        }



        // GET: api/securitydetails?traderId=5 - get the security details by traderId
        [AllowAnonymous]
        [Route("GetSecurityDetailsByTraderId")]
        public IHttpActionResult GetSecurityDetailsByTraderId(string traderId)
        {

            if (!IsValidGUID(traderId))
            {
                ModelState.AddModelError("Message", "The user does not exist in the system!");
                return BadRequest(ModelState);
            }
            try
            {               
                 SecurityDetails securitydetails = db.SecurityDetails.First(sd => sd.traderId == traderId);
                                
                SecurityDetailsDTO scdto = new SecurityDetailsDTO();
                scdto.securityDetailsId = securitydetails.securityDetailsId;
                scdto.traderId = securitydetails.traderId;
                scdto.password = db.Users.First(us => us.Id == securitydetails.traderId).PasswordHash;
                scdto.userName = db.Users.First(us => us.Id == securitydetails.traderId).UserName;
                scdto.email = db.Users.First(us => us.Id == securitydetails.traderId).Email;             
                return Ok<SecurityDetailsDTO>(scdto);
                                                               
            }
            catch (Exception)
            {                         
                ModelState.AddModelError("Message", "An unexpected error has occured during getting security details!");
                return BadRequest(ModelState);
            }
        }

        
        // GET: api/securitydetails/5   single security details by security details id
        [ResponseType(typeof(SecurityDetails))]
        public async Task<IHttpActionResult> GetSecurityDetails(int id)
        {
            SecurityDetails securitydetails = await db.SecurityDetails.FindAsync(id);
            if (securitydetails == null)
            {
                ModelState.AddModelError("Message", "Security details not found!");
                return BadRequest(ModelState);
            }

            try
            {
                SecurityDetailsDTO scdto = new SecurityDetailsDTO();

                scdto.securityDetailsId = securitydetails.securityDetailsId;
                scdto.traderId = securitydetails.traderId;
                scdto.password = db.Users.First(us => us.Id == securitydetails.traderId).PasswordHash;
                scdto.userName = db.Users.First(us => us.Id == securitydetails.traderId).UserName;
                scdto.email = db.Users.First(us => us.Id == securitydetails.traderId).Email;

                return Ok(scdto);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone details!");
                return BadRequest(ModelState);
            }           
        }


        // PUT: api/SecurityDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSecurityDetails(int id, SecurityDetails securityDetails)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The security details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != securityDetails.securityDetailsId)
            {
                ModelState.AddModelError("Message", "The security details id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Security details not found!");
                    return BadRequest(ModelState);
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
                ModelState.AddModelError("Message", "The security details are not valid!");
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
                ModelState.AddModelError("Message", "Security details not found!");
                return BadRequest(ModelState);
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


        public static bool IsValidGUID(string s)
        {
            string pattern = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            return System.Text.RegularExpressions.Regex.IsMatch(s, pattern, System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}