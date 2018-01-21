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

namespace WebApi.Controllers
{
    public class PersonalDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AddressesController addrcnt = new AddressesController();

        // GET: api/personaldetails
        public List<PersonalDetailsDTO> GetPersonalDetails()
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
                    pddto.addresses = addrcnt.GetAddressesByPersonalId(pddto.personalDetailsId);              

                    // add the peersonal details to thee list
                    dtoList.Add(pddto);
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

        // GET: api/personaldetails?traderId=xx  - this is traderId
        public PersonalDetailsDTO GetPersonalDetailsByTraderId(string traderId)
        {
            try
            {
                //List<PersonalDetailsDTO> dtoList = new List<PersonalDetailsDTO>();

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
                        pddto.addresses = addrcnt.GetAddressesByPersonalId(pddto.personalDetailsId);

                        return pddto;
                        //dtoList.Add(pddto);
                    }                    
                }
                // return dtoList;      
                return null;
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // log the exc
                ModelState.AddModelError("Trade", "An unexpected error occured during getting all trades!");
                return null; // BadRequest(ModelState);
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
                pddto.addresses = addrcnt.GetAddressesByPersonalId(pddto.personalDetailsId);
               
                return Ok(pddto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the personal details!");
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