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

        [Required]
        public int questionId { get; set; }

        [Required, MaxLength(20)]
        public string questionAnswer { get; set; }
       
        public int securityDetailsId { get; set; }

        public SecurityDetails SecurityDetails { get; set; }

        public SecurityAnswer() { }

        public SecurityAnswer( int ansId, int quesId, string quesAns, int secId)
        {
            answerId = ansId;
            questionId = quesId;
            questionAnswer = quesAns;
            securityDetailsId = secId;
        }
    }


    public class SecurityAnswerDTO
    {
        public int answerId { get; set; }

        public int questionId { get; set; }

        public string questionText { get; set; }

        public string questionAnswer { get; set; }

        public int securityDetailsId { get; set; }

        public SecurityAnswerDTO() { }

        public SecurityAnswerDTO(int ansId, int queId, string quesTxt, string quesAns, int secId)
        {
            answerId = ansId;
            questionId = queId;
            questionText = quesTxt;
            questionAnswer = quesAns;
            securityDetailsId = secId;
        }

    }
}