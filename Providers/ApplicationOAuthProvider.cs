using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApi.Models;
using System.Text;
using System.Globalization;

namespace WebApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private static ApplicationUserManager userManager;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("The user name or password is incorrect.");
                    // this is added to get difference between return code 400 and wrong username and password
                    context.Response.Headers.Add("Authorization response", new[] { "Failed" });            
                    return;
                }
                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
               //ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

                // add the roles in the response
                string roles = string.Empty;
                IList<string> list = userManager.GetRoles(user.Id);               
                foreach (string s in list)
                {
                    if (roles != string.Empty) { roles = roles + "," + s; }
                    else { roles = s;  }
                }
               
                AuthenticationProperties properties = CreateProperties(user.UserName, user.Id, user.Name, roles);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                bool result = context.Validated(ticket);

                //context.Request.Context.Authentication.SignIn(cookiesIdentity);
          }
          catch (Exception exc)
          {                
                // Could not retrieve the user due to error.  
                context.SetError(exc.Message);            
                return;
            }
        }



        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context){  
               //validate your client  
               //var currentClient = context.ClientId;  
 
               //if (Client does not match)  
               //{  
               //    context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");  
               //    return Task.FromResult<object>(null);  
               //}  
 
               // Change authentication ticket for refresh token requests  
                var newIdentity = new ClaimsIdentity(context.Ticket.Identity);  
                newIdentity.AddClaim(new Claim("newClaim", "newValue"));  
  
                var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);  
                context.Validated(newTicket);  
  
                return Task.FromResult<object>(null);  
        }


        /// <summary>
        /// This method controls thevalues of the token returned
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
            {
                // change the dates to local dates for .issued and .expired values
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    if (property.Key == ".issued")
                    {
                        context.AdditionalResponseParameters.Add(property.Key, context.Properties.IssuedUtc.Value.ToString("o", (IFormatProvider)CultureInfo.InvariantCulture));                 
                    }
                    else if (property.Key == ".expires")
                    {
                        context.AdditionalResponseParameters.Add(property.Key,context.Properties.ExpiresUtc.Value.ToString("o"));
                    }
                    else
                    {
                        context.AdditionalResponseParameters.Add(property.Key, property.Value);
                    }
                }

                return Task.FromResult<object>(null);
            }


        /// <summary>
        /// this method is colled when client_id and client_secret are used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {                      
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);      
        }



        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }


        public static AuthenticationProperties CreateProperties(string userName, string id, string name, string roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "id", id },
                { "name", name},
                { "roles", roles }
            };
            return new AuthenticationProperties(data);
        }

    }
}