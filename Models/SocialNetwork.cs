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
    public class SocialNetwork
    {        
        [Key]
        public int socialNetworkId { get; set; }

        public string type { get; set; }

        public string account { get; set; }

        [ForeignKey("ContactDetails")]
        public int contactDetailsId { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public SocialNetwork() { }

        public SocialNetwork(int id, string typ, string acc, int cdId) {
                    socialNetworkId = id;
                    type = typ;
                    account = acc;
                    contactDetailsId = cdId;
            }

    }
}