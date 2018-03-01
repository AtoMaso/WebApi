using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using WebApi.Models;
using System.Web.Http.Description;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http.Results;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/correspondences")]
    public class CorrespondencesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private AccountController acctr = new AccountController();
        private PersonalDetailsController pdctr = new PersonalDetailsController();

        // GET: api/Correspondences //with no status        
        public IHttpActionResult GetCorrespondences()
        {   
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences)
                {
                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();     
                                  
                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;       
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReciever = corres.traderIdReciever;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;

                    dtoList.Add(mesdto);
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all correspondence!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences/GetCorrespondencesWithStatus?status=sta        
        [Route("GetCorrespondencesWithStatus")]
        public IHttpActionResult GetCorrespondencesWithStatus(string status)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr .status == status))
                {
                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReciever = corres.traderIdReciever;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;


                    dtoList.Add(mesdto);
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all correspondence!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences?tradeId=1       
        [Route("GetCorrespondencesByTradeId")]
        public IHttpActionResult GetCorrespondencesByTradeId(int tradeId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.tradeId == tradeId).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReciever = corres.traderIdReciever;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;

                    dtoList.Add(mesdto);
                                 
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting correspondence by tradeId!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences/GetCorrespondencesByTradeIdWithStatus?tradeId=1&status=sta       
        [Route("GetCorrespondencesByTradeIdWithStatus")]
        public IHttpActionResult GetCorrespondencesByTradeIdWithStatus(int tradeId, string status)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.tradeId == tradeId && corr.status == status).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.FirstOrDefault(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReciever = corres.traderIdReciever;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;

                    dtoList.Add(mesdto);

                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting correspondence by tradeId!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences/GetCorrespondencesByTraderId?traderId ="wwewea534"     
        [Route("GetCorrespondencesByTraderId")]
        public IHttpActionResult GetCorrespondencesByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderIdReciever == traderId ).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReciever = corres.traderIdReciever;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;

                    dtoList.Add(mesdto);                  
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting correspondence by traderId!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences/GetCorrespondencesByTraderIdWithStatus?traderId ="wwewea534"&status=sta      
        [Route("GetCorrespondencesByTraderIdWithStatus")]
        public IHttpActionResult GetCorrespondencesByTraderIdWithStatus(string traderId, string status)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderIdReciever == traderId && corr.status == status).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReciever = corres.traderIdReciever;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;

                    dtoList.Add(mesdto);
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting correspondence by traderId!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/Correspondences/5       
        [ResponseType(typeof(CorrespondenceDTO))]
        public async Task<IHttpActionResult> GetCorrespondence(int id)
        {
            Correspondence corres = await db.Correspondences.FindAsync(id);
            if (corres == null)
            {
                ModelState.AddModelError("Message", "Correspondence not found!");
                return BadRequest(ModelState);
            }

            PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
            CorrespondenceDTO mesdto = new CorrespondenceDTO();

            mesdto.id = corres.id;
            mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
            mesdto.message = corres.message;
            mesdto.content = corres.content;
            mesdto.status = corres.status;
            mesdto.dateSent = corres.dateSent;
            mesdto.tradeId = corres.tradeId;
            mesdto.traderIdSender = corres.traderIdSender;
            mesdto.traderIdReciever = corres.traderIdReciever;
            mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;

            return Ok(mesdto);
        }


        // PUT: api/Correspondences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCorrespondence(int id, Correspondence correspondence)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The correspondence details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != correspondence.id)
            {
                ModelState.AddModelError("Message", "The correspondence id is not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(correspondence).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorrespondenceExists(id))
                {
                    ModelState.AddModelError("Message", "Correspondence not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Correspondences TODO change this as not anonimous
        [ResponseType(typeof(Correspondence))]       
        [Route("PostCorrespondence")]
        public async Task<IHttpActionResult> PostCorrespondence(Correspondence correspondence)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The correspondence details are not valid!");
                return BadRequest(ModelState);
            }        
            try
            {
                correspondence.dateSent = TimeZone.CurrentTimeZone.ToLocalTime(correspondence.dateSent);
                db.Correspondences.Add(correspondence);
                await db.SaveChangesAsync();

                //TODO do we need to return the DTO here???
                Correspondence trdhis = await db.Correspondences.OrderByDescending(trhis => trhis.id).FirstAsync();
                return Ok<Correspondence>(trdhis);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during storing the trade history!");
                return BadRequest(ModelState);
            }

            //return CreatedAtRoute("DefaultApi", new { id = correspondence.id }, correspondence);
        }


        // DELETE: api/Correspondences/5
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> DeleteCorrespondence(int id)
        {
            Correspondence correspondence = await db.Correspondences.FindAsync(id);
            if (correspondence == null)
            {
                ModelState.AddModelError("Message", "Correspondence not found!");
                return BadRequest(ModelState);
            }

            db.Correspondences.Remove(correspondence);
            await db.SaveChangesAsync();

            return Ok(correspondence);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool CorrespondenceExists(int id)
        {
            return db.Correspondences.Count(e => e.id == id) > 0;
        }
    }
}