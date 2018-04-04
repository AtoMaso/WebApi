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
        private ImagesController imgctr = new ImagesController();
        private PersonalDetailsController pdctr = new PersonalDetailsController();       
        private SubcategoriesController subctr = new SubcategoriesController();
        private PlacesController plctr = new PlacesController();
        private PostcodesController pcctr = new PostcodesController();
        private TradeHistoriesController trhictr = new TradeHistoriesController();
        private DateTime Today = DateTime.Today;

        //GOOD
        //GET: api/trades
        [AllowAnonymous]
        public IHttpActionResult GetTrades()
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                ChangeTradeStatus(dbContext.Trades.ToList());

                // get only the trades which should be published        
                var trades = dbContext.Trades.Where(trd => trd.datePublished <= Today);
                int total = trades.Count();

                foreach (Trade trade in trades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;                
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;
                    trdto.category = trade.category;                 
                    trdto.subcategory = trade.subcategory;                   

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;          
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName; 
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                 
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

                    dtoList.Add(trdto);
                }
                return Ok(dtoList);
            }
            catch (Exception)
            {                              
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all address!");
                return BadRequest(ModelState);
            }      
        }


        //GOOD
        //GET: api/trades/GetTradesWithStatus?status="xxx"
        [AllowAnonymous]
        [Route("GetTradesWithStatus")]
        public IHttpActionResult GetTradesWithStatus(string status)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                ChangeTradeStatus(dbContext.Trades.ToList());

                // get only the trades which should be published              
                var trades = dbContext.Trades.Where(trd => trd.datePublished <= Today && trd.status == status);             
                int total = trades.Count();

                foreach (Trade trade in trades.OrderByDescending(trd => trd.datePublished))
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;
                    trdto.category = trade.category;
                    trdto.subcategory = trade.subcategory;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                    
                    // left as an example to be used
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;   // the images per trade are taken by the images controlled                 

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

    
        //GET: api/trades/GetTradesWithSetFilters?categoryid=xx&fsubcategoryid=xx&stateid=xx&placeid=xx - for tradelists
        [AllowAnonymous]
        [Route("GetTradesWithSetFilters")]
        public IHttpActionResult GetTradesWithSetFilters( string category, string subcategory , string state, string place, string postcode, string suburb)
        {

            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                IQueryable<Trade> trades = dbContext.Trades;
                // category filter
                // category filter
                if (category != "null")
                    trades = trades.Where(tr => tr.category == category);

                // subcategory filter
                if (subcategory != "null")
                    trades = trades.Where(tr => tr.subcategory == subcategory);

                // state filter
                if (state != "null")
                    trades = trades.Where(tr => tr.state == state);

                // place filter
                if (place != "null")
                    trades = trades.Where(tr => tr.place == place);

                // postcode filter
                if (postcode != "null")
                    trades = trades.Where(tr => tr.postcode == postcode);

                // suburb filter
                if (suburb != "null")
                    trades = trades.Where(tr => tr.suburb == suburb).OrderByDescending(trd => trd.datePublished);


                ChangeTradeStatus(dbContext.Trades.ToList());

                trades = trades.Where(trd => trd.datePublished <= Today);
                int total = trades.Count();

                foreach (Trade trade in trades) 
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;
                    trdto.category = trade.category;
                    trdto.subcategory = trade.subcategory;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                  
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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


        //GET: api/trades/GetSetOfTradesWithSetFilters?setCounter=z&recordsPerSet=xx&status="Open"&categoryid=xx&fsubcategoryid=xx&stateid=xx&placeid=xx&suburbId=xx -- for tradelist
        [AllowAnonymous]
        [Route("GetSetOfTradesWithSetFilters")]
        public IHttpActionResult GetSetOfTradesWithSetFilters(int setCounter, int recordsPerSet, string status, string category, string subcategory, string state, string place, string postcode, string suburb)
        {

            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                IQueryable<Trade> trades = dbContext.Trades;
                // category filter
                if (category != "null")
                    trades = trades.Where(tr => tr.category == category);

                // subcategory filter
                if (subcategory != "null")
                    trades = trades.Where(tr => tr.subcategory == subcategory);

                // state filter
                if (state != "null")
                    trades = trades.Where(tr => tr.state == state);

                // place filter
                if (place != "null")
                    trades = trades.Where(tr => tr.place == place);

                // postcode filter
                if (postcode != "null")
                    trades = trades.Where(tr => tr.postcode == postcode);

                // suburb filter
                if (suburb != "null")
                    trades = trades.Where(tr => tr.suburb == suburb).OrderByDescending(trd => trd.datePublished);


                ChangeTradeStatus(dbContext.Trades.ToList());

                // Determine the number of records to skip
                int skip = (setCounter - 1) * recordsPerSet;

                // get only the trades which should be published              
                trades = trades.Where(trd => trd.datePublished <= Today && trd.status == status);
              
                // Get total number of records
                int total = trades.Count();
                if ((skip >= total || setCounter < 0) && total != 0)
                {
                    ModelState.AddModelError("Message", "There are no more records!");
                    return BadRequest(ModelState);
                }

                // Select the customers based on paging parameters
                var alltrades = trades
                    .OrderByDescending(trd => trd.datePublished)
                    .Skip(skip)
                    .Take(recordsPerSet)
                    .ToList();

                foreach (Trade trade in trades)
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;                    
                    trdto.category = trade.category;
                    trdto.subcategory = trade.subcategory;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
        //GET: api/trades/GetLimitedTradesWithStatus?number=4&filter=tradeDatePublished&status="Open"  - for Dashboard
        [AllowAnonymous]       
        [Route("GetLimitedTradesWithStatus")]
        public IHttpActionResult GetLimitedTradesWithStatus(int number, string status)
        {
           
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                ChangeTradeStatus(dbContext.Trades.ToList()); 

                var trades = dbContext.Trades.Where(trd => trd.datePublished <= Today && trd.status == status);
                int total = trades.Count();

                foreach (Trade trade in trades.OrderByDescending(trd => trd.datePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();
                    // we do not need total here as we are getting limited number of trades
                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;
                    trdto.category = trade.category;
                    trdto.subcategory = trade.subcategory;
                    trdto.traderId = trade.traderId;

                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                  
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
        //GET: api/trades/GetLimitedTradesNoStatus?number=4   - for Dashboard
        [AllowAnonymous]
        [Route("GetLimitedTradesNoStatus")]
        public IHttpActionResult GetLimitedTradesNoStatus(int number)
        {

            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                ChangeTradeStatus(dbContext.Trades.ToList());

                var trades = dbContext.Trades.Where(trd => trd.datePublished <= Today);
                int total = trades.Count();

                foreach (Trade trade in trades.OrderByDescending(trd => trd.datePublished).Take(number)) //  we get only the number of trades ordered by date published
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;
                    trdto.category = trade.category;
                    trdto.subcategory = trade.subcategory;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
        //GET: api/trades/GetTradesByTraderIdWithStatus?traderId="djhfdsuhguhg"&status="Open"    --  my trade list
        [AllowAnonymous]
        [Route("GetTradesByTraderIdWithStatus")]
        public IHttpActionResult GetTradesByTraderIdWithStatus(string traderId, string status)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                ChangeTradeStatus(dbContext.Trades.ToList());

                // the trader needs to see all trades
                var trades = dbContext.Trades.Where(trd => trd.traderId == traderId && trd.status == status);                            
                int total = trades.Count();

                foreach (Trade trade in trades.OrderByDescending(trd => trd.datePublished))
                {                  
                        TradeDTO trdto = new TradeDTO();

                        trdto.total = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        //trdto.stateId = trade.stateId;
                        trdto.state = trade.state;
                        //trdto.placeId = trade.placeId;
                        trdto.place = trade.place;
                        //trdto.postcodeId = trade.postcodeId;
                        trdto.postcode = trade.postcode;
                        //trdto.suburbId = trade.suburbId;
                        trdto.suburb = trade.suburb;
                        trdto.category = trade.category;
                        trdto.subcategory = trade.subcategory;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                     
                        //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
        //GET: api/trades/GetTradesByTraderIdNoStatus?traderId="djhfdsuhguhg"    --my trade list 
        [AllowAnonymous]
        [Route("GetTradesByTraderIdNoStatus")]
        public IHttpActionResult GetTradesByTraderIdNoStatus(string traderId)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                ChangeTradeStatus(dbContext.Trades.ToList());

                // no date limit here as trader needs to see all trades
                var trades = dbContext.Trades.Where(trd => trd.traderId == traderId);             
                int total = trades.Count();

                foreach (Trade trade in trades.OrderByDescending(trd => trd.datePublished))
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;                
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    //trdto.stateId = trade.stateId;
                    trdto.state = trade.state;
                    //trdto.placeId = trade.placeId;
                    trdto.place = trade.place;
                    //trdto.postcodeId = trade.postcodeId;
                    trdto.postcode = trade.postcode;
                    //trdto.suburbId = trade.suburbId;
                    trdto.suburb = trade.suburb;
                    trdto.category = trade.category;
                    trdto.subcategory = trade.subcategory;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                   
                    //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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


        //GET: api/trades/GetSetOfTradesWithStatus?setCounter=5&recordsPerSet=10"&status="Open"
        [AllowAnonymous]      
        [Route("GetSetOfTradesWithStatus")]  
        public IHttpActionResult GetSetOfTradesWithStatus(int setCounter, int recordsPerSet, string status)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                    ChangeTradeStatus(dbContext.Trades.ToList());

                    // Determine the number of records to skip
                    int skip = (setCounter - 1) * recordsPerSet;
                                  

                    var trades = dbContext.Trades.Where(trd => trd.datePublished <= Today && trd.status == status);                                                                             
                    int total = trades.Count();

                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }
                
                    // Select the customers based on paging parameters
                    var alltrades = trades
                            .OrderByDescending(trd => trd.datePublished)
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
                        //trdto.stateId = trade.stateId;
                        trdto.state = trade.state;
                        //trdto.placeId = trade.placeId;
                        trdto.place = trade.place;
                        //trdto.postcodeId = trade.postcodeId;
                        trdto.postcode = trade.postcode;
                        //trdto.suburbId = trade.suburbId;
                        trdto.suburb = trade.suburb;                     
                        trdto.category = trade.category;
                        trdto.subcategory = trade.subcategory;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                  
                       // trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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


        //GET: api/trades/GetSetOfTradesWithStatusForTrader?traderId="asasas"&setCounter=5&recordsPerSet=10"&status="Open" 
        [AllowAnonymous]
        [Route("GetSetOfTradesWithStatusForTrader")]   
        public IHttpActionResult GetSetOfTradesWithStatusForTrader(string traderId, int setCounter, int recordsPerSet, string status)
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                    ChangeTradeStatus(dbContext.Trades.ToList());

                    // Determine the number of records to skip
                    int skip = (setCounter - 1) * recordsPerSet;

                    // no date limit for trader                  
                    var trades = dbContext.Trades.Where(trd => trd.traderId == traderId && trd.status == status);
                    int total = trades.Count();

                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }               

                // Select the customers based on paging parameters
                var alltradesTrader = trades
                            .OrderByDescending(trd => trd.datePublished)
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
                        //trdto.stateId = trade.stateId;
                        trdto.state = trade.state;
                        //trdto.placeId = trade.placeId;
                        trdto.place = trade.place;
                        //trdto.postcodeId = trade.postcodeId;
                        trdto.postcode = trade.postcode;
                        //trdto.suburbId = trade.suburbId;
                        trdto.suburb = trade.suburb;
                        trdto.category = trade.category;
                        trdto.subcategory = trade.subcategory;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                     
                        //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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


        //GET: api/trades/GetSetOfTradesNoStatusl?traderId=""&setCounter=5&recordsPerSet=10"
        [AllowAnonymous]
        [Route("GetSetOfTradesNoStatus")]   // by traderId or Not 
        public IHttpActionResult GetSetOfTradesNoStatus(string traderId, int setCounter, int recordsPerSet )
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                 ChangeTradeStatus(dbContext.Trades.ToList());

                // Determine the number of records to skip
                int skip = (setCounter - 1) * recordsPerSet;

                if (traderId != null)
                {                                    
                    // no date limit for trader                 
                    var trades = dbContext.Trades.Where(trd => trd.traderId == traderId);
                    int total = trades.Count();

                    if ((skip >= total || setCounter < 0) && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }

                    // Select the customers based on paging parameters
                    var alltradesTrader = trades
                        .OrderByDescending(trd => trd.datePublished)
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
                        //trdto.stateId = trade.stateId;
                        trdto.state = trade.state;
                        //trdto.placeId = trade.placeId;
                        trdto.place = trade.place;
                        //trdto.postcodeId = trade.postcodeId;
                        trdto.postcode = trade.postcode;
                        //trdto.suburbId = trade.suburbId;
                        trdto.suburb = trade.suburb;
                        trdto.category = trade.category;
                        trdto.subcategory = trade.subcategory;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                      
                        //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }
                else
                {
                    
                    ChangeTradeStatus(dbContext.Trades.ToList());

                    // get only the trades which should be published
                    var trades = dbContext.Trades.Where(trd => trd.datePublished <= Today);
                    int total = trades.Count();

                    if ((skip >= total || setCounter < 0)  && total != 0)
                    {
                        ModelState.AddModelError("Message", "There are no more records!");
                        return BadRequest(ModelState);
                    }

                    // Select the customers based on paging parameters
                    var alltrades = trades
                        .OrderByDescending(trd => trd.datePublished)
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
                        //trdto.stateId = trade.stateId;
                        trdto.state = trade.state;
                        //trdto.placeId = trade.placeId;
                        trdto.place = trade.place;
                        //trdto.postcodeId = trade.postcodeId;
                        trdto.postcode = trade.postcode;
                        //trdto.suburbId = trade.suburbId;
                        trdto.suburb = trade.suburb;
                        trdto.category = trade.category;
                        trdto.subcategory = trade.subcategory;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                     
                        //trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }

            }
            catch (Exception exc)
            {
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all trades!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //GET api/trades/5  -- to get trade by the trade id
        [ResponseType(typeof(TradeDTO))]
        [AllowAnonymous]
        public IHttpActionResult GetTrade(int id)
        {         
            var trade = dbContext.Trades.Find(id);
            if (trade == null)
            {
                ModelState.AddModelError("Message", "Trade with that id was not found!");
                return BadRequest(ModelState);
            }
            try
            {
                TradeDTO trdto = new TradeDTO()
                {                 
                    tradeId = trade.tradeId,
                    datePublished = trade.datePublished,
                    status = trade.status,
                    name = trade.name,
                    description = trade.description,
                    tradeFor = trade.tradeFor,
                    //trdto.stateId = trade.stateId;
                    state = trade.state,
                    //placeId = trade.placeId,
                    place = trade.place,
                    //postcodeId = trade.postcodeId,
                    postcode = trade.postcode,
                    //suburbId = trade.suburbId,
                    suburb = trade.suburb,
                    category = trade.category,
                    subcategory = trade.subcategory,

                    traderId = trade.traderId,
                    traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName,
                    traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName,
                    traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName,                                        
                    //Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content
                };
            
                return Ok(trdto);
            }
            catch (Exception exc)
            {
                string error = exc.Message;          
                ModelState.AddModelError("Message", "An error occured in getting the trade!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        //PUT: api/trades/5 - this is update
        [AcceptVerbs("PUT")]
        [ResponseType(typeof(void))]      
        public async Task<IHttpActionResult> PutTrade(int id, Trade trade)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "Trade details are not valid!");
                return BadRequest(ModelState);
            }
        
            if (id != trade.tradeId)
            {
                ModelState.AddModelError("Message", "The trade id is not valid!");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("Message", "Trade not found!");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception exc)
            {
                string str = exc.InnerException.Message;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        
        //GOOD
        // POST: api/trades/posttrade   - this is add trade
        [ResponseType(typeof(TradeDTO))]
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("PostTrade")]
        public async Task<IHttpActionResult> PostTrade([FromBody] TradeDTO passedTrade)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "Trade data provided is not valid!");
                return BadRequest(ModelState);
            }
            try
            {                            
                // ADD TRADE based on the passed trade received
                Trade newTrade = new Trade();
                newTrade.name = passedTrade.name;
                newTrade.description = passedTrade.description;
                newTrade.datePublished = TimeZone.CurrentTimeZone.ToLocalTime( passedTrade.datePublished);
                newTrade.status = passedTrade.status;
                newTrade.tradeFor = passedTrade.tradeFor;
                //newTrade.stateId = passedTrade.stateId;
                //newTrade.placeId = passedTrade.placeId;
                //newTrade.postcodeId = passedTrade.postcodeId;
                //newTrade.suburbId = passedTrade.suburbId;
                newTrade.state = passedTrade.state;
                newTrade.place = passedTrade.place;
                newTrade.postcode = passedTrade.postcode;
                newTrade.suburb = passedTrade.suburb;
                newTrade.category = passedTrade.category;
                newTrade.subcategory = passedTrade.subcategory;                    
                newTrade.traderId = passedTrade.traderId;

                dbContext.Trades.Add(newTrade);
                await dbContext.SaveChangesAsync();
              
                //ADD IMAGES - get the last 
                Trade lastTrade = await dbContext.Trades.OrderByDescending(u => u.tradeId).FirstOrDefaultAsync();

                int counter = 1;
                foreach (ImageDTO imgdto in passedTrade.Images) {
                    char[] delimiters = new char[] { '.' };
                    string[] filenames = imgdto.imageTitle.Split(delimiters);

                    Image img = new Image();
                    img.tradeId = lastTrade.tradeId;
                    img.imageTitle = "trade" + lastTrade.tradeId.ToString() + "_" + counter + "." + filenames[filenames.Length-1];
                    img.imageUrl = imgdto.imageUrl + "trade" + lastTrade.tradeId.ToString() + "/" +  img.imageTitle;
                    counter++;

                    dbContext.Images.Add(img);
                }

                // ADD HISTORY creation record                 
                TradeHistory trhis = new TradeHistory();
                trhis.tradeId = newTrade.tradeId;
                trhis.status = "Created";
                trhis.viewer = "Owner";
                trhis.createdDate = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now);
                await trhictr.PostTradeHistory(trhis);

                // store the changes
                await dbContext.SaveChangesAsync();

                // prepare the post trade object to be sent back
                TradeDTO trade = new TradeDTO();
                trade.tradeId = newTrade.tradeId;
                trade.name = newTrade.name;
                trade.description = newTrade.description;
                trade.datePublished = newTrade.datePublished;
                trade.tradeFor = newTrade.tradeFor;
                // trade.stateId = passedTrade.stateId;
                trade.state = newTrade.state;
                //trade.placeId = passedTrade.placeId;              
                trade.place = newTrade.place;
                //trade.postcodeId = newTrade.postcodeId;
                trade.postcode = newTrade.postcode;
                //trade.suburbId = newTrade.suburbId;
                trade.suburb = newTrade.suburb;
                trade.category = newTrade.category;                      
                trade.subcategory = newTrade.subcategory;           
                trade.traderId = newTrade.traderId;
                trade.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(newTrade.tradeId)).Content;

                return Ok<TradeDTO>(trade);
            }
            catch (Exception)
            {             
                ModelState.AddModelError("Message", "An unexpected error has occured during storing the trade!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        // DELETE: api/trades/DeleteTrade?tradeId=1
        [ResponseType(typeof(Trade))]
        [Route("DeleteTrade")]
        public async Task<IHttpActionResult> DeleteTrade(int tradeId)
        {
            Trade trade = await dbContext.Trades.FindAsync(tradeId);
            if (trade == null)
            {
                ModelState.AddModelError("Message", "Trade not found!");
                return BadRequest(ModelState);
            }

            try
            {
                // we are deleting the physical uploaded file (the images)
                DeletePhysicalImages(tradeId);
            
                // removing of the trade
                dbContext.Trades.Remove(trade);

                await dbContext.SaveChangesAsync();

                return Ok(trade);
            }
            catch (Exception)
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
            
                ModelState.AddModelError("Message", "An unexpected error has occured during removing the trade!");
                return BadRequest(ModelState);
            }
        }


        //GOOD
        public void DeletePhysicalImages(int id) {

                var images = dbContext.Images.Where(img => img.tradeId == id);
                // one file is what we need to get the folder
                foreach (Image img in images)
                {                    
                    string[] imagesFolder = img.imageTitle.Split('_');
                    string uploadDirectory = System.Web.HttpContext.Current.Server.MapPath("~");
                    uploadDirectory = Path.Combine(uploadDirectory + "Uploads/images/" + imagesFolder[0]);

                    // remove all files from the folder
                    DirectoryInfo dirInfo = new DirectoryInfo(uploadDirectory);
                    FileInfo[] Files = dirInfo.GetFiles("*.*");

                    foreach (FileInfo file in Files)
                    {
                        if (file.Name == img.imageTitle)
                        {
                            file.Delete();
                        }
                    }

                    // delete the folder also             
                    dirInfo.Delete(true);
                    // only ones and jump
                    break;
                }                          
                    
        }


        private void ChangeTradeStatus(List<Trade> trades)
        {
            foreach (Trade trdto in trades) {
                if (trdto.datePublished < DateTime.Today && trdto.status != "Open" && trdto.status != "Closed")
                {
                    Trade trd = (from x in dbContext.Trades
                                         where x.tradeId == trdto.tradeId
                                         select x).First();
                    trd.status = "Open";
                    dbContext.SaveChanges();
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