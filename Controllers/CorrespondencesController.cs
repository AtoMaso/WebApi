﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;
using System.Web.Http.Description;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Results;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/correspondences")]
    public class CorrespondencesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();      
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
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();     
                                  
                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;       
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr .statusSender == status))
                {
                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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


        // BY TRADE
        // GET: api/Correspondences/GetCorrespondencesByTradeId?tradeId=1       
        [Route("GetCorrespondencesByTradeId")]
        public IHttpActionResult GetCorrespondencesByTradeId(int tradeId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.tradeId == tradeId).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.tradeId == tradeId && corr.statusSender == status).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.FirstOrDefault(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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




        // BY TRADER
        // GET: api/Correspondences/GetInboxByTraderId?traderId ="wwewea534"     
        [Route("GetInboxByTraderId")]       
        public IHttpActionResult GetInboxByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderIdReceiver == traderId  && 
                                       (corr.statusReceiver == "New" || corr.statusReceiver == "Read" || corr.statusReceiver == "Replied")).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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


        // GET: api/Correspondences/GetSentByTraderId?traderId ="wwewea534"     
        [Route("GetSentByTraderId")]
        public IHttpActionResult GetSentByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderIdSender == traderId && corr.statusSender == "Sent").OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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


        // GET: api/Correspondences/GetArchivedInboxByTraderId?traderId ="wwewea534"&status=sta            
        [Route("GetArchivedInboxByTraderId")]
        public IHttpActionResult GetArchivedInboxByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
             
                    foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderIdReceiver == traderId && corr.statusReceiver == "Archived").OrderByDescending(corr => corr.dateSent))
                    {

                        PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                        PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                        CorrespondenceDTO mesdto = new CorrespondenceDTO();

                        mesdto.id = corres.id;
                        mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                        mesdto.message = corres.message;
                        mesdto.content = corres.content;
                        mesdto.statusSender = corres.statusSender;
                        mesdto.statusReceiver = corres.statusReceiver;
                        mesdto.dateSent = corres.dateSent;
                        mesdto.tradeId = corres.tradeId;
                        mesdto.traderIdSender = corres.traderIdSender;
                        mesdto.traderIdReceiver = corres.traderIdReceiver;
                        mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                        mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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


        // GET: api/Correspondences/GetArchivedSentByTraderId?traderId ="wwewea534"&status=sta            
        [Route("GetArchivedSentByTraderId")]
        public IHttpActionResult GetArchivedSentByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();

                foreach (Correspondence corres in db.Correspondences.Where(corr => corr.traderIdSender == traderId && corr.statusSender == "Archived").OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

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


        // GET: api/Correspondences/GetRemovedCorrespondenceByTraderId?traderId ="wwewea534"  
        [Route("GetRemovedCorrespondenceByTraderId")]
        public IHttpActionResult GetRemovedCorrespondenceByTraderId(string traderId)
        {
            try
            {
                List<CorrespondenceDTO> dtoList = new List<CorrespondenceDTO>();
                foreach (Correspondence corres in db.Correspondences.Where(corr => ((corr.traderIdSender == traderId || corr.traderIdReceiver == traderId) && 
                                                                                                (corr.statusSender == "Removed" || corr.statusReceiver == "Removed"))).OrderByDescending(corr => corr.dateSent))
                {

                    PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
                    PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
                    CorrespondenceDTO mesdto = new CorrespondenceDTO();

                    mesdto.id = corres.id;
                    mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
                    mesdto.message = corres.message;
                    mesdto.content = corres.content;
                    mesdto.statusSender = corres.statusSender;
                    mesdto.statusReceiver = corres.statusReceiver;
                    mesdto.dateSent = corres.dateSent;
                    mesdto.tradeId = corres.tradeId;
                    mesdto.traderIdSender = corres.traderIdSender;
                    mesdto.traderIdReceiver = corres.traderIdReceiver;
                    mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
                    mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

                    dtoList.Add(mesdto);
                }
                return Ok<List<CorrespondenceDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting deleted correspondence by traderId!");
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
            PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
            CorrespondenceDTO mesdto = new CorrespondenceDTO();

            mesdto.id = corres.id;
            mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
            mesdto.message = corres.message;
            mesdto.content = corres.content;
            mesdto.statusSender = corres.statusSender;
            mesdto.statusReceiver = corres.statusReceiver;
            mesdto.dateSent = corres.dateSent;
            mesdto.tradeId = corres.tradeId;
            mesdto.traderIdSender = corres.traderIdSender;
            mesdto.traderIdReceiver = corres.traderIdReceiver;
            mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
            mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

            return Ok(mesdto);
        }


        // GET: api/Correspondences/GetCorrespondenceByTradeIdAndId?loggedOnTrader="sdsd"&id=1      
        [ResponseType(typeof(CorrespondenceDTO))]
        [Route("GetCorrespondenceByTradeIdAndId")]
        public async Task<IHttpActionResult> GetCorrespondenceByTradeIdAndId(string loggedOnTrader, int id)
        {
            Correspondence corres = await db.Correspondences.FindAsync(id);
            if (corres == null)
            {
                ModelState.AddModelError("Message", "Correspondence not found!");
                return BadRequest(ModelState);
            }

            PersonalDetailsDTO personalDetailsSender = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdSender)).Content;
            PersonalDetailsDTO personalDetailsReciever = ((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(corres.traderIdReceiver)).Content;
         
            // TODO do this if the status is not already read!!!!
            if (loggedOnTrader == corres.traderIdSender) { corres.statusSender = "Sent";}
            else if (corres.statusReceiver == "New") { corres.statusReceiver = "Read"; }
         
            db.SaveChanges();


            CorrespondenceDTO mesdto = new CorrespondenceDTO();
            mesdto.id = corres.id;
            mesdto.subject = db.Trades.First(tro => tro.tradeId == corres.tradeId).name;
            mesdto.message = corres.message;
            mesdto.content = corres.content;
            mesdto.statusSender = corres.statusSender;
            mesdto.statusReceiver = corres.statusReceiver;
            mesdto.dateSent = corres.dateSent;
            mesdto.tradeId = corres.tradeId;
            mesdto.traderIdSender = corres.traderIdSender;
            mesdto.traderIdReceiver = corres.traderIdReceiver;
            mesdto.sender = personalDetailsSender.firstName + " " + personalDetailsSender.middleName + " " + personalDetailsSender.lastName;
            mesdto.receiver = personalDetailsReciever.firstName + " " + personalDetailsReciever.middleName + " " + personalDetailsReciever.lastName;

            return Ok(mesdto);
        }




        // PUT: api/Correspondences?id=5
        [ResponseType(typeof(void))]
        [HttpPut]
        [AcceptVerbs("PUT")]
        [Route("PutCorrespondence")]
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

            Correspondence corres = await db.Correspondences.Where(cor => cor.id == id).FirstAsync();
            return Ok<Correspondence>(corres);
           
        }


        // POST: api/Correspondences TODO change this as not anonimous
        [ResponseType(typeof(Correspondence))]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("PostCorrespondence")]
        public async Task<IHttpActionResult> PostCorrespondence([FromBody] Correspondence correspondence)
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