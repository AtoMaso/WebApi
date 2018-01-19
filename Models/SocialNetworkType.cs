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
        public string socialNetworkTypeText { get; set; }

        public SocialNetworkType() { }

        public SocialNetworkType(int id, string text)
        {
            socialNetworkTypeId = id;
            socialNetworkTypeText = text;
        }
    }
}