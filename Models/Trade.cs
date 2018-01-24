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
        public string traderId { get; set; }        
        public ApplicationUser Trader { get; set; }


        public List<TradeObject> tradeObjects { get; set; }

        public List<TradeForObject> tradeForObjects { get; set; }

        public IQueryable<Image> images { get; set; }


        public Trade() { }

        public Trade(int passedTradeId, DateTime passedDatePublished, string passedTraderId)
        {
            tradeId = passedTradeId;                            
            tradeDatePublished = passedDatePublished;          
            traderId = passedTraderId;
        }     
    }




    //[Serializable]
    public class TradeDTO
    {
        public int tradeId { get; set; }
      
        public DateTime tradeDatePublished { get; set; }


        public string traderId { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }



        public List<TradeObjectDTO> tradeObjects { get; set; }

        public List<TradeForObjectDTO> tradeForObjects { get; set; }

        public List<ImageDTO> images { get; set; }
    }




    //[Serializable]
    public class TradeDetailDTO
    {
        public int tradeId { get; set; }             

        public DateTime tradeDatePublished { get; set; }


        public string traderId { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }


        public List<TradeObjectDTO> tradeObjects { get; set; }

        public List<TradeForObjectDTO> tradeForObjects { get; set; }

        public List<ImageDTO> images { get; set; }  // TODO to see if we can pass the images to the carousel control
                                                                          // so we have single calll to get trades and images

    }
}
