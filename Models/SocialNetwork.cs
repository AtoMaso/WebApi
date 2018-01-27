using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class SocialNetwork
    {        
        [Key]
        public int socialNetworkId { get; set; }

        [Required, MaxLength(70)]
        public string socialNetworkAccount { get; set; }

        [Required]
        public int socialNetworkTypeId { get; set; }

        public SocialNetworkType SocialNetworkType { get; set; }

        [Required]
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

        [Required, MaxLength(70)]
        public string socialNetworkAccount { get; set; }

        [Required]
        public int socialNetworkTypeId { get; set; }

        [Required, MaxLength(30)]
        public string socialNetworkTypeDescription { get; set; }

        [Required]
        public int contactDetailsId { get; set; }

        public SocialNetworkDTO() { }

        public SocialNetworkDTO(int snid, string sntypetext, string snacc, int cdId)
        {
            socialNetworkId = snid;
            socialNetworkAccount = snacc;
            socialNetworkTypeDescription = sntypetext;         
            contactDetailsId = cdId;
        }
    }
}