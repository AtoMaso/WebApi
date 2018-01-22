using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ProcessMessage
    {
        [Key]
        public int messageId { get; set; }

        [Required, MaxLength(10)]
        public string messageCode { get; set; }

        [Required, MaxLength(30)]
        public string messageType { get; set; }

        [Required, MaxLength(150)]
        public string messageText { get; set; }

        public ProcessMessage() { }

        public ProcessMessage(int id, string msc, string type, string text)
        {
            messageId = id;
            messageCode = msc;
            messageType = type;
            messageText = text;
        }

    }
}