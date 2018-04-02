using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApi.Models
{
    public class Address
    {

        [Key]
        public int id { get; set; }

        [MaxLength(8)]
        public string number { get; set; }

        [MaxLength(8)]
        public string unit { get; set; }

        [MaxLength(8)]
        public string pobox { get; set; }

        [Required, MaxLength(30)]
        public string street { get; set; }

        [Required, MaxLength(30)]
        public string suburb { get; set; }

        [Required, MaxLength(20)]
        public string place { get; set; }

        [Required, MaxLength(10)]
        public string postcode { get; set; }

        [Required, MaxLength(20)]
        public string state { get; set; }

        [MaxLength(30)]
        public string country { get; set; }

        [Required, MaxLength(10)]
        public string preferredFlag { get; set; }

        public int addressTypeId { get; set; }

        public AddressType AddressType { get; set; }

        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public Address() { }

        public Address(int pid, string passedNumber, string passedStreet,
                                        string passedSuburb, string passedCity,
                                        string passedPostcode, string passedState, 
                                        string passedCountry, int typId, string trid, 
                                        string pref, string uni, string box)
        {
            this.id = pid;
            this.number = passedNumber;
            this.street = passedStreet;
            this.suburb = passedSuburb;
            this.place = passedCity;
            this.postcode = passedPostcode;
            this.state = passedState;
            this.country = passedCountry;
            this.addressTypeId = typId;
            this.traderId = trid;
            this.preferredFlag = pref;
            this.unit = uni;
            this.pobox = box;
        }
    }



    public class AddressDTO
    {

        public int id { get; set; }
     
        public string number { get; set; }

        public string unit { get; set; }

        public string pobox { get; set; }

        [Required]
        public string street { get; set; }

        [Required]
        public string suburb { get; set; }

        [Required]
        public string place { get; set; }

        [Required]
        public string postcode { get; set; }

        [Required]
        public string state { get; set; }

        [Required]
        public string country { get; set; }

        [Required]
        public string preferredFlag { get; set; }

        [Required]
        public int addressTypeId { get; set; }

        public string addressType { get; set; }

        [Required]
        public string traderId { get; set; }

    }
}