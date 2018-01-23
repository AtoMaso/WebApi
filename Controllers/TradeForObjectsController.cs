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
    public class TradeForObjectsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TradeForObjects
        public List<TradeForObjectDTO> GetTradeForObjects()
        {           
            try
            {
                List<TradeForObjectDTO> dtoList = new List<TradeForObjectDTO>();
                foreach (TradeForObject trading in db.TradeForObjects)
                {
                    TradeForObjectDTO trddto = new TradeForObjectDTO();

                    trddto.tradeForObjectId = trading.tradeForObjectId;
                    trddto.tradeForObjectName = trading.tradeForObjectName;
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


        // GET: api/TradeForObjects?tradeId=5
        public List<TradeForObjectDTO> GetTradeForObjectsByTradeId(int tradeId)
        {
            try
            {
                List<TradeForObjectDTO> dtoList = new List<TradeForObjectDTO>();
                foreach (TradeForObject tradForObject in db.TradeForObjects)
                {
                    if(tradForObject.tradeId == tradeId)
                    {
                        TradeForObjectDTO trddto = new TradeForObjectDTO();

                        trddto.tradeForObjectId = tradForObject.tradeForObjectId;
                        trddto.tradeForObjectName = tradForObject.tradeForObjectName;
                        trddto.categoryId = tradForObject.categoryId;
                        trddto.categoryType = db.Categories.FirstOrDefault(cat => cat.categoryId == tradForObject.categoryId).categoryType;

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

        // GET: api/TradeForObjects/5
        [ResponseType(typeof(TradeForObject))]
        public async Task<IHttpActionResult> GetTradeForObject(int id)
        {
            TradeForObject tradeForObject = await db.TradeForObjects.FindAsync(id);
            if (tradeForObject == null)
            {
                return NotFound();
            }

            return Ok(tradeForObject);
        }

        // PUT: api/TradeForObjects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeForObject(int id, TradeForObject tradeForObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradeForObject.tradeForObjectId)
            {
                return BadRequest();
            }

            db.Entry(tradeForObject).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeForObjectExists(id))
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

        // POST: api/TradeForObjects
        [ResponseType(typeof(TradeForObject))]
        public async Task<IHttpActionResult> PostTradeForObject(TradeForObject tradeForObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeForObjects.Add(tradeForObject);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradeForObject.tradeForObjectId }, tradeForObject);
        }

        // DELETE: api/TradeForObjects/5
        [ResponseType(typeof(TradeForObject))]
        public async Task<IHttpActionResult> DeleteTradeForObject(int id)
        {
            TradeForObject tradeForObject = await db.TradeForObjects.FindAsync(id);
            if (tradeForObject == null)
            {
                return NotFound();
            }

            db.TradeForObjects.Remove(tradeForObject);
            await db.SaveChangesAsync();

            return Ok(tradeForObject);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeForObjectExists(int id)
        {
            return db.TradeForObjects.Count(e => e.tradeForObjectId == id) > 0;
        }
    }
}