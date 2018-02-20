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
        private CategoriesController ctctr = new CategoriesController();
        private TradeHistoriesController trhictr = new TradeHistoriesController();

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
                    trdto.placeId = trade.placeId;
                    trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                    trdto.stateId = trade.stateId;
                    trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                    trdto.subcategoryId = trade.subcategoryId;
                    trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;          
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName; 
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                    trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
        //GET: api/trades/GetAllTradesWithStatus?status="xxx"
        [AllowAnonymous]
        [Route("GetAllTradesWithStatus")]
        public IHttpActionResult GetAllTradesWithStatus(string sta = "Open")
        {
            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                int total = dbContext.Trades.Where(tr => tr.status == sta).Count();
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.status == sta))
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.placeId = trade.placeId;
                    trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                    trdto.stateId = trade.stateId;
                    trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                    trdto.subcategoryId = trade.subcategoryId;
                    trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;
                    // left as an example to be used
                    trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;   // the images per trade are taken by the images controlled                 

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



    
        //GET: api/trades/GetFilteredTradesWithStatus?categoryid=xx&fsubcategoryid=xx&stateid=xx&placeid=xx
        [AllowAnonymous]
        [Route("GetTradesWithSetFilters")]
        public IHttpActionResult GetTradesWithSetFilters( int categoryId, int subcategoryId , int stateId, int placeId)
        {

            List<TradeDTO> dtoList = new List<TradeDTO>();
            try
            {
                IQueryable<Trade> trades = dbContext.Trades;
                // category filter
                if (categoryId !=0 )
                    trades = trades.Where(tr => tr.categoryId == categoryId);

                // subcategory filter
                if (subcategoryId != 0)
                    trades = trades.Where(tr => tr.subcategoryId == subcategoryId);

                // state filter
                if (stateId != 0)
                    trades = trades.Where(tr => tr.stateId == stateId);

                // place filter
                if (placeId != 0)
                    trades = trades.Where(tr => tr.placeId == placeId).OrderByDescending(trd => trd.datePublished) ;


                foreach (Trade trade in trades) 
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.tradeId = trade.tradeId;
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.placeId = trade.placeId;
                    trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                    trdto.stateId = trade.stateId;
                    trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                    trdto.subcategoryId = trade.subcategoryId;
                    trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                    trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
                    trdto.placeId = trade.placeId;
                    trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                    trdto.stateId = trade.stateId;
                    trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                    trdto.subcategoryId = trade.subcategoryId;
                    trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                    trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
                    trdto.placeId = trade.placeId;
                    trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                    trdto.stateId = trade.stateId;
                    trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                    trdto.subcategoryId = trade.subcategoryId;
                    trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                    trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
                int total = dbContext.Trades.Where(tr => tr.traderId == traderId && tr.status == status).Count();
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.traderId == traderId &&  tr.status == status).OrderByDescending(tr => tr.datePublished))
                {                  
                        TradeDTO trdto = new TradeDTO();

                        trdto.total = total;
                        trdto.tradeId = trade.tradeId;
                        trdto.datePublished = trade.datePublished;
                        trdto.status = trade.status;
                        trdto.name = trade.name;
                        trdto.description = trade.description;
                        trdto.tradeFor = trade.tradeFor;
                        trdto.placeId = trade.placeId;
                        trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                        trdto.stateId = trade.stateId;
                        trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                        trdto.subcategoryId = trade.subcategoryId;
                        trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                        trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
                int total = dbContext.Trades.Where(tr => tr.traderId == traderId).Count();
                foreach (Trade trade in dbContext.Trades.Where(tr => tr.traderId == traderId).OrderByDescending(tr => tr.datePublished))
                {
                    TradeDTO trdto = new TradeDTO();

                    trdto.total = total;
                    trdto.tradeId = trade.tradeId;                
                    trdto.datePublished = trade.datePublished;
                    trdto.status = trade.status;
                    trdto.name = trade.name;
                    trdto.description = trade.description;
                    trdto.tradeFor = trade.tradeFor;
                    trdto.placeId = trade.placeId;
                    trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                    trdto.stateId = trade.stateId;
                    trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                    trdto.categoryId = trade.categoryId;
                    trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                    trdto.subcategoryId = trade.subcategoryId;
                    trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                    trdto.traderId = trade.traderId;
                    trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                    trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                    trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                    trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
                        trdto.placeId = trade.placeId;
                        trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                        trdto.stateId = trade.stateId;
                        trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                        trdto.subcategoryId = trade.subcategoryId;
                        trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                        trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

                        dtoList.Add(trdto);
                    }
                    return Ok(dtoList);
                }
                else
                {

                    // Get total number of records
                    int total = dbContext.Trades.Where(x => x.status == sta).Count();
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
                        trdto.placeId = trade.placeId;
                        trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                        trdto.stateId = trade.stateId;
                        trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                        trdto.subcategoryId = trade.subcategoryId;
                        trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                        trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
                        trdto.placeId = trade.placeId;
                        trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                        trdto.stateId = trade.stateId;
                        trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                        trdto.subcategoryId = trade.subcategoryId;
                        trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                        trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;
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
                        trdto.placeId = trade.placeId;
                        trdto.place = dbContext.Places.First(pl => pl.id == trade.placeId).name;
                        trdto.stateId = trade.stateId;
                        trdto.state = dbContext.States.First(st => st.id == trade.stateId).name;
                        trdto.categoryId = trade.categoryId;
                        trdto.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription;
                        trdto.subcategoryId = trade.subcategoryId;
                        trdto.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription;

                        trdto.traderId = trade.traderId;
                        trdto.traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName;
                        trdto.traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName;
                        trdto.traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName;

                        trdto.Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content;

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
        [ResponseType(typeof(TradeDetailDTO))]
        [AllowAnonymous]
        public IHttpActionResult GetTrade(int id)
        {
            var trade = dbContext.Trades.Find(id);
            if (trade == null)
            {
                ModelState.AddModelError("Message", "Trade not found!");
                return BadRequest(ModelState);
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
                    placeId = trade.placeId,
                    place = dbContext.Places.First(pl => pl.id == trade.placeId).name,
                    stateId = trade.stateId,
                    state = dbContext.States.First(st => st.id == trade.stateId).name,
                    categoryId = trade.categoryId,
                    categoryDescription = dbContext.Categories.First(cat => cat.categoryId == trade.categoryId).categoryDescription,
                    subcategoryId = trade.subcategoryId,
                    subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == trade.subcategoryId).subcategoryDescription,

                    traderId = trade.traderId,
                    traderFirstName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).firstName,
                    traderMiddleName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).middleName,
                    traderLastName = dbContext.PersonalDetails.First(per => per.traderId == trade.traderId).lastName,

                    Images = ((OkNegotiatedContentResult<List<ImageDTO>>)imgctr.GetImagesByTradeId(trade.tradeId)).Content
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
                newTrade.stateId = passedTrade.stateId;
                newTrade.placeId = passedTrade.placeId;
                newTrade.categoryId = passedTrade.categoryId;
                newTrade.subcategoryId = passedTrade.subcategoryId;                    
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
                trhis.createdDate = DateTime.Today;
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
                trade.stateId = passedTrade.stateId;
                trade.state = dbContext.States.First(st => st.id == newTrade.stateId).name;
                trade.placeId = passedTrade.placeId;
                trade.place = dbContext.Places.First(pl => pl.id == newTrade.placeId).name;
                trade.categoryId = newTrade.categoryId;
                trade.categoryDescription = dbContext.Categories.First(cat => cat.categoryId == newTrade.categoryId).categoryDescription;
                trade.subcategoryId = newTrade.subcategoryId;
                trade.subcategoryDescription = dbContext.Subcategories.First(subcat => subcat.subcategoryId == newTrade.subcategoryId).subcategoryDescription;
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