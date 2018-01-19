using System.ComponentModel.DataAnnotations;
using System.Linq;

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