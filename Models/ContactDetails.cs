using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class ContactDetails
    {

        [Key]
        public int contactDetailsId { get; set; }       
  
        [Required]
        public string traderId { get; set; }       

        public ApplicationUser Trader { get; set; }

        public List<Email> emails { get; set; }

        public List<Phone> phones { get; set; }

        public List<SocialNetwork> socialNetworks { get; set; }

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

        public List<EmailDTO> emails { get; set; }

        public List<PhoneDTO> phones { get; set; }

        public List<SocialNetworkDTO> socialNetworks { get; set; }

        public ContactDetailsDTO() { }

        public ContactDetailsDTO(int id, string traid)
        {
            contactDetailsId = id;
            traderId = traid;
        }
    }
}