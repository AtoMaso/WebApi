using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class EmailType
    {
        [Key]
        public int emailTypeId { get; set; }

        [Required, MaxLength(30)]
        public string emailType { get; set; }

        public EmailType() { }

        public EmailType(int emTyId, string emTyDesc) {

            emailTypeId = emTyId;
            emailType = emTyDesc;
        }
    }
}