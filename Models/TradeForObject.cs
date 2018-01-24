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

        [Required, MaxLength(10)]
        public string tradeForObjectDescription { get; set; }

        public int objectCategoryId { get; set; }

        public ObjectCategory ObjectCategory { get; set; }

        public int tradeId { get; set; }

        public Trade Trade { get; set; }



        public TradeForObject() { }

        public TradeForObject(int tdObjId, string trObjDesc, int objCatId, int traId)
        {
            tradeForObjectId = tdObjId;
            tradeForObjectDescription = trObjDesc;
            objectCategoryId = objCatId;
            tradeId = traId;
        }
    }


    public class TradeForObjectDTO
    {
        public int tradeForObjectId { get; set; }

        public string tradeForObjectDescription { get; set; }

        public int objectCategoryId { get; set; }

        public string tradeForObjectCategoryDescription { get; set; } // will be categoryDescription from the ObjectTypes table or categoryDescription from ObjectCategories table

        public int tradeId { get; set; }


        public TradeForObjectDTO() { }

        public TradeForObjectDTO(int tdObjId, string trObjDesc, int objCatId, string trObjCatDesc, int traId)
        {
            tradeForObjectId = tdObjId;
            tradeForObjectDescription = trObjDesc;
            objectCategoryId = objCatId;
            tradeForObjectCategoryDescription = trObjCatDesc;
            tradeId = traId;


        }
    }
}