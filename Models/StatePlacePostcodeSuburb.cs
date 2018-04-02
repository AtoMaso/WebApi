using System;
using System.ComponentModel.DataAnnotations;
namespace WebApi.Models
{
    public class StatePlacePostcodeSuburb
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
}