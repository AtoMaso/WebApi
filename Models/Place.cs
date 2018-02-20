using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Place
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(25)]
        public string name { get; set; }

        public int stateId { get; set; }

        //public State State { get; set; }

        public Place() { }

        public Place(int plid, string plname, int stid)
        {
            id = plid;
            name = plname;
            stateId = stid;
        }
    }


    public class PlaceDTO
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(25)]
        public string name { get; set; }

        [Required]
        public int stateId { get; set; }

       public string stateName { get; set; }

        public PlaceDTO() { }

        public PlaceDTO(int plid, string plname, int stid, string stname)
        {
            id = plid;
            name = plname;
            stateId = stid;
            stateName = stname;
        }
    }
}