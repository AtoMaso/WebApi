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
using WebApi.Controllers;
using System.Web.Http.Results;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/personaldetails")]
    public class PersonalDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AddressesController addrcnt = new AddressesController();

        // GET: api/personaldetails       
        public IHttpActionResult GetPersonalDetails()
        {
            try
            {
                List<PersonalDetailsDTO> dtoList = new List<PersonalDetailsDTO>();            

                foreach (PersonalDetails personaldetails in db.PersonalDetails)
                {                    
                    PersonalDetailsDTO pddto = new PersonalDetailsDTO();                    

                    pddto.personalDetailsId = personaldetails.personalDetailsId;
                    pddto.firstName = personaldetails.firstName;
                    pddto.middleName = personaldetails.middleName;
                    pddto.lastName = personaldetails.lastName;
                    pddto.dateOfBirth = personaldetails.dateOfBirth;
                    pddto.traderId = personaldetails.traderId;
                    pddto.addresses = ((OkNegotiatedContentResult<List<AddressDTO>>)addrcnt.GetAddressesByPersonalId(pddto.personalDetailsId)).Content;              
                  
                    dtoList.Add(pddto);
                }
                return Ok<List<PersonalDetailsDTO>>(dtoList);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all personal details!");
                return  BadRequest(ModelState);
            }
        }


        // GET: api/personaldetails?traderId=xx  - this is traderId      
        [AllowAnonymous]
        [Route("GetPersonalDetailsByTraderId")]       
        public IHttpActionResult GetPersonalDetailsByTraderId(string traderId)
        {

            if (!IsValidGUID(traderId))
            {
                ModelState.AddModelError("Message", "The user does not exist in the system!");
                return BadRequest(ModelState);
            }
            try
            {
                PersonalDetails personaldetails = db.PersonalDetails.First(pd => pd.traderId == traderId);
              
                PersonalDetailsDTO pddto = new PersonalDetailsDTO();

                pddto.personalDetailsId = personaldetails.personalDetailsId;
                pddto.firstName = personaldetails.firstName;
                pddto.middleName = personaldetails.middleName;
                pddto.lastName = personaldetails.lastName;
                pddto.dateOfBirth = personaldetails.dateOfBirth;
                pddto.traderId = personaldetails.traderId;
                pddto.addresses = ((OkNegotiatedContentResult<List<AddressDTO>>)addrcnt.GetAddressesByPersonalId(pddto.personalDetailsId)).Content;

                return Ok<PersonalDetailsDTO>(pddto);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                ModelState.AddModelError("Message", "An unexpected error occured during getting all personal details!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/personaldetails/5
        [ResponseType(typeof(PersonalDetails))]
        public async Task<IHttpActionResult> GetPersonalDetails(int id)
        {
            PersonalDetails personaldetails = await db.PersonalDetails.FindAsync(id);          
            if (personaldetails == null)
            {
                ModelState.AddModelError("Message", "Personal details not found!");
                return BadRequest(ModelState);
            }

            try
            {
                List<AddressDTO> addDtoList = new List<AddressDTO>();
                PersonalDetailsDTO pddto = new PersonalDetailsDTO();

                pddto.personalDetailsId = personaldetails.personalDetailsId;
                pddto.firstName = personaldetails.firstName;
                pddto.middleName = personaldetails.middleName;
                pddto.lastName = personaldetails.lastName;
                pddto.dateOfBirth = personaldetails.dateOfBirth;
                pddto.traderId = personaldetails.traderId;
                pddto.addresses = ((OkNegotiatedContentResult<List<AddressDTO>>)addrcnt.GetAddressesByPersonalId(pddto.personalDetailsId)).Content;

                return Ok(pddto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the personal details!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/PersonalDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonalDetails(int id, PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The personal details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != personalDetails.personalDetailsId)
            {
                ModelState.AddModelError("Message", "The personal details id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(personalDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalDetailsExists(id))
                {
                    ModelState.AddModelError("Message", "Personal details not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/PersonalDetails
        [ResponseType(typeof(PersonalDetails))]
        public async Task<IHttpActionResult> PostPersonalDetails(PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The personal details are not valid!");
                return BadRequest(ModelState);
            }

            db.PersonalDetails.Add(personalDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = personalDetails.personalDetailsId }, personalDetails);
        }


        // DELETE: api/PersonalDetails/5
        [ResponseType(typeof(PersonalDetails))]
        public async Task<IHttpActionResult> DeletePersonalDetails(int id)
        {
            PersonalDetails personalDetails = await db.PersonalDetails.FindAsync(id);
            if (personalDetails == null)
            {
                ModelState.AddModelError("Message", "Personal details not found!");
                return BadRequest(ModelState);
            }

            db.PersonalDetails.Remove(personalDetails);
            await db.SaveChangesAsync();

            return Ok(personalDetails);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool PersonalDetailsExists(int id)
        {
            return db.PersonalDetails.Count(e => e.personalDetailsId == id) > 0;
        }


        public static bool IsValidGUID(string s)
        {
            string pattern = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            return System.Text.RegularExpressions.Regex.IsMatch(s, pattern, System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}