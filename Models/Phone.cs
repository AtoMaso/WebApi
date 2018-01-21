using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Phone
    {

        [Key]
        public int phoneId { get; set; }

        [Required]
        public int phoneTypeId { get; set; }

        [Required, MaxLength(10)]
        public string phoneNumber { get; set; }

        [Required, MaxLength(10)]
        public string phoneCountryCode { get; set; }

        [Required, MaxLength(10)]
        public string phoneCityCode { get; set; }
       
        public int contactDetailsId { get; set; }

        //navigation property
        public ContactDetails ContactDetails { get; set; }

        public Phone() { }

        public Phone(int id, int typId, string num, string countrycod, string citycod, int cdid)
        {
            phoneId = id;
            phoneTypeId = typId;
            phoneNumber = num;
            phoneCountryCode = countrycod;
            phoneCityCode = citycod;
            contactDetailsId = cdid;
        }
    }


    public class PhoneDTO
    {
        public int phoneId { get; set; }

        [Required, MaxLength(10)]
        public string phoneNumber { get; set; }      

        [Required, MaxLength(10)]
        public string phoneCityCode { get; set; }

        [Required, MaxLength(10)]
        public string phoneCountryCode { get; set; }

        [Required]
        public int phoneTypeId { get; set; }

        [Required, MaxLength(30)]
        public string phoneTypeDescription { get; set; }

        public int contactDetailsId { get; set; }

        public PhoneDTO() { }

        public PhoneDTO(int id, int typId, string phonetd, string num, string countrycod, string citycod, int cdid)
        {
            phoneId = id;
            phoneTypeId = typId;
            phoneTypeDescription = phonetd;
            phoneNumber = num;
            phoneCountryCode = countrycod;
            phoneCityCode = citycod;
            contactDetailsId = cdid;
        }
    }
}