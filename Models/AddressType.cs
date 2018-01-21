using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class AddressType
    {
        [Key]
        public int addressTypeId { get; set; }

        [Required, MaxLength(30)]
        public string addressTypeDescription { get; set; }

        public AddressType() { }

        public AddressType(int id, string desc)
        {
            addressTypeId = id;
            addressTypeDescription = desc;
        }
    }
}