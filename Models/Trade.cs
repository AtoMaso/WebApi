using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    //[Serializable]
    public class Trade
    {
        [Key]
        public int tradeId { get; set; }

        [Required, MaxLength(80)]
        public string title { get; set; }

        [Required]
        public DateTime datePublished { get; set; }        
               
        [Required]
        public int categoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }     
          
        [Required]     
        public string traderId { get; set; }

        // Navigation property  
        public ApplicationUser Trader { get; set; }     
          
        public List<Image> Images { get; set; }

        public Trade() { }

        public Trade(int passedTradeId, string passedTitle,DateTime passedDatePublished, int passedCategoryId, string passedTraderId)
        {
            tradeId = passedTradeId;
            title = passedTitle;                                 
            datePublished = passedDatePublished;
            categoryId = passedCategoryId;            
            traderId = passedTraderId;
        }
     
    }

    //[Serializable]
    public class TradeDTO
    {
        public int tradeId { get; set; }
        public string title { get; set; }         
        public DateTime datePublished { get; set; }
        public string categoryType { get; set; }        
        public string traderId { get; set; }
        public string traderFirstName { get; set; }
        public string traderSecondName { get; set; }
        public List<Image> Images { get; set; }
    }

    //[Serializable]
    public class TradeDetailDTO
    {
        public int tradeId { get; set; }       
        public string title { get; set; }      
        public DateTime datePublished { get; set; }  
        public string categoryType { get; set; }               
        public string traderId { get; set; }
        public string traderFirstName { get; set; }
        public string traderSecondName { get; set; }
        public List<Image> Images { get; set; }

    }
}
