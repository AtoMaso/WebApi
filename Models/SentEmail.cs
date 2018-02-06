using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace WebApi.Models
{
    public class SentEmail
    {
        public string Subject { get; set; }
     
        public MailAddress From { get; set; }
         
        public string To { get; set; }
         
        public string Text { get; set; }

        public string Html { get; set; }


        public SentEmail() { }

        public SentEmail(string subject, MailAddress from, string to, string message, string html)
        {
            Subject = subject;
            From = from;
            To = to;
            Text = message;
            Html = html;
        }
    }
}