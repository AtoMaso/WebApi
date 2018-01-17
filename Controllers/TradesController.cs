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
using System.IO;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/trades")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class TradesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // DONE - WORKS
        //GET: api/Trades
        [AllowAnonymous]
        public IHttpActionResult GetTrades()
        {
            try
            {
                var tradesdto = from a in db.Trades
                                  select new TradeDTO()
                                  {                                      
                                      tradeId = a.tradeId,
                                      title = a.title,
                                      datePublished = a.datePublished,
                                      categoryType = db.Categories.Where(cat => cat.categoryId == a.categoryId).FirstOrDefault().categoryType,
                                      traderId = a.Trader.Id,                      
                                  };
                return Ok(tradesdto);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // log the exc
                ModelState.AddModelError("Trade", "An unexpected error occured during getting all trades!");
                return BadRequest(ModelState);
            }
        }


        // DONE - WORKS
        //GET: api/Trades?authorid=2      
        [AllowAnonymous]
        public IHttpActionResult GetTrades(string traderId)
        {
            try
            {
                var trades = from a in db.Trades
                               where (a.traderId.Equals(traderId))
                               select new TradeDTO()
                               {
                                   //Id = a.Id,
                                   tradeId = a.tradeId,
                                   title = a.title,
                                   datePublished = a.datePublished,
                                   categoryType = a.Category.categoryType,
                                   traderId = a.Trader.Id,                        
                               };
                return Ok(trades);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // log the exc
                ModelState.AddModelError("Trade", "An unexpected error occured during getting all trade!");
                return BadRequest(ModelState);
            }
        }



        // DONE - WORKS
        //GET api/Trades/5
        [ResponseType(typeof(TradeDetailDTO))]
        [AllowAnonymous]
        public IHttpActionResult GetTrade(int id)
        {
            var trade = db.Trades.Find(id);
            if (trade == null)
            {
                return NotFound();
            }
            try
            {
                TradeDetailDTO tradedto = new TradeDetailDTO()
                {
                    //Id = trade.Id,
                    tradeId = trade.tradeId,
                    title = trade.title,
                    datePublished = trade.datePublished,                   

                    categoryType = db.Categories.First(cat => cat.categoryId == trade.categoryId).categoryType,

                    traderId = db.Users.First(user => user.Id == trade.traderId).Id,
                };
                var attachments = from att in db.Images
                                  where att.tradeId == trade.tradeId
                                  select att;

                tradedto.Images = attachments.ToList();

                return Ok(tradedto);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // log the exc
                ModelState.AddModelError("Trade", "An error occured in getting the trade!");
                return BadRequest(ModelState);
            }
        }


        // TO DO UPDATE
        //PUT: api/Trades/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrade(int id, Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != trade.Id)
            if (id != trade.tradeId)
            {
                return BadRequest();
            }

            db.Entry(trade).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeExists(id))
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

        
        // POST: api/trades/PostTrade       
        [ResponseType(typeof(TradeDetailDTO))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostTrade")]
        public async Task<IHttpActionResult> PostTrade([FromBody] Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // add the trades' images first
                foreach (Image img in trade.Images) { db.Images.Add(img); }
                // add the trade now
                db.Trades.Add(trade);
                await db.SaveChangesAsync();

                // Load trader and category virtual properties
                db.Entry(trade).Reference(x => x.Trader).Load();
                db.Entry(trade).Reference(x => x.Category).Load();

                var tradedto = new TradeDetailDTO()
                {
                    tradeId = trade.tradeId,
                    title = trade.title,                   
                    datePublished = trade.datePublished,                   
                    categoryType = trade.Category.categoryType,
                    Images = trade.Images
                };

                return Ok(tradedto);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // TODO come up with logging solution here                
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during storing the trade!");
                return BadRequest(ModelState);
            }
        }



        // DELETE: api/Trades/5
        [ResponseType(typeof(Trade))]
        [Route("DeleteTrade")]
        public async Task<IHttpActionResult> DeleteTrade(int tradeId)
        {
            Trade trade = await db.Trades.FindAsync(tradeId);
            if (trade == null)
            {
                return NotFound();
            }

            try
            {
                // we are deleting the physical uploaded file (the attachements)
                DeletePhysicalTrade(tradeId);

                // remove the record from the attachements table
                db.Images.RemoveRange(trade.Images);

                // removing of the trade
                db.Trades.Remove(trade);
                await db.SaveChangesAsync();

                return Ok(trade);
            }
            catch (Exception exc)
            {
                // try to roll back the changes
                DbEntityEntry entry = db.Entry(trade);
                if (entry != null)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        default: break;
                    }
                }

                string mess = exc.Message;
                // TODO come up with logging solution here                
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during removing the trade!");
                return BadRequest(ModelState);
            }
        }


        public void DeletePhysicalTrade(int id) {

            Trade trade = db.Trades.Find(id);
            if (trade != null)
            {
                // remove the uploaded files on the server side.
                foreach (Image attach in trade.Images)
                {
                    string uploadDirectory = System.Web.HttpContext.Current.Server.MapPath("~");
                    uploadDirectory = Path.Combine(uploadDirectory + "Uploads");

                    DirectoryInfo dirInfo = new DirectoryInfo(uploadDirectory);
                    FileInfo[] Files = dirInfo.GetFiles("*.*");

                    foreach (FileInfo file in Files)
                    {
                        if (file.Name == attach.title)
                        {
                            file.Delete();
                        }
                    }
                }
            }          
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool TradeExists(int id)
        {
            return db.Trades.Count(e => e.tradeId == id) > 0;
        }
    }
}