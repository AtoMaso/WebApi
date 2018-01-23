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
    public class TradeObjectsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/tradeobjects
        public List<TradeObjectDTO> GetTradeObjects()
        {
            try
            {
                List<TradeObjectDTO> dtoList = new List<TradeObjectDTO>();
                foreach (TradeObject trading in db.TradeObjects)
                {
                    TradeObjectDTO trddto = new TradeObjectDTO();

                    trddto.tradeObjectId = trading.tradeObjectId;
                    trddto.tradeObjectName = trading.tradeObjectName;
                    trddto.categoryId = trading.categoryId;
                    trddto.categoryType = db.Categories.FirstOrDefault(cat => cat.categoryId == trading.categoryId).categoryType;

                    dtoList.Add(trddto);
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting tradings!");
                return null;// BadReq
            }
        }

        // GET: api/tradings?categoryId=5
        public List<TradeObjectDTO> GetTradeObjectsByCategoryId(int categoryId)
        {
            try
            {
                List<TradeObjectDTO> dtoList = new List<TradeObjectDTO>();
                foreach (TradeObject trading in db.TradeObjects)
                {
                    if(trading.categoryId == categoryId)
                    {
                        TradeObjectDTO trddto = new TradeObjectDTO();

                        trddto.tradeObjectId = trading.tradeObjectId;
                        trddto.tradeObjectName = trading.tradeObjectName;
                        trddto.categoryId = trading.categoryId;
                        trddto.categoryType = db.Categories.FirstOrDefault(cat => cat.categoryId == trading.categoryId).categoryType;

                        dtoList.Add(trddto);
                    }                   
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting tradings!");
                return null;// BadReq
            }
        }


        // GET: api/tradings/5
        [ResponseType(typeof(TradeObject))]
        public async Task<IHttpActionResult> GetTradeObject(int id)
        {
            TradeObject trading = await db.TradeObjects.FindAsync(id);
            if (trading == null)
            {
                return NotFound();
            }

            return Ok(trading);
        }

        // PUT: api/Tradings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeObject(int id, TradeObject trading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trading.tradeObjectId)
            {
                return BadRequest();
            }

            db.Entry(trading).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradingExists(id))
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

        // POST: api/Tradings
        [ResponseType(typeof(TradeObject))]
        public async Task<IHttpActionResult> PostTradeObject(TradeObject trading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeObjects.Add(trading);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = trading.tradeObjectId }, trading);
        }

        // DELETE: api/Tradings/5
        [ResponseType(typeof(TradeObject))]
        public async Task<IHttpActionResult> DeleteTradeObject(int id)
        {
            TradeObject trading = await db.TradeObjects.FindAsync(id);
            if (trading == null)
            {
                return NotFound();
            }

            db.TradeObjects.Remove(trading);
            await db.SaveChangesAsync();

            return Ok(trading);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradingExists(int id)
        {
            return db.TradeObjects.Count(e => e.tradeObjectId == id) > 0;
        }
    }
}