using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    // [Serializable]
    public class Subcategory
    {
        // primary key    
        [Key]
        public int subcategoryId { get; set; }

        [Required, MaxLength(30)]
        public string subcategory { get; set; }

        [Required]
        public int categoryId{ get; set; }

        public Category Category { get; set; }
       
        public Subcategory() { }

        public Subcategory(int id, string catname, int catid)
        {
            subcategoryId = id;
            subcategory = catname;
            categoryId = catid;
        }
    }

    public class SubcategoryDTO
    {
        // primary key         
        public int subcategoryId { get; set; }

        [Required, MaxLength(30)]
        public string subcategory { get; set; }
      
        public int categoryId { get; set; }

        public string category { get; set; }

        public Category Category { get; set; }

        public SubcategoryDTO() { }

    
    }

}