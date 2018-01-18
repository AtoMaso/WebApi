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
    public class ContactDetails
    {

        [Key]
        public int contactDetailsId { get; set; }       
  
        public string traderId { get; set; }       

        public ApplicationUser Trader { get; set; }

        public IQueryable<Phone> Phones { get; set; }

        public IQueryable<SocialNetwork> SocialNetworks { get; set; }

        public ContactDetails() { }


        public ContactDetails(int id, string traid)
        {
            contactDetailsId = id;         
            traderId = traid;
        }
    }


    public class ContactDetailsDTO
    {
        public int contactDetailsId { get; set; }

        public string traderId { get; set; }

        public IQueryable<Phone> Phones { get; set; }

        public IQueryable<SocialNetwork> SocialNetworks { get; set; }

        public ContactDetailsDTO() { }
  
    }
}