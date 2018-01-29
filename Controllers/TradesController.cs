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
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        private ImagesController imgctr = new ImagesController();
        private PersonalDetailsController pdctr = new PersonalDetailsController();
        private ObjectCategoriesController ctctr = new ObjectCategoriesController();
        private TradeObjectsController trobctr = new TradeObjectsController();
        private TradeForObjectsController trfobctr = new TradeForObjectsController();

        //GOOD
        //GET: api/trades
        [AllowAnonymous]
        public IHttpActionResult GetTrades()
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {                                                          
                foreach (Trade trade in dbContext.Trades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;                
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;   // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;

                    // TODO have a look do we need to remove images as the carousel component gets the images itself based on the tradeId
                    trdto.images = imgctr.GetImagesByTradeId(trade.tradeId);  
                    trdto.tradeObjects = (List<TradeObjectDTO>) trobctr.GetTradeObjectsByTradeId(trade.tradeId);
                    trdto.tradeForObjects = trfobctr.GetTradeForObjectsByTradeId(trade.tradeId);

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }      
        }

        //GOOD
        //GET: api/trades?number=4&filter=tradeDatePublished
        [AllowAnonymous]       
        public IHttpActionResult GetFilteredTrades(int number, string filter)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {                              
                foreach (Trade trade in dbContext.Trades.OrderByDescending(x => x.tradeDatePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;   // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;

                    // TODO have a look do we need to remove images as the carousel component gets the images itself based on the tradeId
                    trdto.images = imgctr.GetImagesByTradeId(trade.tradeId);
                    trdto.tradeObjects = (List<TradeObjectDTO>)  trobctr.GetTradeObjectsByTradeId(trade.tradeId);
                    trdto.tradeForObjects = trfobctr.GetTradeForObjectsByTradeId(trade.tradeId);

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //GET: api/trades?traderId="djhfdsuhguhg"    --  to get list of trades of a trader by traderid 
        [AllowAnonymous]
        public IHttpActionResult GetTrades(string traderId)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {                
                foreach (Trade trade in dbContext.Trades)
                {
                    if (trade.traderId == traderId)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.tradeId = trade.tradeId;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;   // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName; //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;

                        // TODO have a look do we need to remove images as the carousel component gets the images itself based on the tradeId
                        trdto.images = imgctr.GetImagesByTradeId(trade.tradeId);
                        trdto.tradeObjects = (List<TradeObjectDTO>) trobctr.GetTradeObjectsByTradeId(trade.tradeId);
                        trdto.tradeForObjects = trfobctr.GetTradeForObjectsByTradeId(trade.tradeId);

                        dtoList.Add(trdto);
                    }
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }
        }


        //GET: api/trades?page=5&perpage=10"
        [AllowAnonymous]      
        [Route("GetPagesOfTrades")]
        public IHttpActionResult GetPagesOfTrades(int page=1, int perpage = 50)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {               
                // Determine the number of records to skip
                int skip = (page - 1) * perpage;
                // Get total number of records
                int total = dbContext.Trades.Count();

                if(skip >= total ) {
                    ModelState.AddModelError("Message", "There are no more records!");                    
                    return BadRequest(ModelState);
                }

                // Select the customers based on paging parameters
                var alltrades = dbContext.Trades
                    .OrderByDescending(x => x.tradeDatePublished)                   
                    .Skip(skip)
                    .Take(perpage)
                    .ToList();
               
                    
                foreach (Trade trade in alltrades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;         // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;  //pdctr.GetPersonalDetailsByTraderId(trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;          //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;

                    // TODO have a look do we need to remove images as the carousel component gets the images itself based on the tradeId
                    trdto.images = imgctr.GetImagesByTradeId(trade.tradeId);
                    trdto.tradeObjects = (List<TradeObjectDTO>) trobctr.GetTradeObjectsByTradeId(trade.tradeId);
                    trdto.tradeForObjects = trfobctr.GetTradeForObjectsByTradeId(trade.tradeId);
 
                    //return Ok(trdto)
                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }
        }

        //GOOD
        //GET api/trades/5  -- to get trade by the trade id
        [ResponseType(typeof(TradeDetailDTO))]
        [AllowAnonymous]
        public IHttpActionResult GetTrade(int id)
        {
            var trade = dbContext.Trades.Find(id);
            if (trade == null)
            {
                return NotFound();
            }
            try
            {
                TradeDetailDTO tradedto = new TradeDetailDTO()
                {
                    tradeId = trade.tradeId,
                    tradeDatePublished = trade.tradeDatePublished,

                    traderId = trade.traderId,
                    traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName,  // pdctr.GetPersonalDetailsByTraderId(trade.traderId).firstName;
                    traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName, //pdctr.GetPersonalDetailsByTraderId(trade.traderId).middleName;
                    traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName, //pdctr.GetPersonalDetailsByTraderId(trade.traderId).lastName;

                    images = imgctr.GetImagesByTradeId(trade.tradeId), // TODO have a look do we need to remove images as the carousel component gets the images itself based on the tradeId
                    tradeObjects = (List<TradeObjectDTO>) trobctr.GetTradeObjectsByTradeId(trade.tradeId),
                    tradeForObjects = (List<TradeForObjectDTO>) trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)
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

            dbContext.Entry(trade).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
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

        
        // POST: api/trades/posttrade       
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
                foreach (Image img in trade.images) { dbContext.Images.Add(img); }
                // add the trade now
                dbContext.Trades.Add(trade);
                await dbContext.SaveChangesAsync();

                // Load trader and category virtual properties            
                var tradedto = new Trade()
                {
                    tradeId = trade.tradeId,
                    tradeDatePublished = trade.tradeDatePublished,
                  
                    traderId = trade.traderId,                
                    images = trade.images
                    // put the images TODO
                };

                return Ok(tradedto);
            }
            catch (Exception exc)
            {
                string error = exc.InnerException.Message;
                // TODO come up with logging solution here                
                ModelState.AddModelError("Message", "An unexpected error has occured during storing the trade!");
                return BadRequest(ModelState);
            }
        }


        // DELETE: api/trades/5
        [ResponseType(typeof(Trade))]
        [Route("DeleteTrade")]
        public async Task<IHttpActionResult> DeleteTrade(int tradeId)
        {
            Trade trade = await dbContext.Trades.FindAsync(tradeId);
            if (trade == null)
            {
                return NotFound();
            }

            try
            {
                // we are deleting the physical uploaded file (the attachements)
                DeletePhysicalTrade(tradeId);

                // remove the record from the attachements table
                dbContext.Images.RemoveRange(trade.images);

                // removing of the trade
                dbContext.Trades.Remove(trade);
                await dbContext.SaveChangesAsync();

                return Ok(trade);
            }
            catch (Exception exc)
            {
                // try to roll back the changes
                DbEntityEntry entry = dbContext.Entry(trade);
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
                ModelState.AddModelError("Message", "An unexpected error has occured during removing the trade!");
                return BadRequest(ModelState);
            }
        }


        public void DeletePhysicalTrade(int id) {

            Trade trade = dbContext.Trades.Find(id);
            if (trade != null)
            {
                // remove the uploaded files on the server side.
                foreach (Image attach in trade.images)
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
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool TradeExists(int id)
        {
            return dbContext.Trades.Count(e => e.tradeId == id) > 0;
        }
    }
}