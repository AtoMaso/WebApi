using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;


namespace WebApi.Models
{
    public class PersonalDetails
    {
        [Key]
        public int pdId { get; set; }

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

        public PersonalDetails() { }

        public PersonalDetails(int id, string first, string middle, string last, DateTime birth, string traid)
        {
            pdId = id;
            firstName = first;
            middleName = middle;
            lastName = last;
            dateOfBirth = birth;
            traderId = traid;
        }

    }
}