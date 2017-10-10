using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    //[Serializable]
    public class Attachement
    {
        //primary key       
        [Key]
        public long AttachementId { get; set; }
        public string ArticleId { get; set; }
        // Does not serialise with this properly
        //public Article Article { get; set; }
        [Required]
        public string PhysicalPath { get; set; }
        [Required]
        public string Name { get; set; }
        public Attachement() { }

        public Attachement(string artid, string path, string name)
        {
            //AttachementId = id;
            ArticleId = artid;    
            PhysicalPath = path;
            Name = name;
        }
    }
}