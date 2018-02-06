using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebApi.Models;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace WebApi.Providers
{
    public class EmailService : IIdentityMessageService
    {

        public ApplicationUserManager userManager { get; set; }
        public ApplicationUser sentEmailUser { get; set; }

        public string body { get; set; }

        public EmailService() { }

        public EmailService(ApplicationUserManager usermanager,ApplicationUser user){
            userManager = userManager;
            sentEmailUser = user;       
        }


        public async Task SendAsync(IdentityMessage message)
        {
           await SendEmailAsync(message);

        }


        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task SendEmailAsync(IdentityMessage message)
        {
                   
            var myMessage = new MailMessage();                      
            myMessage.Body = message.Body;
            myMessage.Subject = message.Subject;
            myMessage.Sender = new MailAddress("srbinovskim@optusnet.com.au");
            MailAddressCollection mc = new MailAddressCollection();
            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"], ConfigurationManager.AppSettings["emailService:Password"]);        
            var client = new SmtpClient("mail.optusnet.com.au");
            client.Credentials = credentials;

            // Send the email.
            if (client != null)
            {                       
                client.Send(myMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }
}