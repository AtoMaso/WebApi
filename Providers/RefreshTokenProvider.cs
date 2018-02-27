using Microsoft.Owin.Security;  
using Microsoft.Owin.Security.Infrastructure;  
using System;  
using System.Collections.Concurrent;  
using System.Threading.Tasks;  
 
namespace WebApi.Providers
{  
    public class RefreshTokenProvider : IAuthenticationTokenProvider  
    {  
        // this is the data is stored, we should have database for it
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();  
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {  
            var guid = Guid.NewGuid().ToString();  
  
            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)  
            {  
                IssuedUtc = context.Ticket.Properties.IssuedUtc,  
                ExpiresUtc = DateTime.Now.AddMinutes(20)//DateTime.UtcNow.AddYears(1)  
            };  
            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);  
  
            _refreshTokens.TryAdd(guid, refreshTokenTicket);  
  
            // consider storing only the hash of the handle  
            context.SetToken(guid);  
        }  
  
        public void Create(AuthenticationTokenCreateContext context)
        {  
            throw new NotImplementedException();  
        }  
  
        public void Receive(AuthenticationTokenReceiveContext context)
        {  
            throw new NotImplementedException();  
        }  
  
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {  
            AuthenticationTicket ticket;  
           string header = context.OwinContext.Request.Headers["Authorization"];  

            if (_refreshTokens.TryRemove(context.Token, out ticket))  
           {  
                context.SetTicket(ticket);  
            }  
        }  
    }
}  
