using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Suburb
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(25)]
        public string name { get; set; }
       
        public int postcodeId { get; set; }

        public Postcode Postcode { get; set; }

        public Suburb() { }

        public Suburb(int pcid, string sname, int plid)
        {
            id = plid;
            name = sname;
            postcodeId = plid;
        }
    }


    public class SuburbDTO
    {
        public int id { get; set; }
     
        public string name { get; set; }
      
        public int postcodeId { get; set; }
      
        public string postcodeNumber { get; set; }

    }
}