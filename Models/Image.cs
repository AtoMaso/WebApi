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
    public class Image
    {       
        [Key]       
        public int imageId { get; set; }

        [Required, MaxLength(70)]
        public string url { get; set; }

        [Required, MaxLength(70)]
        public string title { get; set; }

        public int tradeId { get; set; }

        public Trade Trades { get; set; }

        public Image() { }

        public Image( int id, int passedTradeId, string passedUrl , string passedTitle)
        {
            imageId = id;
            tradeId = passedTradeId;
            url = passedUrl;
            title = passedTitle;
        }
    }

    public class ImageDTO
    {
    
        public int imageId { get; set; }

        [Required, MaxLength(70)]
        public string url { get; set; }

        [Required, MaxLength(70)]
        public string title { get; set; }

        public int tradeId { get; set; }

        public ImageDTO() { }

    }

}
