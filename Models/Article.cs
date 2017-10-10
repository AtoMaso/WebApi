using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    //[Serializable]
    public class Article
    {
        public string ArticleId { get; set; }
        [Required, MaxLength(80)]
        public string Title { get; set; }
        [Required]
        public DateTime DatePublished { get; set; }               
        [Required, MaxLength(200)]
        public string Flash { get; set; }
        public int CategoryId { get; set; }
        // Navigation property
        public Category Category { get; set; }       
        [Required, MaxLength(8000)]
        public string Content { get; set; }
        public string AuthorId { get; set; }
        // Navigation property  
        public ApplicationUser Author { get; set; }       
        public virtual List<Attachement> Attachements { get; set; }

        public Article() { }

        public Article(string articleId, string title, string flash, string content,
                            DateTime datepublished, int categoryid, string id)
        {
            ArticleId = articleId;
            Title = title;           
            Flash = flash;
            Content = content;
            DatePublished = datepublished;
            CategoryId = categoryid;            
            AuthorId = id;
        }
     
    }

    //[Serializable]
    public class ArticleDTO
    {
        public string ArticleId { get; set; }
        public string Title { get; set; }      
        public string Flash { get; set; }        
        public DateTime DatePublished { get; set; }
        public string CategoryName { get; set; }        
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }

    }

    //[Serializable]
    public class ArticleDetailDTO
    {
        public string ArticleId { get; set; }       
        public string Title { get; set; }      
        public DateTime DatePublished { get; set; }
        public string Flash { get; set; }       
        public string CategoryName { get; set; }               
        public string Content { get; set; }  
        public string AuthorId { get; set; }           
        public string AuthorName { get; set; }
        public List<Attachement> Attachements { get; set; }

    }
}
