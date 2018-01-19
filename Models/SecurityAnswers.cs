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
        public string questionText { get; set; }

        [Required]
        public string questionAnswer { get; set; }

        public int securityDetailsId { get; set; }

        public SecurityDetails SecurityDetails { get; set; }

        public SecurityAnswer() { }

        public SecurityAnswer( int ansId, string quesTxt, string quesAns, int secId)
        {
            answerId = ansId;
            questionText = quesTxt;
            questionAnswer = quesAns;
            securityDetailsId = secId;
        }
    }


    public class SecurityAnswerDTO
    {

        public int answerId { get; set; }

        [Required]
        public string questionText { get; set; }

        [Required]
        public string questionAnswer { get; set; }

        public int securityDetailsId { get; set; }

        public SecurityAnswerDTO() { }

    }
}