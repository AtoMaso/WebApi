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

        [Required, MaxLength(10)]
        public string tradeObjectDescription { get; set; }

        public int objectCategoryId { get; set; }

        public ObjectCategory ObjectCategory { get; set; }

        public int tradeId { get; set; }

        public Trade Trade { get; set; }



        public TradeObject() { }

        public TradeObject(int tdObjId, string trObjDesc, int objCatId, int traId)
        {
            tradeObjectId = tdObjId;
            tradeObjectDescription = trObjDesc;
            objectCategoryId = objCatId;
            tradeId = traId;
        }
    }



    public class TradeObjectDTO
    {
        public int tradeObjectId { get; set; }

        public string tradeObjectDescription { get; set; }

        public int objectCategoryId { get; set; }

        public string tradeObjectCategoryDescription { get; set; } // will be categoryDescription from the ObjectTypes table or categoryDescription from ObjectCategories table

        public int tradeId { get; set; }


        public TradeObjectDTO() { }

        public TradeObjectDTO(int tdObjId, string trObjDesc,  int objCatId, string trObjCatDesc, int traId)
        {
            tradeObjectId = tdObjId;
            tradeObjectDescription = trObjDesc;
            objectCategoryId = objCatId;
            tradeObjectCategoryDescription = trObjCatDesc;
            tradeId = traId;

 
        }
    }
}