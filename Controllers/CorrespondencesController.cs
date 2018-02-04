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
        private AccountController acctr = new AccountController();

        // GET: api/Correspondences //with no status
        [AllowAnonymous]
        public IHttpActionResult GetCorrespondences()
        {   
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences)
                {
                    ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();     
                                  
                    mesdto.id = corres.id;
                    mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
                    mesdto.message = corres.message;             
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;       
                    mesdto.traderId = corres.traderId;
                    mesdto.sender = trader.personalDetails.firstName + " "  +  trader.personalDetails.middleName + " " + trader.personalDetails.lastName;
                

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
        [AllowAnonymous]
        [Route("GetCorrespondencesWithStatus")]
        public IHttpActionResult GetCorrespondencesWithStatus(string status)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr .status == status))
                {
                    ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
                    mesdto.message = corres.message;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderId = corres.traderId;
                    mesdto.sender = trader.personalDetails.firstName + " " + trader.personalDetails.middleName + " " + trader.personalDetails.lastName;


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
        [AllowAnonymous]
        [Route("GetCorrespondencesByTradeId")]
        public IHttpActionResult GetCorrespondencesByTradeId(int tradeId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.tradeId == tradeId).OrderByDescending(corr => corr.dateSent))
                {

                    ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
                    mesdto.message = corres.message;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderId = corres.traderId;
                    mesdto.sender = trader.personalDetails.firstName + " " + trader.personalDetails.middleName + " " + trader.personalDetails.lastName;

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
        [AllowAnonymous]
        [Route("GetCorrespondencesByTradeIdWithStatus")]
        public IHttpActionResult GetCorrespondencesByTradeIdWithStatus(int tradeId, string status)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.tradeId == tradeId && corr.status == status).OrderByDescending(corr => corr.dateSent))
                {

                    ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
                    mesdto.message = corres.message;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderId = corres.traderId;
                    mesdto.sender = trader.personalDetails.firstName + " " + trader.personalDetails.middleName + " " + trader.personalDetails.lastName;

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
        [AllowAnonymous]
        [Route("GetCorrespondencesByTraderId")]
        public IHttpActionResult GetCorrespondencesByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderId == traderId ).OrderByDescending(corr => corr.dateSent))
                {

                    ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
                    mesdto.message = corres.message;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderId = corres.traderId;
                    mesdto.sender = trader.personalDetails.firstName + " " + trader.personalDetails.middleName + " " + trader.personalDetails.lastName;

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
        [AllowAnonymous]
        [Route("GetCorrespondencesByTraderIdWithStatus")]
        public IHttpActionResult GetCorrespondencesByTraderIdWithStatus(string traderId, string status)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderId == traderId && corr.status == status).OrderByDescending(corr => corr.dateSent))
                {

                    ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
                    mesdto.message = corres.message;
                    mesdto.status = corres.status;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderId = corres.traderId;
                    mesdto.sender = trader.personalDetails.firstName + " " + trader.personalDetails.middleName + " " + trader.personalDetails.lastName;

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
        [AllowAnonymous]
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> GetCorrespondence(int id)
        {
            Correspondence corres = await db.Correspondences.FindAsync(id);
            if (corres == null)
            {
                return NotFound();
            }            

            ApplicationUserDetailDTO trader = ((OkNegotiatedContentResult<ApplicationUserDetailDTO>)acctr.GetTraders(corres.traderId)).Content;
            CorrespondenceDTO mesdto = new CorrespondenceDTO();

            mesdto.id = corres.id;
            mesdto.subject = db.TradeObjects.FirstOrDefault(tro => tro.tradeId == corres.tradeId).tradeObjectDescription;
            mesdto.message = corres.message;
            mesdto.status = corres.status;
            mesdto.dateSent = corres.dateSent;
            mesdto.tradeId = corres.tradeId;
            mesdto.traderId = corres.traderId;
            mesdto.sender = trader.personalDetails.firstName + " " + trader.personalDetails.middleName + " " + trader.personalDetails.lastName;

            return Ok(corres);
        }


        // PUT: api/Correspondences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCorrespondence(int id, Correspondence correspondence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != correspondence.id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Correspondences
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> PostCorrespondence(Correspondence correspondence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Correspondences.Add(correspondence);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = correspondence.id }, correspondence);
        }


        // DELETE: api/Correspondences/5
        [ResponseType(typeof(Correspondence))]
        public async Task<IHttpActionResult> DeleteCorrespondence(int id)
        {
            Correspondence correspondence = await db.Correspondences.FindAsync(id);
            if (correspondence == null)
            {
                return NotFound();
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