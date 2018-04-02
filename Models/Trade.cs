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

        [Required, MaxLength(20)]
        public string status { get; set; } // Open, Closed, Not Published

        [Required, Index]
        public int stateId { get; set; }

        public State State { get; set; }

        [Required, Index]
        public int placeId { get; set; }

        //public Place Place { get;  set;}

        [Required, Index]
        public int postcodeId { get; set; }

        [Required, Index]
        public int suburbId { get; set; }

        //public Postcode Postcode { get; set; }

        [Required, Index]
        public int categoryId { get; set; }

        public Category Category { get; set; }

        [Required, Index]
        public int subcategoryId { get; set; }

        //public Subcategory Subcategory { get; set; }

        [Required, Index]
        public string traderId { get; set; }        
        public ApplicationUser Trader { get; set; }

        public List<Image> Images { get; set; }


        public Trade() { }

        public Trade(int passedTradeId, DateTime passedDatePublished, string stat, string passedTraderId, string nam, string desc, string trfor, int catId, int subcatid, int staId, int plaId, int postcodId,int subId)
        {
            tradeId = passedTradeId;                            
            datePublished = passedDatePublished;
            status = stat;       
            traderId = passedTraderId;
            name = nam;
            description = desc;
            tradeFor = trfor;
            categoryId = catId;
            subcategoryId = subcatid;
            stateId = staId;
            placeId = plaId;
            postcodeId = postcodId;
            suburbId = subId;
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

        [Required, MaxLength(20)]
        public string status { get; set; } // Open, Closed

        [Required]
        public int stateId { get; set; }
     
        public  string state { get; set; }

        [Required]
        public int placeId { get; set; }

        public string place {get; set; }

        [Required]
        public int postcodeId { get; set; }

        public string postcodeNumber { get; set; }

        [Required]
        public int suburbId { get; set; }

        public string suburbName { get; set; }

        [Required]
        public int categoryId { get; set; }

        public string categoryDescription { get; set; }

        [Required]
        public int subcategoryId { get; set; }

        public string subcategoryDescription { get; set; }

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

        [Required, MaxLength(20)]
        public string status { get; set; } // Open, Closed

        [Required]
        public int stateId { get; set; }

        [Required, MaxLength(20)]
        public string state { get; set; }

        [Required]
        public int placeId { get; set; }

        public string place { get; set; }

        [Required]
        public int postcodeId { get; set; }

        public string postcodeNumber { get; set; }

        [Required]
        public int suburbId { get; set; }

        public string suburbName { get; set; }

        [Required]
        public int categoryId { get; set; }

        [Required, MaxLength(30) ]
        public string categoryDescription { get; set; }

        [Required]
        public int subcategoryId { get; set; }
             
        public string subcategoryDescription { get; set; }

        [Required]
        public string traderId { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }

        public List<ImageDTO> Images { get; set; }
    }
}
