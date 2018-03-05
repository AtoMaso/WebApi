using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class Email
    {
        [Key]
        public int id { get; set; }
    
        [Required, MaxLength(70)]
        public string account { get; set; }

        [Required, MaxLength(5)]
        public string preferredFlag { get; set; }

        public int emailTypeId { get; set; }

        public EmailType EmailType { get; set; }

        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public Email() { }

        public Email(int emId, int emTypeId, string emAccount, string trid, string pref)
        {
            id = emId;
            emailTypeId = emTypeId;
            account = emAccount;
            traderId = trid;
            preferredFlag = pref;
        }
    }


    public class EmailDTO
    {

        public int id { get; set; }

        public string account { get; set; }

        public int emailTypeId { get; set; }

        public string emailType { get; set; }     

        public string preferredFlag { get; set; }

        public string traderId { get; set; }
    }
}