using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class PersonalDetails
    {
        [Key]
        public int personalDetailsId { get; set; }

        [Required, MaxLength(20)]
        public string firstName { get; set; }

        [MaxLength(20)]
        public string middleName { get; set; }

        [Required, MaxLength(30)]
        public string lastName { get; set; }

        [Required]
        public DateTime dateOfBirth { get; set; }

      
        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public IQueryable<Address> Addresses { get; set; }

        public PersonalDetails() { }


        public PersonalDetails(int id, string first, string middle, string last, DateTime birth, string traid)
        {
            personalDetailsId = id;
            firstName = first;
            middleName = middle;
            lastName = last;
            dateOfBirth = birth;
            traderId = traid;
        }

    }
}