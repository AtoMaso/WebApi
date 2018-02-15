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
        public IHttpActionResult GetTradeForObjects()
        {
            try
            {
                List<TradeForObjectDTO> dtoList = new List<TradeForObjectDTO>();
                foreach (TradeForObject tradingFor in db.TradeForObjects)
                {
                    TradeForObjectDTO trddto = new TradeForObjectDTO();

                    trddto.id = tradingFor.id;
                    trddto.name = tradingFor.name;
                    trddto.description = tradingFor.description;
                    trddto.categoryId = tradingFor.categoryId;
                    trddto.categoryDescription = db.Categories.FirstOrDefault(cat => cat.categoryId == tradingFor.categoryId).categoryDescription;
                    trddto.tradeId = tradingFor.tradeId;

                    dtoList.Add(trddto);
                }
                return Ok<List<TradeForObjectDTO>>(dtoList);           
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting tradings!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/TradeForObjects?categoryId=5
        public IHttpActionResult  GetTradeForObjectsByCategoryId(int catId)
        {
            try
            {
                List<TradeForObjectDTO> dtoList = new List<TradeForObjectDTO>();
                foreach (TradeForObject tradingFor in db.TradeForObjects)
                {
                    if(tradingFor.categoryId == catId)
                    {
                        TradeForObjectDTO trddto = new TradeForObjectDTO();


                        trddto.id = tradingFor.id;
                        trddto.name = tradingFor.name;
                        trddto.description = tradingFor.description;
                        trddto.categoryId = tradingFor.categoryId;
                        trddto.categoryDescription = db.Categories.FirstOrDefault(cat => cat.categoryId == tradingFor.categoryId).categoryDescription;
                        trddto.tradeId = tradingFor.tradeId;

                        dtoList.Add(trddto);
                    }                  
                }
                return Ok<List<TradeForObjectDTO>>(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting tradings!");
                return BadRequest(ModelState);
            }
        }


        // GET: api/tradeforobjects?tradeId=5
        public IHttpActionResult GetTradeForObjectsByTradeId(int tradeId)
        {
            try
            {               
                foreach (TradeForObject tradingFor in db.TradeForObjects)
                {
                    if (tradingFor.tradeId == tradeId)
                    {
                        TradeForObjectDTO trddto = new TradeForObjectDTO();


                        trddto.id = tradingFor.id;
                        trddto.name = tradingFor.name;
                        trddto.description = tradingFor.description;
                        trddto.categoryId = tradingFor.categoryId;
                        trddto.categoryDescription = db.Categories.FirstOrDefault(cat => cat.categoryId == tradingFor.categoryId).categoryDescription;
                        trddto.tradeId = tradingFor.tradeId;

                        return Ok<TradeForObjectDTO>(trddto);
                    }
                }
                ModelState.AddModelError("Message", "The object trading for can not be found!");
                return BadRequest(ModelState);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting tradings!");
                return BadRequest(ModelState);
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

            try
            {
                TradeForObjectDTO troobjdto = new TradeForObjectDTO();

                troobjdto.id = tradeForObject.id;
                troobjdto.name = tradeForObject.name;
                troobjdto.description = tradeForObject.description;
                troobjdto.categoryId = tradeForObject.categoryId;
                troobjdto.categoryDescription = db.Categories.FirstOrDefault(cat => cat.categoryId == tradeForObject.categoryId).categoryDescription;
                troobjdto.tradeId = tradeForObject.tradeId;

                return Ok(troobjdto);
            }
            catch (Exception exc)
            {
                // TODO come up with audit loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the phone by phone Id!");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/TradeForObjects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeForObject(int id, TradeForObject tradeForObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradeForObject.id)
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

            return CreatedAtRoute("DefaultApi", new { id = tradeForObject.id }, tradeForObject);
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
            return db.TradeForObjects.Count(e => e.id == id) > 0;
        }
    }
}