﻿using System;
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
                foreach (TradeForObject tradingFor in db.TradeForObjects)
                {
                    TradeForObjectDTO trddto = new TradeForObjectDTO();

                    trddto.tradeForObjectId = tradingFor.tradeForObjectId;
                    trddto.tradeForObjectDescription = tradingFor.tradeForObjectDescription;
                    trddto.objectCategoryId = tradingFor.objectCategoryId;
                    trddto.tradeForObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingFor.objectCategoryId).objectCategoryDescription;
                    trddto.tradeId = tradingFor.tradeId;

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

        // GET: api/TradeForObjects?categoryId=5
        public List<TradeForObjectDTO> GetTradeForObjectsByCategoryId(int categoryId)
        {
            try
            {
                List<TradeForObjectDTO> dtoList = new List<TradeForObjectDTO>();
                foreach (TradeForObject tradingFor in db.TradeForObjects)
                {
                    if(tradingFor.objectCategoryId == categoryId)
                    {
                        TradeForObjectDTO trddto = new TradeForObjectDTO();

                        trddto.tradeForObjectId = tradingFor.tradeForObjectId;
                        trddto.tradeForObjectDescription = tradingFor.tradeForObjectDescription;
                        trddto.objectCategoryId = tradingFor.objectCategoryId;
                        trddto.tradeForObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingFor.objectCategoryId).objectCategoryDescription;
                        trddto.tradeId = tradingFor.tradeId;

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

        // GET: api/tradeforobjects?tradeId=5
        public List<TradeForObjectDTO> GetTradeForObjectsByTradeId(int tradeId)
        {
            try
            {
                List<TradeForObjectDTO> dtoList = new List<TradeForObjectDTO>();
                foreach (TradeForObject tradingFor in db.TradeForObjects)
                {
                    if (tradingFor.tradeId == tradeId)
                    {
                        TradeForObjectDTO trddto = new TradeForObjectDTO();

                        trddto.tradeForObjectId = tradingFor.tradeForObjectId;
                        trddto.tradeForObjectDescription = tradingFor.tradeForObjectDescription;
                        trddto.objectCategoryId = tradingFor.objectCategoryId;
                        trddto.tradeForObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradingFor.objectCategoryId).objectCategoryDescription;
                        trddto.tradeId = tradingFor.tradeId;

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

                troobjdto.tradeForObjectId = tradeForObject.tradeForObjectId;
                troobjdto.tradeForObjectDescription = tradeForObject.tradeForObjectDescription;
                troobjdto.objectCategoryId = tradeForObject.objectCategoryId;
                troobjdto.tradeForObjectCategoryDescription = db.ObjectCategories.FirstOrDefault(cat => cat.objectCategoryId == tradeForObject.objectCategoryId).objectCategoryDescription;
                troobjdto.tradeId = tradeForObject.tradeId;

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