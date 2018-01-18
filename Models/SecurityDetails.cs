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
       
        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public IQueryable<SecurityAnswer> Questions { get; set; }

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

        public IQueryable<SecurityAnswer> questions { get; set; }

        public SecurityDetailsDTO() { }

        public SecurityDetailsDTO(int id, string traid, IQueryable<SecurityAnswer> ques)
        {
            securityDetailsId = id;          
            traderId = traid;
            questions = ques;
        }

    }
}