using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ProcessMessage
    {
        [Key]
        public int messageId { get; set; }

        [Required, MaxLength(10)]
        public string messageCode { get; set; }
        
        [Required, MaxLength(150)]
        public string messageText { get; set; }
        
        [Required]
        public int messageTypeId { get; set; }

        public ProcessMessageType ProcessMessageType { get; set; }

        public ProcessMessage() { }

        public ProcessMessage(int meId, string meCode, int meTypeId, string meText)
        {
            messageId = meId;
            messageCode = meCode;
            messageTypeId = meTypeId;
            messageText = meText;
        }
    }


    public class ProcessMessageDTO
    {

        public int messageId { get; set; }

        public string messageCode { get; set; }

        public string messageText { get; set; }

        public int messageTypeId { get; set; }

        public string messageType { get; set; }


        public ProcessMessageDTO() { }

        public ProcessMessageDTO(int meId, string meCode, string meText, string meTypeDesc, int meTypeId)
        {
            messageId = meId;
            messageCode = meCode;
            messageText = meText;
            messageTypeId = meTypeId;
            messageType = meTypeDesc;

        }
    }
}