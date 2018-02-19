using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApi.Models
{
    public class PhoneType
    {
        [Key]
        public int typeId { get; set;}

        [Required, MaxLength(30)]
        public string typeDescription { get; set; }

        public PhoneType() { }

        public PhoneType(int id, string desc)
        {
            typeId = id;
            typeDescription = desc;
        }
    }
}