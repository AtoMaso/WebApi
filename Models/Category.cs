using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    //[Serializable]
    public class Category
    {
        // primary key    
        [Key]   
        public int CategoryId { get; set; }    
        [Required]
        public string CategoryName { get; set; }

        public Category() { }

        public Category(int catid, string catname )
        {        
            CategoryName = catname;
        }
       
    }
}
