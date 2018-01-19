using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Phone
    {

        [Key]
        public int phoneId { get; set; }

        [Required, MaxLength(10)]
        public string phoneType { get; set; }

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

        public Phone(int id, string typ, string num, string countrycod, string citycod, int cdid)
        {
            phoneId = id;
            phoneType = typ;
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
        public string phoneType { get; set; }

        [Required, MaxLength(10)]
        public string phoneNumber { get; set; }

        [Required, MaxLength(10)]
        public string phoneCountryCode { get; set; }

        [Required, MaxLength(10)]
        public string phoneCityCode { get; set; }

        public int contactDetailsId { get; set; }

        public PhoneDTO() { }

        public PhoneDTO(int id, string typ, string num, string countrycod, string citycod, int cdid)
        {
            phoneId = id;
            phoneType = typ;
            phoneNumber = num;
            phoneCountryCode = countrycod;
            phoneCityCode = citycod;
            contactDetailsId = cdid;
        }
    }
}