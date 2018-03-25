using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Place
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(25)]
        public string name { get; set; }
        [Required]
        public int stateId { get; set; }

        public State State { get; set; }

         public List<Postcode> postcodes { get; set; }

        public Place() { }

        public Place(int plid, string plname, int stid)
        {
            id = plid;
            name = plname;
            stateId = stid;
        }
    }


    public class PlaceDTO
    {   
        public int id { get; set; }
      
        public string name { get; set; }
     
        public int stateId { get; set; }
    
       public string stateName { get; set; }

        public List<Postcode> postcodes { get; set; }
    }
}