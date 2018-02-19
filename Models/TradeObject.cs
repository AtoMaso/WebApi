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
        public int id { get; set; }

        [Required, MaxLength(10)]
        public string name { get; set; }

        [Required, MaxLength(200)]
        public string description { get; set; }

        [Required]
        public int categoryId { get; set; }

        public Category Category { get; set; }


        [Required]
        public int tradeId { get; set; }

        public virtual Trade Trade { get; set; }



        public TradeObject() { }

        public TradeObject(int tdObjId, string trObjName, int objCatId, int traId, string des)
        {
            id = tdObjId;
            name = trObjName;
            categoryId = objCatId;
            tradeId = traId;
            description = des;
        }
    }



    public class TradeObjectDTO
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public int categoryId { get; set; }

        public string categoryDescription { get; set; } // will be categoryDescription from the ObjectTypes table or categoryDescription from ObjectCategories table

        public int tradeId { get; set; }


        public TradeObjectDTO() { }

        public TradeObjectDTO(int tdObjId, string trObjName,  int objCatId, string trObjCatDesc, int traId, string desc)
        {
            id = tdObjId;
            name = trObjName;
            categoryId = objCatId;
            categoryDescription = trObjCatDesc;
            tradeId = traId;
            description = desc;
 
        }
    }
}