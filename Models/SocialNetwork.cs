using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class SocialNetwork
    {        
        [Key]
        public int socialNetworkId { get; set; }

        public string socialNetworkAccount { get; set; }

        public int socialNetworkTypeId { get; set; }

        public SocialNetworkType SocialNetworkType { get; set; }
      
        public int contactDetailsId { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public SocialNetwork() { }

        public SocialNetwork(int id, string account, int typid, int cdId) {
            socialNetworkId = id;
            socialNetworkAccount = account;
            socialNetworkTypeId = typid;          
            contactDetailsId = cdId;
         }

    }



    public class SocialNetworkDTO
    {    
        public int socialNetworkId { get; set; }
     
        public string socialNetworkAccount { get; set; }

        public int socialNetworkTypeId { get; set; }

        public string socialNetworkTypeText { get; set; }

        public int contactDetailsId { get; set; }

        public SocialNetworkDTO() { }

        public SocialNetworkDTO(int snid, string sntypetext, string snacc, int cdId)
        {
            socialNetworkId = snid;
            socialNetworkAccount = snacc;
            socialNetworkTypeText = sntypetext;         
            contactDetailsId = cdId;
        }
    }
}