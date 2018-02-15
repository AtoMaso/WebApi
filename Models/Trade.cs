using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models
{
    public class Trade
    {
        [Key]
        public int tradeId { get; set; }                    

        [Required, MaxLength(20)]
        public string name { get; set; }

        [Required, MaxLength(200)]
        public string description { get; set; }

        [Required, MaxLength(20)]
        public string tradeFor { get; set; }

        [Required]
        public DateTime datePublished { get; set; }

        [Required, MaxLength(10)]
        public string status { get; set; } // Open, Closed

        [Required]
        public int categoryId { get; set; }

        public Category Category { get; set; }
   
        [Required]
        public string traderId { get; set; }        
        public ApplicationUser Trader { get; set; }

        public List<Image> Images { get; set; }


        public Trade() { }

        public Trade(int passedTradeId, DateTime passedDatePublished, string stat, string passedTraderId, string nam, string desc, string trfor, int catId)
        {
            tradeId = passedTradeId;                            
            datePublished = passedDatePublished;
            status = stat;       
            traderId = passedTraderId;
            name = nam;
            description = desc;
            tradeFor = trfor;
            categoryId = catId;
        }     
    }



    public class TradeDTO
    {
        public int tradeId { get; set; }

        [Required, MaxLength(20)]
        public string name { get; set; }

        [Required, MaxLength(200)]
        public string description { get; set; }

        [Required, MaxLength(20)]
        public string tradeFor { get; set; }
   
        [Required]
        public DateTime datePublished { get; set; }
   
        [Required, MaxLength(10)]
        public string status { get; set; } // Open, Closed

        [Required]
        public int categoryId { get; set; }

        public string categoryDescription { get; set; }

        [Required]
        public string traderId { get; set; }
 
        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }

        public List<ImageDTO> Images { get; set; }

        public int total { get; set; }

    }


    public class TradeDetailDTO
    {

        public int tradeId { get; set; }

        [Required, MaxLength(20)]
        public string name { get; set; }

        [Required, MaxLength(200)]
        public string description { get; set; }

        [Required, MaxLength(20)]
        public string tradeFor { get; set; }

        [Required]
        public DateTime datePublished { get; set; }

        [Required, MaxLength(10)]
        public string status { get; set; } // Open, Closed

        [Required]
        public int categoryId { get; set; }

        public string categoryDescription { get; set; }

        [Required]
        public string traderId { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }

        public List<ImageDTO> Images { get; set; }
    }
}
