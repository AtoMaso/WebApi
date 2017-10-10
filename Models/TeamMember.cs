using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    //[Serializable]
    public class TeamMember
    {
        //primary key     
        [Key]       
        [Column(Order = 1)]     
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        // primary key       
        [Key]
        [Column(Order = 2)]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public TeamMember() { }

        public TeamMember(int teamid, string memberId)
        {
            TeamId = teamid;
            Id = memberId; 
        }
    }
}