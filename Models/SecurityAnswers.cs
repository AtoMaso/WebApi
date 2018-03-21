using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class SecurityAnswer
    {

        [Key]
        public int answerId { get; set; }

        [Required, MaxLength(20)]
        public string questionAnswer { get; set; }

        [Required]
        public int questionId { get; set; }
       
        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public SecurityAnswer() { }

        public SecurityAnswer( int ansId, int quesId, string quesAns, string trId)
        {
            answerId = ansId;
            questionId = quesId;
            questionAnswer = quesAns;
            traderId = trId;
        }
    }


    public class SecurityAnswerDTO
    {
        [Required]
        public int answerId { get; set; }

        [Required]
        public string questionAnswer { get; set; }

        [Required]
        public int questionId { get; set; }

        [Required]
        public string questionText { get; set; }

        [Required]
        public string traderId { get; set; }
    }
}