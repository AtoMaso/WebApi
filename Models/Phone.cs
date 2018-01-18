using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using System.IO;


namespace WebApi.Models
{
    public class Phone
    {
        [Key]
        public int phoneId { get; set; }

        [Required, MaxLength(10)]
        public string type { get; set; }

        [Required, MaxLength(12)]
        public string number { get; set; }

        [Required, MaxLength(6)]
        public string countryCode { get; set; }

        [Required, MaxLength(6)]
        public string cityCode { get; set; }

        [ForeignKey("ContactDetails")]
        public int contactDetailsId { get; set;  }

        public ContactDetails ContactDetails { get; set; }

        public Phone() { }

        public Phone(int id, string typ, string num, string countrycod, string citycod, int cdid ) {

            phoneId = id;
            type = typ;
            number = num;
            countryCode = countrycod;
            cityCode = cityCode;
            contactDetailsId = cdid;
        }
    }
}