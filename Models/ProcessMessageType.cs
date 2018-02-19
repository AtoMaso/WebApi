using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models
{
    public class ProcessMessageType
    {
        [Key]
        public int messageTypeId { get; set; }

        [Required, MaxLength(30)]
        public string messageTypeDescription { get; set; }


        public ProcessMessageType() { }

        public ProcessMessageType(int pmTyId, string pmTyDesc)
        {
            messageTypeId = pmTyId;
            messageTypeDescription = pmTyDesc;
        }
    }
}