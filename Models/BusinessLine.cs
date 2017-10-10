using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    //[Serializable]
    public class BusinessLine
    {
        // primary key        
        [Key]
        public int BusinessLineId { get; set; }     
        [Required]
        public string BusinessLineName { get; set; }

        public BusinessLine() { }

        public BusinessLine(string name)
        {       
            BusinessLineName = name;
        }
    }
}
