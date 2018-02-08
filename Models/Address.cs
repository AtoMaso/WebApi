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
        public int addressId { get; set; }

        [Required, MaxLength(8)]
        public string addressNumber { get; set; }

        [Required, MaxLength(30)]
        public string addressStreet { get; set; }

        [Required, MaxLength(30)]
        public string addressSuburb { get; set; }

        [Required, MaxLength(20)]
        public string addressCity { get; set; }

        [Required, MaxLength(10)]
        public string addressPostcode { get; set; }

        [Required, MaxLength(20)]
        public string addressState { get; set; }

        [Required, MaxLength(30)]
        public string addressCountry { get; set; }

        [Required, MaxLength(10)]
        public string addressPreferredFlag { get; set; }

        public int addressTypeId { get; set; }

        public AddressType AddressType { get; set; }

        public int personalDetailsId { get; set; }

        public PersonalDetails PersonalDetails { get; set; }

        public Address() { }

        public Address(int id, string passedNumber, string passedStreet, string passedSuburb,
                                        string passedCity, string passedPostcode, string passedState, 
                                        string passedCountry, int typId, int pdid, string pref)
        {
            this.addressId = id;
            this.addressNumber = passedNumber;
            this.addressStreet = passedStreet;
            this.addressSuburb = passedSuburb;
            this.addressCity = passedCity;
            this.addressPostcode = passedPostcode;
            this.addressState = passedState;
            this.addressCountry = passedCountry;
            this.addressTypeId = typId;
            this.personalDetailsId = pdid;
            this.addressPreferredFlag = pref;
        }
    }



    public class AddressDTO
    {

        public int addressId { get; set; }


        public string addressNumber { get; set; }


        public string addressStreet { get; set; }


        public string addressSuburb { get; set; }


        public string addressCity { get; set; }


        public string addressPostcode { get; set; }


        public string addressState { get; set; }


        public string addressCountry { get; set; }

        public string addressPreferredFlag { get; set; }

        public int addressTypeId { get; set; }


        public string addressTypeDescription { get; set; }

        [Required]
        public int personalDetailsId { get; set; }

        public AddressDTO() { }

        public AddressDTO(int id, string passedNumber, string passedStreet, string passedSuburb,
                                        string passedCity, string passedPostcode, string passedState,
                                        string passedCountry, int typId, string addtId, int pdid, string pref)
        {
            this.addressId = id;
            this.addressNumber = passedNumber;
            this.addressStreet = passedStreet;
            this.addressSuburb = passedSuburb;
            this.addressCity = passedCity;
            this.addressPostcode = passedPostcode;
            this.addressState = passedState;
            this.addressCountry = passedCountry;
            this.addressTypeId = typId;
            this.addressTypeDescription = addtId;
            this.personalDetailsId = pdid;
            this.addressPreferredFlag = pref;
        } 

    }
}