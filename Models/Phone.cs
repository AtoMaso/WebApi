using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Phone
    {

        [Key]
        public int id { get; set; }

        public int typeId { get; set; }

        public PhoneType PhoneType { get; set; }

        [Required, MaxLength(10)]
        public string number { get; set; }

        [Required, MaxLength(10)]
        public string countryCode { get; set; }

        [Required, MaxLength(10)]
        public string cityCode { get; set; }

        [Required, MaxLength(10)]
        public string preferred { get; set; }

        public int contactDetailsId { get; set; }

        //navigation property
        public ContactDetails ContactDetails { get; set; }

        public Phone() { }

        public Phone(int pid, int typId, string num, string countrycod, string citycod, int cdid, string pref)
        {
            id = pid;
            typeId = typId;
            number = num;
            countryCode = countrycod;
            cityCode = citycod;
            contactDetailsId = cdid;
            preferred = pref;
        }
    }


    public class PhoneDTO
    {
        public int id { get; set; }

        public string number { get; set; }      

        public string cityCode { get; set; }    

        public string countryCode { get; set; }

        public int typeId { get; set; }

        public string preferred { get; set; }

        public string typeDescription { get; set; }

        public int contactDetailsId { get; set; }

        public PhoneDTO() { }

        public PhoneDTO(int pid, int typId, string phonetd, string num, string countrycod, string citycod, int cdid, string pref)
        {
            id = pid;
            typeId = typId;
            typeDescription = phonetd;
            number = num;
            countryCode = countrycod;
            cityCode = citycod;
            contactDetailsId = cdid;
            preferred = pref;
        }
    }
}