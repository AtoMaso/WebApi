using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class Email
    {
        [Key]
        public int emailId { get; set; }

    
        [Required, MaxLength(70)]
        public string emailAccount { get; set; }

        // foreign key
        [Required]
        public int emailTypeId { get; set; }

        public EmailType EmailType { get; set; }

        // foreign key
        [Required]
        public int contactDetailsId { get; set; }

        //navigation property
        public ContactDetails ContactDetails { get; set; }

        public Email() { }

        public Email(int emId, int emTypeId, string emAccount, int contId)
        {
            emailId = emId;
            emailTypeId = emTypeId;
            emailAccount = emAccount;
            contactDetailsId = contId;
        }
    }


    public class EmailDTO
    {

        public int emailId { get; set; }

        public int emailTypeId { get; set; }

        public string emailType { get; set; }

        public string emailAccount { get; set; }

        public int contactDetailsId { get; set; }

        public EmailDTO() { }

        public EmailDTO(int emId, int emTyId,  string emType, string emAccount, int contId)
        {
            emailId = emId;
            emailTypeId = emTyId;
            emailType = emType;
            emailAccount = emAccount;
            contactDetailsId = contId;
        }
    }
}