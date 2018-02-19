using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class EmailType
    {
        [Key]
        public int typeId { get; set; }

        [Required, MaxLength(30)]
        public string typeDescription { get; set; }


        public EmailType() { }

        public EmailType(int emTyId, string emTyDesc) {

            typeId = emTyId;
            typeDescription = emTyDesc;
        }
    }
}