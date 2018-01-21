using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

namespace WebApi.Models
{
    //[Serializable]    
    public class Trade
    {
        [Key]
        public int tradeId { get; set; }

        [Required, MaxLength(80)]
        public string tradeTitle { get; set; }

        [Required]
        public DateTime tradeDatePublished { get; set; }        
               
        [ForeignKey("Category")]
        public int categoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }
      
        public string traderId { get; set; }
        
        public ApplicationUser Trader { get; set; }
          
        public IQueryable<Image> Images { get; set; }

        public Trade() { }

        public Trade(int passedTradeId, string passedTitle,DateTime passedDatePublished, int passedCategoryId, string passedTraderId)
        {
            tradeId = passedTradeId;
            tradeTitle = passedTitle;                                 
            tradeDatePublished = passedDatePublished;
            categoryId = passedCategoryId;            
            traderId = passedTraderId;
        }
     
    }

    //[Serializable]
    public class TradeDTO
    {
        public int tradeId { get; set; }
        public string tradeTitle { get; set; }         
        public DateTime tradeDatePublished { get; set; }
        public string tradeCategoryType { get; set; }        
        public string traderId { get; set; }
        public string traderFirstName { get; set; }
        public string traderLastName { get; set; }
        public List<ImageDTO> Images { get; set; }
    }

    //[Serializable]
    public class TradeDetailDTO
    {
        public int tradeId { get; set; }       
        public string tradeTitle { get; set; }      
        public DateTime tradeDatePublished { get; set; }  
        public string tradeCategoryType { get; set; }               
        public string traderId { get; set; }
        public string traderFirstName { get; set; }
        public string traderLastName { get; set; }
        public List<ImageDTO> Images { get; set; }

    }
}
