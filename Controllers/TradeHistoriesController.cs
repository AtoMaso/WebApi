using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.IO;
using System.Web.Http.Results;

namespace WebApi.Controllers
{

    [Authorize]
    [RoutePrefix("api/tradehistories")]
    public class TradeHistoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();       

        // GET: api/TradeHistories
        [AllowAnonymous]
        public IHttpActionResult GetTradeHistories()
        {           
            try
            {
                List<TradeHistory> dtoList = new List<TradeHistory>();
                foreach (TradeHistory history in db.TradeHistories.OrderByDescending(trhis => trhis.historyId))
                {
                    TradeHistory hisdto = new TradeHistory();

                    hisdto.historyId = history.historyId;
                    hisdto.tradeId = history.tradeId;
                    hisdto.createdDate = history.createdDate;
                    hisdto.status = history.status;
                    hisdto.viewer = history.viewer;

                    dtoList.Add(hisdto);
                }
                return Ok<List<TradeHistory>>(dtoList);
            }
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting history!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/TradeHistories?tradeId=5
        [AllowAnonymous]
        [Route("GetTradeHistoriesByTradeId")]
        public IHttpActionResult GetTradeHistoriesByTradeId(int tradeId)
        {
            try
            {               
                List<TradeHistory> dtoList = new List<TradeHistory>();
                foreach (TradeHistory history in db.TradeHistories.Where(his=> his.tradeId == tradeId).OrderByDescending(trhis => trhis.createdDate))
                {                  
                    TradeHistory hisdto = new TradeHistory();

                    hisdto.historyId = history.historyId;
                    hisdto.tradeId = history.tradeId;
                    hisdto.createdDate = history.createdDate;
                    hisdto.status = history.status;
                    hisdto.viewer = history.viewer;

                    dtoList.Add(hisdto);

                }
                return Ok<List<TradeHistory>>(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting history!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/TradeHistories/5
        [ResponseType(typeof(TradeHistory))]
        public IHttpActionResult GetTradeHistory(int id)
        {
            TradeHistory history = db.TradeHistories.FirstOrDefault(x => x.historyId == id);
            if (history == null)
            {
                ModelState.AddModelError("Message", "Trade history not found!");
                return BadRequest(ModelState);
            }

             try
            {                            
                TradeHistory hisdto = new TradeHistory();

                hisdto.historyId = history.historyId;
                hisdto.tradeId = history.tradeId;
                hisdto.createdDate = history.createdDate;
                hisdto.status = history.status;
                hisdto.viewer = history.viewer;

                return Ok<TradeHistory>(hisdto);
            }                         
            catch (Exception exc)
            {                
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting history!");
                return BadRequest(ModelState);
            }
        }


        // PUT: api/TradeHistories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeHistory(int id, TradeHistory tradeHistory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The trade history details are not valid!");
                return BadRequest(ModelState);
            }

            if (id != tradeHistory.historyId)
            {
                ModelState.AddModelError("Message", "The trade history id are not valid!");
                return BadRequest(ModelState);
            }

            db.Entry(tradeHistory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeHistoryExists(id))
                {
                    ModelState.AddModelError("Message", "Trade history not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/TradeHistories
        [ResponseType(typeof(TradeHistory))]
        [AllowAnonymous]
        [Route("PostTradeHistory")]
        public async Task<IHttpActionResult> PostTradeHistory(TradeHistory tradeHistory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The trade history details are not valid!");
                return BadRequest(ModelState);
            }
            try
            {
                tradeHistory.createdDate = TimeZone.CurrentTimeZone.ToLocalTime(tradeHistory.createdDate);
                db.TradeHistories.Add(tradeHistory);
                await db.SaveChangesAsync();

                // keep only the latest 10 records for each trade   
                var histories = db.TradeHistories.Where(his => his.tradeId == tradeHistory.tradeId).OrderByDescending(his => his.createdDate).Skip(10);
                db.TradeHistories.RemoveRange(histories);
                await db.SaveChangesAsync();

                TradeHistory trdhis = await db.TradeHistories.OrderByDescending(trhis =>trhis.historyId).FirstAsync();
                return Ok<TradeHistory>(trdhis);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "An unexpected error has occured during storing the trade history!");
                return BadRequest(ModelState);
            }
        }



        // DELETE: api/TradeHistories/5
        [ResponseType(typeof(TradeHistory))]
        public async Task<IHttpActionResult> DeleteTradeHistory(int id)
        {
            TradeHistory tradeHistory = await db.TradeHistories.FindAsync(id);
            if (tradeHistory == null)
            {
                ModelState.AddModelError("Message", "Trade history not found!");
                return BadRequest(ModelState);
            }

            db.TradeHistories.Remove(tradeHistory);
            await db.SaveChangesAsync();

            return Ok(tradeHistory);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool TradeHistoryExists(int id)
        {
            return db.TradeHistories.Count(e => e.historyId == id) > 0;
        }
    }
}