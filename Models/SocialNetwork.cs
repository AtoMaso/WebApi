using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class SocialNetwork
    {        
        [Key]
        public int id { get; set; }

        [Required, MaxLength(70)]
        public string account { get; set; }

        [Required, MaxLength(10)]
        public string preferredFlag { get; set; }

        public int socialTypeId { get; set; }

        public SocialNetworkType SocialNetworkType { get; set; }

        public string  traderId { get; set; }

        public ApplicationUser Trader { get; set; }

        public SocialNetwork() { }

        public SocialNetwork(int pid, string paccount, int typid, string trId, string pref) {
            id = pid;
            account = paccount;
            socialTypeId = typid;          
            traderId = trId;
            preferredFlag = pref;            
         }

    }



    public class SocialNetworkDTO
    {    
        public int id { get; set; }

        public string account { get; set; }
      
        public string preferredFlag { get; set; }

        public int socialTypeId { get; set; }

        public string socialType { get; set; }
      
        public string traderId { get; set; }      
    }
}