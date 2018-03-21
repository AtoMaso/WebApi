using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models{
    public class Correspondence
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(20)]
        public string subject { get; set; }   // the subject will be the trade subject description

        [Required, MaxLength(70)]
        public string message { get; set; }

        [Required, MaxLength(500)]
        public string content { get; set; }

        [Required, MaxLength(10)]
        public string statusSender { get; set; }   // Read, New, Archived, Deleted

        [Required, MaxLength(10)]
        public string statusReceiver { get; set; }   // Read, New, Archived, Deleted

        [Required]
        public DateTime dateSent { get; set; }

        [Required]
        public int tradeId { get; set; }

        public Trade Trade { get; set; }

        [Required]
        public string traderIdReciever { get; set; }

        [Required]
        public string traderIdSender { get; set; }

        public Correspondence() { }

        public Correspondence(int idnum, string subj, string text, string stas, DateTime dt, int trid, string traderSen, string traderRec , string con, string star) // the subject will be the Trade object
        {
            id = idnum;
            subject = subj;
            message = text;
            statusSender = stas;
            dateSent = dt;
            tradeId = trid;          
            traderIdSender = traderSen;
            traderIdReciever = traderRec;
            content = con;
            statusReceiver = star;
        }
    }



    public class CorrespondenceDTO
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(20)]
        public string subject { get; set; }   // the subject will be the trade name

        [Required, MaxLength(70)]
        public string message { get; set; }

        [Required, MaxLength(500)]
        public string content { get; set; }

        [Required, MaxLength(10)]
        public string statusSender { get; set; }   // Red, New, Archived, Deleted

        [Required, MaxLength(10)]
        public string statusReceiver { get; set; }   // Read, New, Archived, Deleted

        [Required]
        public DateTime dateSent { get; set; }

        [Required]
        public int tradeId { get; set; }

        [Required]
        public string traderIdReciever { get; set; }

        [Required]
        public string traderIdSender { get; set; }

        public string sender { get; set; }

        public string receiver { get; set; }

    
    }

}