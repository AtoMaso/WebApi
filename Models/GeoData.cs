using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class GeoData
    {
        [Key]
        public int id { get; set; }
       
        [Required, MaxLength(3)]
        public string state { get; set; }

        [Required, MaxLength(45)]
        public string place { get; set; }

        [Required, MaxLength(4)]
        public string postcode { get; set; }

        [Required, MaxLength(45)]
        public string suburb { get; set; }
    }


    public class State
    {    
        public string state { get; set; }

       public List<Place> places { get; set; }   
    }


    public class Place
    {
        public string place { get; set; }

        public string parentstate { get; set; }
        public List<Postcode> postcodes { get; set; }
    }


    public class Postcode
    { 
        public string postcode { get; set; }

        public string parentplace { get; set; }

        public List<Suburb> suburbs { get; set; }
    }

    public class Suburb
    { 
        public string suburb { get; set; }

       public string parentpostcode { get; set; }
    }
}