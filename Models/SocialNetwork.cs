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
        public string preferred { get; set; }

        public int typeId { get; set; }

        public SocialNetworkType SocialNetworkType { get; set; }

        public int contactDetailsId { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public SocialNetwork() { }

        public SocialNetwork(int pid, string paccount, int typid, int cdId, string pref) {
            id = pid;
            account = paccount;
            typeId = typid;          
            contactDetailsId = cdId;
            preferred = pref;            
         }

    }



    public class SocialNetworkDTO
    {    
        public int id { get; set; }

        public string account { get; set; }
      
        public string preferred { get; set; }

        public int typeId { get; set; }

        public string typeDescription { get; set; }

        [Required]
        public int contactDetailsId { get; set; }

        public SocialNetworkDTO() { }

        public SocialNetworkDTO(int snid, string sntypetext, string snacc, int cdId, string pref)
        {
            id = snid;
            account = snacc;
            typeDescription = sntypetext;         
            contactDetailsId = cdId;
            preferred = pref;
        }
    }
}