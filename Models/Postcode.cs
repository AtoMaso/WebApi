using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Postcode
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(25)]
        public string number { get; set; }

        [Required]
        public int placeId { get; set; }

        public Place Place { get; set; }


        public List<Suburb> suburbs { get; set; }

        public Postcode() { }

        public Postcode(int pcid, string pcnum, int plid)
        {
            id = plid;
            number = pcnum;
            placeId = plid;
        }
    }


    public class PostcodeDTO
    {
        public int id { get; set; }
      
        public string number { get; set; }
       
        public int placeId { get; set; }
      
        public string placename {get; set;}

    }
}