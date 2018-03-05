using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.IO;

namespace WebApi.Models
{
    public class SocialNetworkType
    {
        [Key]
        public int socialTypeId { get; set; }

        [Required, MaxLength(30)]
        public string socialType { get; set; }

        public SocialNetworkType() { }

        public SocialNetworkType(int tyId, string desc)
        {
            socialTypeId = tyId;
            socialType = desc;
        }
    }
}