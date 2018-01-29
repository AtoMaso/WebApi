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
        public IHttpActionResult GetPersonalDetailsByTraderId(string traderId)
        {
            try
            {            
                foreach (PersonalDetails personaldetails in db.PersonalDetails)
                {
                    if (personaldetails.traderId == traderId)
                    {
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
                }
                ModelState.AddModelError("Message", "An unexpected error occured during getting all trades!");
                return BadRequest(ModelState);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;               
                ModelState.AddModelError("Message", "An unexpected error occured during getting all trades!");
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
                return NotFound();
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
                return BadRequest(ModelState);
            }

            if (id != personalDetails.personalDetailsId)
            {
                return BadRequest();
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
                    return NotFound();
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
                return NotFound();
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
    }
}