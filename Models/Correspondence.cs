using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace WebApi.Models
{
    public class Correspondence
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(30)]
        public string subject { get; set; }   // the subject will be the trade subject description

        [Required, MaxLength(200)]
        public string message { get; set; }

        [Required, MaxLength(20)]
        public string status { get; set; }   // Red, New, Deleted

        [Required]
        public DateTime dateSent { get; set; }

        [Required]
        public int tradeId { get; set; }

        public Trade Trade { get; set; }

        [Required]
        public string traderId { get; set; }

        //public ApplicationUser Trader { get; set; }

        public Correspondence() { }

        public Correspondence(int idnum, string subj, string text, string sta, DateTime dt, int trid, string trerid ) // the subject will be the Trade object
        {
            id = idnum;
            subject = subj;
            message = text;
            status = sta;
            dateSent = dt;
            tradeId = trid;
            traderId = trerid;
        }
    }



    public class CorrespondenceDTO
    {
      
        public int id { get; set; }

        public string subject { get; set; }   // the subject will be the trade subject description

        public string message { get; set; }

        public string status { get; set; }

        public DateTime dateSent { get; set; }

        public int tradeId { get; set; }

        public string traderId { get; set; }

        public CorrespondenceDTO() { }

        public CorrespondenceDTO(int idnum, string subj, string text, string sta, DateTime dt, int trid, string trerid) // the subject will be the Trade object
        {
            id = idnum;
            subject = subj;
            message = text;
            status = sta;
            dateSent = dt;
            tradeId = trid;
            traderId = trerid;
        }
    }

}