using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.IO;

namespace WebApi.Models
{
    public class SocialNetworkType
    {
        [Key]
        public int socialNetworkTypeId { get; set; }

        [Required, MaxLength(30)]
        public string socialNetworkTypeDescription { get; set; }

        public SocialNetworkType() { }

        public SocialNetworkType(int tyId, string desc)
        {
            socialNetworkTypeId = tyId;
            socialNetworkTypeDescription = desc;
        }
    }
}