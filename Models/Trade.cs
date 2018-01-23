using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models
{
    //[Serializable]    
    public class Trade
    {
        [Key]
        public int tradeId { get; set; }
              
        [Required]
        public DateTime tradeDatePublished { get; set; }      
        
                        

        [Required]
        public int tradeObjectId { get; set; }

        //public TradeObject TradeObject { get; set; }


        [Required]
        public string traderId { get; set; }
        
        public ApplicationUser Trader { get; set; }



        //public IQueryable<TradeForObject> TradeForObjects { get; set; }

          
        public IQueryable<Image> Images { get; set; }

        public Trade() { }

        public Trade(int passedTradeId, int tradingid,DateTime passedDatePublished, string passedTraderId)
        {
            tradeId = passedTradeId;
            tradeObjectId = tradingid;                                 
            tradeDatePublished = passedDatePublished;          
            traderId = passedTraderId;
        }
     
    }

    //[Serializable]
    public class TradeDTO
    {
        public int tradeId { get; set; }
      
        public DateTime tradeDatePublished { get; set; }


        public int tradeObjectId { get; set; }

        public TradeObject TradeObject { get; set; }

        public string tradeObjectName { get; set; }

        public int tradeCategoryId { get; set; }

        public string tradeCategoryType { get; set; }


        public string traderId { get; set; }


       //public ApplicationUserListDTO Trader { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }


        //public List<TradeForObjectDTO> tradeForObjects { get; set; }

        //public List<ImageDTO> Images { get; set; }
    }

    //[Serializable]
    public class TradeDetailDTO
    {
        public int tradeId { get; set; }             

        public DateTime tradeDatePublished { get; set; }


        public int tradeObjectId { get; set; }

        public TradeObject TradeObject { get; set; }

        public string tradeObjectName { get; set; }

        public int tradeCategoryId { get; set; }

        public string tradeCategoryType { get; set; }



        public string traderId { get; set; }

        //public ApplicationUserDetailDTO Trader { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }



        //public List<TradeForObjectDTO> tradeForObjects { get; set; }

        // TODO to see if we can pass the images to the carousel control
        // so we have single calll to get trades and images

        //public List<ImageDTO> Images { get; set; }

    }
}
