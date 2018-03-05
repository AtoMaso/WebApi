using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApi.Models
{
    public class PhoneType
    {
        [Key]
        public int phoneTypeId { get; set;}

        [Required, MaxLength(30)]
        public string phoneType { get; set; }

        public PhoneType() { }

        public PhoneType(int id, string desc)
        {
            phoneTypeId = id;
            phoneType = desc;
        }
    }
}