using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    //[Serializable]
    public class Level
    {
        // primary key    
        [Key]    
        public long LevelId { get; set; }
        [Required]
        public string LevelTitle { get; set; }

        public Level() { }

        public Level( string title)
        {         
            LevelTitle = title;
        }
    }
}