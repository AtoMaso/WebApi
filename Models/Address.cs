﻿using System;
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

        [Required, MaxLength(8)]
        public string number { get; set; }

        [Required, MaxLength(30)]
        public string street { get; set; }

        [Required, MaxLength(30)]
        public string suburb { get; set; }

        [Required, MaxLength(20)]
        public string city { get; set; }

        [Required, MaxLength(10)]
        public string postcode { get; set; }

        [Required, MaxLength(20)]
        public string state { get; set; }

        [Required, MaxLength(30)]
        public string country { get; set; }

        [Required, MaxLength(10)]
        public string preferred { get; set; }

        public int typeId { get; set; }

        public AddressType AddressType { get; set; }

        public int personalDetailsId { get; set; }

        public PersonalDetails PersonalDetails { get; set; }

        public Address() { }

        public Address(int pid, string passedNumber, string passedStreet, string passedSuburb,
                                        string passedCity, string passedPostcode, string passedState, 
                                        string passedCountry, int typId, int pdid, string pref)
        {
            this.id = pid;
            this.number = passedNumber;
            this.street = passedStreet;
            this.suburb = passedSuburb;
            this.city = passedCity;
            this.postcode = passedPostcode;
            this.state = passedState;
            this.country = passedCountry;
            this.typeId = typId;
            this.personalDetailsId = pdid;
            this.preferred = pref;
        }
    }



    public class AddressDTO
    {

        public int id { get; set; }


        public string number { get; set; }


        public string street { get; set; }


        public string suburb { get; set; }


        public string city { get; set; }


        public string postcode { get; set; }


        public string state { get; set; }


        public string country { get; set; }

        public string preferred { get; set; }

        public int typeId { get; set; }


        public string typeDescription { get; set; }

        [Required]
        public int personalDetailsId { get; set; }

        public AddressDTO() { }

        public AddressDTO(int pid, string passedNumber, string passedStreet, string passedSuburb,
                                        string passedCity, string passedPostcode, string passedState,
                                        string passedCountry, int typId, string addtId, int pdid, string pref)
        {
            this.id = pid;
            this.number = passedNumber;
            this.street = passedStreet;
            this.suburb = passedSuburb;
            this.city = passedCity;
            this.postcode = passedPostcode;
            this.state = passedState;
            this.country = passedCountry;
            this.preferred = pref;
            this.typeId = typId;
            this.typeDescription = addtId;
            this.personalDetailsId = pdid;
           
        } 

    }
}