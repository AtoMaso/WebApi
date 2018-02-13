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

namespace WebApi.Controllers
{
    public class TradeHistoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TradeHistories
        public IHttpActionResult GetTradeHistories()
        {           
            try
            {
                List<TradeHistory> dtoList = new List<TradeHistory>();
                foreach (TradeHistory history in db.TradeHistories)
                {
                    TradeHistory hisdto = new TradeHistory();

                    hisdto.historyId = history.historyId;
                    hisdto.tradeId = history.tradeId;
                    hisdto.createdDate = history.createdDate;
                    hisdto.status = history.status;

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
        public IHttpActionResult GetTradeHistoriesByTradeId(int tradeId)
        {
            try
            {
                List<TradeHistory> dtoList = new List<TradeHistory>();
                foreach (TradeHistory history in db.TradeHistories)
                {
                    if(history.tradeId == tradeId)
                    {
                        TradeHistory hisdto = new TradeHistory();

                        hisdto.historyId = history.historyId;
                        hisdto.tradeId = history.tradeId;
                        hisdto.createdDate = history.createdDate;
                        hisdto.status = history.status;

                        dtoList.Add(hisdto);
                    }                  
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
                return NotFound();
            }

             try
            {                            
                TradeHistory hisdto = new TradeHistory();

                hisdto.historyId = history.historyId;
                hisdto.tradeId = history.tradeId;
                hisdto.createdDate = history.createdDate;
                hisdto.status = history.status;

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
                return BadRequest(ModelState);
            }

            if (id != tradeHistory.historyId)
            {
                return BadRequest();
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
                    return NotFound();
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
        public async Task<IHttpActionResult> PostTradeHistory(TradeHistory tradeHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeHistories.Add(tradeHistory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradeHistory.historyId }, tradeHistory);
        }

        // DELETE: api/TradeHistories/5
        [ResponseType(typeof(TradeHistory))]
        public async Task<IHttpActionResult> DeleteTradeHistory(int id)
        {
            TradeHistory tradeHistory = await db.TradeHistories.FindAsync(id);
            if (tradeHistory == null)
            {
                return NotFound();
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