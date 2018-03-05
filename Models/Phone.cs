using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Phone
    {

        [Key]
        public int id { get; set; }

        [Required, MaxLength(10)]
        public string number { get; set; }

        [Required, MaxLength(10)]
        public string countryCode { get; set; }

        [Required, MaxLength(10)]
        public string cityCode { get; set; }

        [Required, MaxLength(10)]
        public string preferredFlag { get; set; }


        public int phoneTypeId { get; set; }

        public PhoneType PhoneType { get; set; }

        public string traderId { get; set; }

        public ApplicationUser Trader { get; set; } 

        public Phone() { }

        public Phone(int pid, int typId, string num, string countrycod, string citycod, string trid, string pref)
        {
            id = pid;
            phoneTypeId = typId;
            number = num;
            countryCode = countrycod;
            cityCode = citycod;
            traderId =trid;
            preferredFlag = pref;
        }
    }


    public class PhoneDTO
    {
        public int id { get; set; }

        public string number { get; set; }      

        public string cityCode { get; set; }    

        public string countryCode { get; set; }

        public int phoneTypeId { get; set; }

        public string preferredFlag { get; set; }

        public string phoneType { get; set; }
      
        public string traderId { get; set; }
  

    }
}