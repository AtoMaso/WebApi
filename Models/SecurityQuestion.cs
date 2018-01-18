using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class SecurityQuestion
    {
        [Key]
        public int questionId { get; set; }

        [Required]
        public string questionText { get; set; }

        public SecurityQuestion() { }

        public SecurityQuestion(int queId, string quesTxt)
        {
            questionId = queId;
            questionText = quesTxt;
        }
    }
}