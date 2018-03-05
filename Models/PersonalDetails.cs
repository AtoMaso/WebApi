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
        public int id { get; set; }

        [Required, MaxLength(20)]
        public string firstName { get; set; }

        [MaxLength(20)]
        public string middleName { get; set; }

        [Required, MaxLength(30)]
        public string lastName { get; set; }

        public DateTime dateOfBirth { get; set; }

        [Required]
        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public PersonalDetails() { }


        public PersonalDetails(int ids, string first, string middle, string last, DateTime birth, string traid)
        {
            id = ids;
            firstName = first;
            middleName = middle;
            lastName = last;
            dateOfBirth = birth;
            traderId = traid;
        }

    }



    public class PersonalDetailsDTO
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string firstName { get; set; }


        public string middleName { get; set; }

        [Required]
        public string lastName { get; set; }


        public DateTime dateOfBirth { get; set; }

        [Required]
        public string traderId { get; set; }


        public PersonalDetailsDTO() { }

    }

}