using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
   // [Serializable]
    public class Category
    {
        // primary key    
        [Key]   
        public int categoryId { get; set; }    

        [Required, MaxLength(30)]
        public string categoryDescription { get; set; }     

        public Category() { }

        public Category(int id, string catname )
        {
            categoryId = id;
            categoryDescription = catname;
        }       
    }

    public class CategoryDTO
    {
        // primary key    
        [Key]
        public int categoryId { get; set; }

        [Required, MaxLength(30)]
        public string categoryDescription { get; set; }

        public List<Subcategory> subcategories { get; set; }

        public CategoryDTO() { }

        public CategoryDTO(int id, string catname)
        {
            categoryId = id;
            categoryDescription = catname;
        }
    }

  }
