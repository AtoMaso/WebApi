using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApi.Models
{
    public class SecurityDetails
    {
        [Key]
        public int securityDetailsId { get; set; }
       
        [Required]
        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public List<SecurityAnswer> securityAnswers { get; set; }

        public SecurityDetails() { }

        public SecurityDetails(int id, string traid)
        {
            securityDetailsId = id;         
            traderId = traid;
        }

    }



    public class SecurityDetailsDTO
    {
        public int securityDetailsId { get; set; }

        public string traderId { get; set; }

        public string password { get; set; }

        public string confirmPassword { get; set; }

        public string userName { get; set; }

        public string email { get; set; }

        public List<SecurityAnswerDTO> securityAnswers { get; set; }

        public SecurityDetailsDTO() { }

    }
}