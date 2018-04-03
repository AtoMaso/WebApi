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

        [Required, MaxLength(3), Index]
        public string state { get; set; }

        //[Required, Index]
        //public int stateId { get; set; }
        //public State State { get; set; }

        [Required,MaxLength(45), Index]
        public string place { get; set; }
        //[Required, Index]
        //public int placeId { get; set; }        
        //public Place Place { get;  set;}

        [Required, MaxLength(4), Index]
        public string postcode { get; set; }
        //public int postcodeId { get; set; }

        [Required,MaxLength(45), Index]
        public string suburb { get; set; }
        //public int suburbId { get; set; }
        //public Postcode Postcode { get; set; }

        [Required, MaxLength(45), Index]
        public string category { get; set; }
        //public Category Category { get; set; }

        [Required, MaxLength(45), Index]
        public string subcategory { get; set; }
        //public Subcategory Subcategory { get; set; }

        [Required, Index]
        public string traderId { get; set; }        
        public ApplicationUser Trader { get; set; }

        public List<Image> Images { get; set; }


        public Trade() { }

    }



    public class TradeDTO
    {
        public int tradeId { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string tradeFor { get; set; }

        public DateTime datePublished { get; set; }

        public string status { get; set; } // Open, Closed

        public string state { get; set; }
        //public int stateId { get; set; }
   
        public string place { get; set; }
        //public int placeId { get; set; }

        public string postcode { get; set; }
        //public int postcodeId { get; set; }

        public string suburb { get; set; }
        //public int suburbId { get; set; }

        public string category { get; set; }

        public string subcategory { get; set; }

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
  
        public string name { get; set; }
    
        public string description { get; set; }
       
        public string tradeFor { get; set; }
     
        public DateTime datePublished { get; set; }

        public string status { get; set; } // Open, Closed
   
        //public int stateId { get; set; }
        public string state { get; set; }

        public string place { get; set; }
        //public int placeId { get; set; }

        //public int postcodeId { get; set; }
        public string postcode { get; set; }

        //public int suburbId { get; set; }

        public string suburb { get; set; }

        public int category { get; set; }

        public string subcategory { get; set; }
            
        public string traderId { get; set; }

        public string traderFirstName { get; set; }

        public string traderMiddleName { get; set; }

        public string traderLastName { get; set; }

        public List<ImageDTO> Images { get; set; }
    }
}
