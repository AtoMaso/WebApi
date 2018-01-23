using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models
{
    public class TradeForObject
    {
        [Key]
        public int tradeForObjectId { get; set; }

        [Required, MaxLength(20)]
        public string tradeForObjectName { get; set; }


        [Required]
        public int tradeId { get; set; }


        [Required]
        public int categoryId { get; set; }

        public Category Category { get; set; }


        public TradeForObject() { }

        public TradeForObject(int id, string name, int catid)
        {
            tradeForObjectId = id;
            tradeForObjectName = name;
            categoryId = catid;
        }
    }


    public class TradeForObjectDTO
    {
        public int tradeForObjectId { get; set; }
      
        public string tradeForObjectName { get; set; }


        public int tradeId { get; set; }


        public int categoryId { get; set; }

        public string categoryType { get; set; }

        public TradeForObjectDTO() { }

        public TradeForObjectDTO(int id, string name,int traid, int catid)
        {
            tradeForObjectId = id;
            tradeForObjectName = name;
            tradeId = traid;
            categoryId = catid;
        }
    }
}