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
        private CategoriesController ctctr = new CategoriesController();
        //private TradeObjectsController trobctr = new TradeObjectsController();
        //private TradeForObjectsController trfobctr = new TradeForObjectsController();

        //GOOD
        //GET: api/trades
        [AllowAnonymous]
        public IHttpActionResult GetTrades()
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                int total = dbContext.Trades.Count();
                foreach (Trade trade in dbContext.Trades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;                
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;
                 

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;          
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; 
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;                    

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
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;                  

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;   // the images per trade are taken by the images controlled
                    //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.status == status).OrderByDescending(x => x.datePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;
                  
                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;  
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName; 
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;      
                               
                   //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                    //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                foreach (Trade trade in dbContext.Trades.OrderByDescending(x => x.datePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;                  

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                    //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.traderId == traderId &&  tr.status == status).OrderByDescending(tr => tr.datePublished))
                {                  
                        TradeDTO trdto = new TradeDTO();

                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;
                   
                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;   
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;    

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.traderId == traderId).OrderByDescending(tr => tr.datePublished))
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;                
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;
                   
                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                    //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                    //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                    //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
        public IHttpActionResult GetPagesOfTradesWithStatus(string traderId, int setCounter = 1, int recordsPerSet = 50, string sta="Open")
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {               
                // Determine the number of records to skip
                int skip = (setCounter - 1) * recordsPerSet;
                            
                if (traderId != null) {

                    // Get total number of records
                    int total = dbContext.Trades.Where(x => x.traderId == traderId && x.status == sta).Count();
                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }

                    // Select the customers based on paging parameters
                    var alltradesTrader = dbContext.Trades.Where(x => x.traderId == traderId && x.status == sta)
                        .OrderByDescending(x => x.datePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltradesTrader)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.total = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;
                       
                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                    var alltrades = dbContext.Trades.Where(x => x.status == sta)
                        .OrderByDescending(x => x.datePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltrades)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.total = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;
                      
                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                        .OrderByDescending(x => x.datePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltradesTrader)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.total = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;                      

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                        .OrderByDescending(x => x.datePublished)
                        .Skip(skip)
                        .Take(recordsPerSet)
                        .ToList();

                    foreach (Trade trade in alltrades)
                    {
                        TradeDTO trdto = new TradeDTO();

                        trdto.total = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription;                    

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName;

                        //trdto.images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
                        //trdto.TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content;
                        //trdto.TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content;

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
                    datePublished = trade.datePublished,
                    status = trade.status,
                    name = trade.name,
                    description = trade.description,
                    tradeFor = trade.tradeFor,
                    categoryId = trade.categoryId,
                    categoryDescription = dbContext.Categories.FirstOrDefault(cat => cat.categoryId == trade.categoryId).categoryDescription,                 

                    traderId = trade.traderId,
                    traderFirstName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).firstName,  
                    traderMiddleName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).middleName,
                    traderLastName = dbContext.PersonalDetails.FirstOrDefault(per => per.traderId == trade.traderId).lastName,      
                          
                    //images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content,
                    //TradeObject = ((OkNegotiatedContentResult<TradeObjectDTO>)trobctr.GetTradeObjectsByTradeId(trade.tradeId)).Content,
                    //TradeForObject = ((OkNegotiatedContentResult<TradeForObjectDTO>)trfobctr.GetTradeForObjectsByTradeId(trade.tradeId)).Content
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
        [ResponseType(typeof(TradeDTO))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostTrade")]
        public async Task<IHttpActionResult> PostTrade([FromBody] TradeDTO passedTrade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {                            
                // ADD THE NEW TRADE based on the tradedto recived
                Trade newTrade = new Trade();
                newTrade.name = passedTrade.name;
                newTrade.description = passedTrade.description;
                newTrade.datePublished = passedTrade.datePublished;
                newTrade.status = passedTrade.status;
                newTrade.tradeFor = passedTrade.tradeFor;
                newTrade.categoryId = passedTrade.categoryId;                                 
                newTrade.traderId = passedTrade.traderId;

                dbContext.Trades.Add(newTrade);

                // TODO 
                //ADD IMAGES
                foreach (Image img in passedTrade.Images) { dbContext.Images.Add(img); }

                await dbContext.SaveChangesAsync();

                // Get the object to return back      
                var tradedto = new TradeDTO()
                {
                    tradeId = passedTrade.tradeId,
                    datePublished = passedTrade.datePublished,                  
                    traderId = passedTrade.traderId,                
                    Images = passedTrade.Images                
                };

                return Ok(tradedto);
            }
            catch (Exception exc)
            {               
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
                dbContext.Images.RemoveRange(trade.Images);

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