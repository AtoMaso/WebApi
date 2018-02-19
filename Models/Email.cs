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

        [Required, MaxLength(10)]
        public string preferred { get; set; }

        public int typeId { get; set; }

        public EmailType EmailType { get; set; }

        public int contactDetailsId { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public Email() { }

        public Email(int emId, int emTypeId, string emAccount, int contId, string pref)
        {
            id = emId;
            typeId = emTypeId;
            account = emAccount;
            contactDetailsId = contId;
            preferred = pref;
        }
    }


    public class EmailDTO
    {

        public int id { get; set; }

        public string account { get; set; }

        public int typeId { get; set; }

        public string typeDescription { get; set; }     

        public string preferred { get; set; }

        public int contactDetailsId { get; set; }

        public EmailDTO() { }

        public EmailDTO(int emId, int emTyId,  string emType, string emAccount, int contId, string pref)
        {
            id = emId;
            typeId = emTyId;
            typeDescription = emType;
            account = emAccount;
            contactDetailsId = contId;
            preferred = pref;
        }
    }
}