using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    //[Serializable]
    public class Locality
    {       
        [Key]
        public int LocalityId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Suburb { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string State { get; set; }

        public Locality() { }

        public Locality( string number, string street, string suburb, string city, string postcode, string state)
        {            
            Number = number;
            Street = street;
            Suburb = suburb;
            City = city;
            Postcode = postcode;
            State = state;
        }
    }
}
