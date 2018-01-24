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
                foreach (TradeObject tradingObj in db.TradeObjects)
                {
                    TradeObjectDTO trddto = new TradeObjectDTO();

                    trddto.tradeObjectId = tradingObj.tradeObjectId;
                    trddto.tradeObjectDescription = tradingObj.tradeObjectDescription;
                    trddto.objectCategoryId= tradingObj.objectCategoryId;                 
                    trddto.tradeObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingObj.objectCategoryId).objectCategoryDescription;
                    trddto.tradeId = tradingObj.tradeId;

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

        // GET: api/tradeobjects?categoryId=5
        public List<TradeObjectDTO> GetTradeObjectsByCategoryId(int categoryId)
        {
            try
            {
                List<TradeObjectDTO> dtoList = new List<TradeObjectDTO>();
                foreach (TradeObject tradingObj in db.TradeObjects)
                {
                    if(tradingObj.objectCategoryId == categoryId)
                    {
                        TradeObjectDTO trddto = new TradeObjectDTO();

                        trddto.tradeObjectId = tradingObj.tradeObjectId;
                        trddto.tradeObjectDescription = tradingObj.tradeObjectDescription;
                        trddto.objectCategoryId = tradingObj.objectCategoryId;
                        trddto.tradeObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingObj.objectCategoryId).objectCategoryDescription;
                        trddto.tradeId = tradingObj.tradeId;

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


        // GET: api/tradeobjects?tradeId=5
        public List<TradeObjectDTO> GetTradeObjectsByTradeId(int tradeId)
        {
            try
            {
                List<TradeObjectDTO> dtoList = new List<TradeObjectDTO>();
                foreach (TradeObject tradingObj in db.TradeObjects)
                {
                    if (tradingObj.tradeId == tradeId)
                    {
                        TradeObjectDTO trddto = new TradeObjectDTO();

                        trddto.tradeObjectId = tradingObj.tradeObjectId;
                        trddto.tradeObjectDescription = tradingObj.tradeObjectDescription;
                        trddto.objectCategoryId = tradingObj.objectCategoryId;
                        trddto.tradeObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingObj.objectCategoryId).objectCategoryDescription;
                        trddto.tradeId = tradingObj.tradeId;

                        //return trddto;
                        dtoList.Add(trddto);
                    }
                }
                //return null;
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



        // GET: api/tradeobjects/5
        [ResponseType(typeof(TradeObject))]
        public async Task<IHttpActionResult> GetTradeObject(int id)
        {
            TradeObject tradingObj = await db.TradeObjects.FindAsync(id);
            if (tradingObj == null)
            {
                return NotFound();
            }

            try
            {
                TradeObjectDTO troobjdto = new TradeObjectDTO();

                troobjdto.tradeObjectId = tradingObj.tradeObjectId;
                troobjdto.tradeObjectDescription = tradingObj.tradeObjectDescription;
                troobjdto.objectCategoryId = tradingObj.objectCategoryId;
                troobjdto.tradeObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingObj.objectCategoryId).objectCategoryDescription;
                troobjdto.tradeId = tradingObj.tradeId;

                return Ok(troobjdto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the phone by phone Id!");
                return BadRequest(ModelState);
            }
        }



        // PUT: api/tradeobjects/5
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


        // POST: api/tradeobjects
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


        // DELETE: api/tradeobjects/5
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