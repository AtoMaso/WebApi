using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Suburb
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(45)]
        public string name { get; set; }
       
        public int postcodeId { get; set; }

        public Postcode Postcode { get; set; }

        public Suburb() { }

        public Suburb(int pcid, string sname, int plid)
        {
            id = plid;
            name = sname;
            postcodeId = plid;
        }
    }


    public class SuburbDTO
    {
        public int id { get; set; }
     
        public string name { get; set; }
      
        public int postcodeId { get; set; }
      
        public string postcodeNumber { get; set; }

    }
}