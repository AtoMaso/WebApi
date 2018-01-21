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

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/trades")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class TradesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ImagesController imgctr = new ImagesController();
        private PersonalDetailsController pdctr = new PersonalDetailsController();
        private CategoriesController ctctr = new CategoriesController();
     
        //GET: api/trades
        [AllowAnonymous]
        public List<TradeDTO> GetTrades()
        {
            try
            {                                            
                List<TradeDTO> dtoList = new List<TradeDTO>();
                foreach (Trade trade in db.Trades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeTitle = trade.tradeTitle;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.tradeCategoryType = db.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryType;   //ctctr.GetCategoryByCategoryId(trade.categoryId).;
                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = db.PersonalDetails.FirstOrDefault( per => per.traderId == trade.traderId).firstName;   // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                    trdto.traderLastName = db.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;
                    trdto.Images = imgctr.GetImagesByTradeId(trade.tradeId);

                    dtoList.Add(trdto);
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all address!");
                return null; //BadRequest(ModelState);
            }      
        }


        //GET: api/trades?traderId=2    --  to get list of trades of a trader by traderid 
        [AllowAnonymous]
        public List<TradeDTO> GetTrades(string traderId)
        {
            try
            {
                List<TradeDTO> dtoList = new List<TradeDTO>();
                foreach (Trade trade in db.Trades)
                {
                    if(trade.traderId == traderId)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.tradeId = trade.tradeId;
                        trdto.tradeTitle = trade.tradeTitle;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.tradeCategoryType = db.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryType;   //ctctr.GetCategoryByCategoryId(trade.categoryId).;
                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = db.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;   // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                        trdto.traderLastName = db.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;
                        trdto.Images = imgctr.GetImagesByTradeId(trade.tradeId);

                        dtoList.Add(trdto);
                    }                  
                }
                return dtoList;
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all address!");
                return null; //BadRequest(ModelState);
            }
        }


        //GET api/trades/5  -- to get trade by the trade id
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
                    tradeId = trade.tradeId,
                    tradeTitle = trade.tradeTitle,
                    tradeDatePublished = trade.tradeDatePublished,                   
                    tradeCategoryType = db.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryType,   //ctctr.GetCategoryByCategoryId(trade.categoryId).;
                    traderId = trade.traderId,
                    traderFirstName = db.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName,  // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                    traderLastName = db.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName, //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;
                    Images = imgctr.GetImagesByTradeId(trade.tradeId)
                };             

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


        //PUT: api/trades/5
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
        [ResponseType(typeof(Trade))]
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
                var tradedto = new Trade()
                {
                    tradeId = trade.tradeId,
                    tradeTitle = trade.tradeTitle,                   
                    tradeDatePublished = trade.tradeDatePublished,                   
                    categoryId = trade.Category.categoryId,
                    Images = trade.Images
                    // put the images TODO
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


        // DELETE: api/trades/5
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
                        if (file.Name == attach.imageTitle)
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