using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models
{
    public class TradeObject
    {
        [Key]
        public int tradeObjectId { get; set; }

        [Required, MaxLength(20)]
        public string tradeObjectName { get; set; }

        [Required]
        public int categoryId { get; set; }

        public Category Category { get; set; } 

        public TradeObject() { }

        public TradeObject(int id, string name, int catid)
        {
            tradeObjectId = id;
            tradeObjectName = name;
            categoryId = catid;
        }
    }



    public class TradeObjectDTO
    {
        public int tradeObjectId { get; set; }

        public string tradeObjectName { get; set; }

        public int categoryId { get; set; }

        public string categoryType { get; set; }

        public TradeObjectDTO() { }

        public TradeObjectDTO(int id, string name, int catid)
        {
            tradeObjectId = id;
            tradeObjectName = name;
            categoryId = catid;
        }
    }
}