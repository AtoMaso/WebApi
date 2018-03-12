using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/personaldetails")]
    public class PersonalDetailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();      

        // GET: api/personaldetails           
        public IHttpActionResult GetPersonalDetails()
        {
            try
            {
                List<PersonalDetailsDTO> dtoList = new List<PersonalDetailsDTO>();            

                foreach (PersonalDetails personaldetails in db.PersonalDetails)
                {                    
                    PersonalDetailsDTO pddto = new PersonalDetailsDTO();                    

                    pddto.id = personaldetails.id;
                    pddto.firstName = personaldetails.firstName;
                    pddto.middleName = personaldetails.middleName;
                    pddto.lastName = personaldetails.lastName;
                    pddto.dateOfBirth = personaldetails.dateOfBirth;
                    pddto.traderId = personaldetails.traderId;                  
                  
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


        // GET: api/personaldetails/GetPersonalDetailsByTraderId?traderId=xx  - this is traderId                
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
                var personaldetails = db.PersonalDetails.FirstOrDefault(pd => pd.traderId == traderId);
                if (personaldetails != null) {
                        PersonalDetailsDTO pddto = new PersonalDetailsDTO();

                        pddto.id = personaldetails.id;
                        pddto.firstName = personaldetails.firstName;
                        pddto.middleName = personaldetails.middleName;
                        pddto.lastName = personaldetails.lastName;
                        pddto.dateOfBirth = personaldetails.dateOfBirth;
                        pddto.traderId = personaldetails.traderId;

                        return Ok<PersonalDetailsDTO>(pddto);
                    }
                return Ok<PersonalDetailsDTO>(new PersonalDetailsDTO());
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
                PersonalDetailsDTO pddto = new PersonalDetailsDTO();

                pddto.id = personaldetails.id;
                pddto.firstName = personaldetails.firstName;
                pddto.middleName = personaldetails.middleName;
                pddto.lastName = personaldetails.lastName;
                pddto.dateOfBirth = personaldetails.dateOfBirth;
                pddto.traderId = personaldetails.traderId;            

                return Ok(pddto);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the personal details!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/PutPersonalDetail?id=122
        [ResponseType(typeof(void))] 
        [AcceptVerbs("Put")]
        [Route("PutPersonalDetail")]
        public async Task<IHttpActionResult> PutPersonalDetails(int id, PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The personal details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != personalDetails.id)
            {
                ModelState.AddModelError("Message", "The personal details id is not valid!");
                return BadRequest(ModelState);
            }

            personalDetails.dateOfBirth = TimeZone.CurrentTimeZone.ToLocalTime(personalDetails.dateOfBirth);
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

            return Ok<PersonalDetails>(personalDetails);
        }


        // POST: api/PersonalDetails
        [ResponseType(typeof(PersonalDetails))]
        [HttpPost]
        [AcceptVerbs("POST")]      
        [Route("PostPersonalDetails")]
        public async Task<IHttpActionResult> PostPersonalDetails([FromBody]  PersonalDetails personalDetails)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The personal details are not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                personalDetails.dateOfBirth = TimeZone.CurrentTimeZone.ToLocalTime(personalDetails.dateOfBirth);
                db.PersonalDetails.Add(personalDetails);
                await db.SaveChangesAsync();

                PersonalDetails lastPersonal = await db.PersonalDetails.OrderByDescending(u => u.id).FirstAsync();

                return Ok<PersonalDetails>(lastPersonal);
            }
            catch(Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during storing the personal details!");
                return BadRequest(ModelState);
            }
                  
        }


        // DELETE: api/PersonalDetails/DeletePersonalDetails?id=5     
        [ResponseType(typeof(PersonalDetails))]
        [Route("DeletePersonalDetails")]
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
            return db.PersonalDetails.Count(e => e.id == id) > 0;
        }


        public static bool IsValidGUID(string s)
        {
            string pattern = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            return System.Text.RegularExpressions.Regex.IsMatch(s, pattern, System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}