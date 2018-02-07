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
    [RoutePrefix("api/trades")]
    // [UseSSL] this attribute is used to enforce using of the SSL connection to the webapi
    public class TradesController : ApiController
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        //private ImagesController imgctr = new ImagesController();
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
                    trdto.tradeStatus = trade.tradeStatus;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;          
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; 
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;    
                                               
                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;   // the images per trade are taken by the images controlled
                    trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {               
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }      
        }


        //GOOD
        //GET: api/trades
        [AllowAnonymous]
        [Route("GetAllTradesWithStatus")]
        public IHttpActionResult GetAllTradesWithStatus()
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                foreach (Trade trade in dbContext.Trades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.tradeStatus = trade.tradeStatus;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;   // the images per trade are taken by the images controlled
                    trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //GET: api/trades/GetFilteredTradesWithStatus?number=4&filter=tradeDatePublished&status="Open" // for Dashboard
        [AllowAnonymous]       
        [Route("GetFilteredTradesWithStatus")]
        public IHttpActionResult GetFilteredTradesWithStatus(int number, string status)
        {
           
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {                              
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.tradeStatus == status).OrderByDescending(x => x.tradeDatePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.tradeStatus = trade.tradeStatus;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;  
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; 
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;      
                               
                   //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                    trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all trades!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //GET: api/trades/GetFilteredTradesAll?number=4&filter=tradeDatePublished&status="Open" // for Dashboard
        [AllowAnonymous]
        [Route("GetFilteredTradesAll")]
        public IHttpActionResult GetFilteredTradesAll(int number)
        {

            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                foreach (Trade trade in dbContext.Trades.OrderByDescending(x => x.tradeDatePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.tradeStatus = trade.tradeStatus;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                    trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all trades!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //GET: api/trades/GetTradesWithStatus?traderId="djhfdsuhguhg"&status="Open"    --  to get list of trades of a trader by traderid 
        [AllowAnonymous]
        [Route("GetTradesByTraderIdWithStatus")]
        public IHttpActionResult GetTradesByTraderIdWithStatus(string traderId, string status = "Open")
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {                
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.traderId == traderId &&  tr.tradeStatus == status).OrderByDescending(tr => tr.tradeDatePublished))
                {                  
                        TradeDTO trdto = new TradeDTO();

                        trdto.tradeId = trade.tradeId;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.tradeStatus = trade.tradeStatus;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;   
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;    

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                 
                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all trades!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //GET: api/trades/GetAllTrades?traderId="djhfdsuhguhg"    --  to get list of trades of a trader by traderid 
        [AllowAnonymous]
        [Route("GetTradesByTraderId")]
        public IHttpActionResult GetTradesByTraderId(string traderId)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.traderId == traderId).OrderByDescending(tr => tr.tradeDatePublished))
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.tradeDatePublished = trade.tradeDatePublished;
                    trdto.tradeStatus = trade.tradeStatus;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                    trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                    dtoList.Add(trdto);

                }
                return Ok(dtoList);
            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all trades!");
                return BadRequest(ModelState);
            }
        }


        //GET: api/trades/GetPagesOfTrades?traderId=""&setCounter=5&recordsPerSet=10"&status="Open" - or "Closed"
        [AllowAnonymous]      
        [Route("GetPagesOfTradesWithStatus")]   // by traderId or Not 
        public IHttpActionResult GetPagesOfTradesWithStatus(string traderId, int setCounter = 1, int recordsPerSet = 50, string status="Open")
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {               
                // Determine the number of records to skip
                int skip = (setCounter - 1) * recordsPerSet;
                            
                if (traderId != null) {

                    // Get total number of records
                    int total = dbContext.Trades.Where(x => x.traderId == traderId && x.tradeStatus == status).Count();
                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }

                    // Select the customers based on paging parameters
                    var alltradesTrader = dbContext.Trades.Where(x => x.traderId == traderId && x.tradeStatus == status)
                        .OrderByDescending(x => x.tradeDatePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltradesTrader)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.totalTradesNumber = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.tradeStatus = trade.tradeStatus;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }
                else
                {

                    // Get total number of records
                    int total = dbContext.Trades.Count();
                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }
                   
                    // Select the customers based on paging parameters
                    var alltrades = dbContext.Trades.Where(x => x.tradeStatus == status)
                        .OrderByDescending(x => x.tradeDatePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltrades)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.totalTradesNumber = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.tradeStatus = trade.tradeStatus;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }

            }
            catch (Exception exc)
            {              
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }
        }


        //GET: api/trades/GetPagesOfTradesAll?traderId=""&setCounter=5&recordsPerSet=10"
        [AllowAnonymous]
        [Route("GetPagesOfTradesAll")]   // by traderId or Not 
        public IHttpActionResult GetPagesOfTradesAll(string traderId, int setCounter = 1, int recordsPerSet = 50)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                // Determine the number of records to skip
                int skip = (setCounter - 1) * recordsPerSet;

                if (traderId != null)
                {

                    // Get total number of records
                    int total = dbContext.Trades.Where(x => x.traderId == traderId).Count();
                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }

                    // Select the customers based on paging parameters
                    var alltradesTrader = dbContext.Trades.Where(x => x.traderId == traderId)
                        .OrderByDescending(x => x.tradeDatePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltradesTrader)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.totalTradesNumber = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.tradeStatus = trade.tradeStatus;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }
                else
                {

                    // Get total number of records
                    int total = dbContext.Trades.Count();
                    if ((skip >= total || setCounter < 0)  && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }

                    // Select the customers based on paging parameters
                    var alltrades = dbContext.Trades
                        .OrderByDescending(x => x.tradeDatePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltrades)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.totalTradesNumber = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.tradeDatePublished = trade.tradeDatePublished;
                        trdto.tradeStatus = trade.tradeStatus;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        trdto.tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        trdto.tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }

            }
            catch (Exception exc)
            {
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
                    tradeStatus = trade.tradeStatus,

                    traderId = trade.traderId,
                    traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName,  
                    traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName,
                    traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName,      
                          
                    //images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content,
                    tradeObjects = ((OkNegotiatedContentResult<List<TradeObjectDTO>>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content,
                    tradeForObjects = ((OkNegotiatedContentResult<List<TradeForObjectDTO>>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content
            }; 

                return Ok(tradedto);
            }
            catch (Exception exc)
            {
                string error = exc.Message;          
                ModelState.AddModelError("Message", "An error occured in getting the trade!");
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