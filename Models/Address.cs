using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Address
    {

        [Key]
        public int addressId { get; set; }

        [Required, MaxLength(8)]
        public string number { get; set; }

        [Required, MaxLength(30)]
        public string street { get; set; }

        [Required, MaxLength(30)]
        public string suburb { get; set; }

        [Required, MaxLength(30)]
        public string city { get; set; }

        [Required, MaxLength(10)]
        public string postcode { get; set; }

        [Required, MaxLength(20)]
        public string state { get; set; }

        [Required, MaxLength(20)]
        public string country { get; set; }

        public Address() { }

        public Address(int id, string passedNumber, string passedStreet, string passedSuburb,
                              string passedCity, string passedPostcode, string passedState, string passedCountry)
        {
            this.addressId = id;
            this.number = passedNumber;
            this.street = passedStreet;
            this.suburb = passedSuburb;
            this.city = passedCity;
            this.postcode = passedPostcode;
            this.state = passedState;
            this.country = passedCountry;
        }
    }
}