using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
   // [Serializable]
    public class ObjectCategory
    {
        // primary key    
        [Key]   
        public int objectCategoryId { get; set; }    

        [Required, MaxLength(30)]
        public string objectCategoryDescription { get; set; }

        public ObjectCategory() { }

        public ObjectCategory(int id, string catname )
        {
            objectCategoryId = id;
            objectCategoryDescription = catname;
        }
       
    }
}
