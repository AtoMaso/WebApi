using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class State
    {
        [Key]
        public int id{ get; set; }

        [Required, MaxLength(25)]
        public string name { get; set; }

        public List<Place> places { get; set; }

        public State() { }

        public State(int stid, string stname)
        {
            id = stid;
            name = stname;
        }
    }


    public class StateDTO
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(25)]
        public string name { get; set; }

        public List<Place> places { get; set; }

        public StateDTO() { }

        public StateDTO(int stid, string stname)
        {
            id = stid;
            name = stname;
        }
    }
}