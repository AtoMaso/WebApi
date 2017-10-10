using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    //[Serializable]
    public class Team
    {       
        [Key]
        public int TeamId { get; set; }
        [Required, MaxLength(100)]
        public string  TeamName { get; set; }
        [Required, MaxLength(500)]
        public string TeamDescription { get; set; }
        public string TeamLeadId { get; set; }    
        public string ProjectDirectorId { get; set; }
        public string ProjectManagerId { get; set; }     
        public int BusinessLineId { get; set; }              
        public virtual BusinessLine BusinessLine { get; set; }     
        public int LocalityId { get; set; }      
        public virtual Locality Locality { get; set; }
        //public virtual List<ApplicationUser> Members { get; set; }

        public Team() { }

        public Team(string name, string description,
                       string director, string pmanager, string teamlead,
                       int locality, int businessline)
        {      
            TeamName = name;
            TeamDescription = description;
            ProjectDirectorId = director;
            ProjectManagerId = pmanager;
            TeamLeadId = teamlead;
            LocalityId = locality;
            BusinessLineId = businessline;
        }
    }

    //[Serializable]
    public class TeamDTO
    {
        public int TeamId { get; set; }       
        public string TeamName { get; set; }
        public string TeamLeadId { get; set; }
        public string TeamLead { get; set; }        
        public string ProjectDirectorId { get; set; }
        public string ProjectDirector { get; set; }      
        public string ProjectManagerId { get; set; }
        public string ProjectManager { get; set; }       
        public string BusinessLineName { get; set; }
    }

    //[Serializable]
    public class TeamDetailDTO
    {
        public int TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }              
        public string TeamDescription { get; set; }
        public string TeamLeadId { get; set; }
        public string TeamLead { get; set; }
        public string ProjectDirectorId { get; set; }
        public string ProjectDirector { get; set; }
        public string ProjectManagerId { get; set; }
        public string ProjectManager { get; set; }             
        public int BusinessLineId { get; set; }
        public string BusinessLineName { get; set; }     
        public int? LocalityId { get; set; }      
        public string LocalityNumber { get; set; }
        public string LocalityStreet { get; set; }
        public string LocalitySuburb { get; set; }
        public string LocalityCity { get; set; }
        public string LocalityPostcode { get; set; }
        public string LocalityState { get; set; }
    }
}
