using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.IO;

namespace WebApi.Models
{
    public class SocialNetworkType
    {
        [Key]
        public int typeId { get; set; }

        [Required, MaxLength(30)]
        public string typeDescription { get; set; }

        public SocialNetworkType() { }

        public SocialNetworkType(int tyId, string desc)
        {
            typeId = tyId;
            typeDescription = desc;
        }
    }
}