using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    //[Serializable]
    public class Position
    {
        // primary key       
        [Key]
        public int PositionId { get; set; }
        [Required]
        public string PositionTitle { get; set; }

        public Position() { }

        public Position(string title)
        {          
            PositionTitle = title;
        }
    }
}