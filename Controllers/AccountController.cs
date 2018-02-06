using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using WebApi.Models;
using System.Web.Http.Description;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http.Results;
using WebApi.Providers;

namespace WebApi.Controllers
{
    //[Serializable]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        private ContactDetailsController cdctr = new ContactDetailsController();
        private PersonalDetailsController pdctr = new PersonalDetailsController();
        private SecurityDetailsController sdctr = new SecurityDetailsController();


        public AccountController(){}

        public AccountController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {                
                return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationRoleManager>();             
            }
            private set
            {
                roleManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }


        // GET api/account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null              
            };
        }


     

        // TODO USE IT
        // POST api/account/Logout'
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok(new { message = "Logout successful." });
        }



        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        // TODO USE IT
        // POST api/account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        // POST api/account/Register        
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            string message = string.Empty;
            string userid = string.Empty;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }         

            try
            {
                    // check is trader is in database
                    var existingUser = UserManager.FindByEmail(model.Email);
                  
                    if (existingUser == null) {
                            // does not exists so create one
                            var newTrader = new ApplicationUser()
                            {                             
                                UserName = model.Email,
                                Email = model.Email,                                                       
                            };

                                                      
                            // An account can be created if there no existing one
                            IdentityResult resultCreate = await UserManager.CreateAsync(newTrader, model.Password);
                            if (!resultCreate.Succeeded)
                            {
                                foreach (string err in resultCreate.Errors) { message += err; }
                                ModelState.AddModelError("Message", "Trader Create Error:" + message + " Please contact the application administrator.");
                                return BadRequest(ModelState);
                            }


                            // add the role
                            IdentityResult roleResultRole = UserManager.AddToRole(newTrader.Id, "Trader");
                            if (!roleResultRole.Succeeded)
                            {
                                foreach (string err in roleResultRole.Errors) { message += err; }
                                ModelState.AddModelError("Message", "Trader Role Error: " + message + " Please contact the application administrator.");                      
                                return BadRequest(ModelState);
                            }

                            userid = newTrader.Id;
                           

                            string code = UserManager.GenerateEmailConfirmationToken(newTrader.Id);

                            //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = newTrader.Id, code = code }));

                            string Url = "http://localhost/api/account/ConfirmEmail?userid=" + newTrader.Id + "& code=" + code;

                            string body = "Please confirm your account by clicking <a href=\"" + Url + "\">here</a>";

                            UserManager.EmailService = new EmailService(UserManager, newTrader);

                            IdentityMessage messageIdentity = new IdentityMessage();
                            messageIdentity.Body = body;
                            messageIdentity.Destination = newTrader.Email;
                            messageIdentity.Subject = "Please confirm your account";
                           
                            await UserManager.EmailService.SendAsync(messageIdentity);

                            //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = newTrader.Id }));


                    // return ok if everything OK  
                    return Ok();
                   }    
                   else
                    {
                    // does exists as a trader the ADMIN guys will be added as script                
                    if (UserManager.IsInRole(existingUser.Id, "Trader"))
                    {
                        ModelState.AddModelError("Message", "Account with the email account provided already exist!");
                        return BadRequest(ModelState);
                    }
                    return Ok();
                }            
            }
            catch (Exception exc)
            {
                RollBackDatabaseChanges();

                UserManager.Delete(UserManager.FindById(userid));

                ModelState.AddModelError("Message", "An unexpected error occured during the creation of the account. Please contact the application administrator." + exc.Message);

                return BadRequest(ModelState);               
            }                                                     
        }
      
     

        //Added to use it for email confirmation
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("Message", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    var counter = 0;
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("Error" + counter, error);
                        counter++;
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
       
        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<System.Security.Claims.Claim> GetClaims()
            {
                IList<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
                claims.Add(new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new System.Security.Claims.Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                System.Security.Claims.Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }


        #endregion



        #region "Traders"    

        // GOOD 
        // GET localhost:5700/api/account/gettraders - this is for the list of traders
        [AllowAnonymous]
        [Route("GetTraders")]
        public IHttpActionResult GetTraders()
        {
            IQueryable<ApplicationUser> allusers = db.Users;           
            List<ApplicationUserListDTO> traders = new List<ApplicationUserListDTO>();

            try
            {
                if (allusers != null)
                {
                    foreach (ApplicationUser user in allusers)
                    {
                        if (UserManager.IsInRole(user.Id, "Trader"))
                        {
                            ApplicationUserListDTO dto = new ApplicationUserListDTO();

                            dto.traderId = user.Id;
                            dto.traderFirstName = (((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(user.Id)).Content).firstName;
                            dto.traderMiddleName = (((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(user.Id)).Content).middleName;
                            dto.traderLastName = (((OkNegotiatedContentResult<PersonalDetailsDTO>)pdctr.GetPersonalDetailsByTraderId(user.Id)).Content).lastName;
                            dto.traderContactEmail = user.Email;
                            dto.traderContactPhone = ((OkNegotiatedContentResult<ContactDetailsDTO>)cdctr.GetContactDetailsByTraderId(user.Id)).Content.Phones[0].phoneNumber;                            
                           
                            traders.Add(dto);
                        }
                    }
                }
                return Ok<List<ApplicationUserListDTO>>(traders);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting all traders!");
                return BadRequest(ModelState);
            }
        }


        // GOOD
        // GET: api/account/gettraders?traderId=string    -- to be used by a single traderid call
        [AllowAnonymous]
        [ResponseType(typeof(ApplicationUserDetailDTO))]
        [Route("GetTraders")]
        public IHttpActionResult GetTraders(string traderId)
        {
            ApplicationUser user = db.Users.Find(traderId);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                ApplicationUserDetailDTO trddto = new ApplicationUserDetailDTO();
                trddto.traderId = user.Id;                
                trddto.personalDetails = ((OkNegotiatedContentResult< PersonalDetailsDTO>) pdctr.GetPersonalDetailsByTraderId(user.Id)).Content;
                trddto.contactDetails = ((OkNegotiatedContentResult<ContactDetailsDTO>) cdctr.GetContactDetailsByTraderId(user.Id)).Content; 
                trddto.securityDetails = ((OkNegotiatedContentResult<SecurityDetailsDTO>)sdctr.GetSecurityDetailsByTraderId(user.Id)).Content;
                
                return Ok(trddto);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Message", "An unexpected error has occured during getting the trader details!");
                return BadRequest(ModelState);
            }
        }



        // PUT: api/Account/PutMember/5
        [ResponseType(typeof(void))]
        [Route("PutTrader")]
        public async Task<IHttpActionResult> PutTrader(string id, ApplicationUser author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!id.Equals(author.Id))
            {
                return BadRequest();
            }

            db.Entry(author).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


     
        // when user registers as Trader the Register method is used
        // GET api/account/PostTrader
        [ResponseType(typeof(ApplicationUser))]
        [Route("PostTrader")]
        public async Task<IHttpActionResult> PostTrader(ApplicationUser passedTrader)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The data provided is not valid!");
                return BadRequest(ModelState);
            }

            try
            {
                // check is author in database as author or member
                var exist = UserManager.FindByEmail(passedTrader.Email);
                if (exist == null)
                {
                    // does not exists
                    // we haven't created new app user so we have no id and we have no username
                    passedTrader.UserName = passedTrader.Email;

                    // Business Rule: An account can be created if there no existing one
                    IdentityResult resultCreate = await UserManager.CreateAsync(passedTrader);
                    if (!resultCreate.Succeeded)
                    {
                        foreach (string err in resultCreate.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Create Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // add the role
                    IdentityResult resultRole = UserManager.AddToRole(passedTrader.Id, "Trader");
                    if (!resultRole.Succeeded)
                    {
                        foreach (string err in resultRole.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Role Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // Add DUMMY PASSWORD for the author                     
                    IdentityResult resultPassword = await UserManager.AddPasswordAsync(passedTrader.Id, "July2015!");
                    if (!resultPassword.Succeeded)
                    {
                        foreach (string err in resultPassword.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Password Error: " + message + " Please contact the application administrator.");
                        return BadRequest(ModelState);
                    }
                    // TODO SEND THE EMAIL WITH THE DUMMY PASSWORD TO THE AUTHOR

                    // create result dto to be sent back
                    ApplicationUserDetailDTO resultdto = CreateTraderToRetun(passedTrader);
                    await db.SaveChangesAsync();
                    // return ok if everything OK
                    return Ok(resultdto);
                }
                else
                {
                    // does exists
                    // Business Rule: add the role to the account if there is no existing role
                    if (UserManager.IsInRole(exist.Id, "Trader"))
                    {
                        ModelState.AddModelError("Message", "Trader with the credentials provided already exist!");
                        return BadRequest(ModelState);
                    }
                    // add the role now
                    IdentityResult resultRole = UserManager.AddToRole(exist.Id, "Trader");
                    if (!resultRole.Succeeded)
                    {
                        foreach (string err in resultRole.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Role Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // TODO SEND THE EMAIL WITH THE DUMMY PASSWORD TO THE AUTHOR

                    // Add DUMMY PASSWORD for the author                     
                    IdentityResult resultPassword = await UserManager.AddPasswordAsync(exist.Id, "July2015!");
                    if (!resultPassword.Succeeded)
                    {
                        foreach (string err in resultPassword.Errors) { message += err; }
                        ModelState.AddModelError("Trader Password Error", "Trader Password Error: " + message + " Please contact the application administrator.");
                        return BadRequest(ModelState);
                    }

                    ApplicationUserDetailDTO resultdto = CreateTraderToRetun(exist);
                    await db.SaveChangesAsync();
                    // return Ok if everything is OK
                    return Ok(resultdto);
                }

            }
            catch (Exception)
            {
                RollBackDatabaseChanges();
                // log the exception
                ModelState.AddModelError("Message", "An unexpected error occured during the creation" +
                                                                " of the account. Please contact the application administrator.");
                return BadRequest(ModelState);
            }
        }



        private ApplicationUserDetailDTO CreateTraderToRetun(ApplicationUser passedTrader)
        {
           
            try
            {
                var dtoTrader = new ApplicationUserDetailDTO()
                {
                    traderId = passedTrader.Id,
                    //personalDetailsId = pd,
                    //personalDetails = (PersonalDetails)db.PersonalDetails.Where(personal => personal.pdId == pd),
                    // to come here personal details
                    // security details
                    // contact details
                    //TeamName = te.TeamName
            };
                return dtoTrader;
            }
            catch (Exception)
            {
                // the exception will be bubled up
                throw;
            }

        }


        
        // GET api/account/deletetrader?id=xx
        [ResponseType(typeof(ApplicationUser))]
        [Route("DeleteTrader")]
        public async Task<IHttpActionResult> DeleteTrader(string traderId)
        {
            TradesController artCon = new TradesController();

            string message = string.Empty;
            ApplicationUser trader = db.Users.Find(traderId);
            if (trader == null)
            {
                // prepare the message
                ModelState.AddModelError("Message", "The author account can not be found!");
                // TODO logging here an unexpected error has occured
                return BadRequest(ModelState);
            }
            try
            {

                // if account has multiple roles
                if (trader.Roles.Count() > 1)
                {

                    // we get the articles as cascading delete of articles will not delete the physical files                       
                    var arts = from art in db.Trades
                               where (art.traderId == traderId)
                               select art;
                    // removing of articles will take care of removing the article's attachement 
                    // (multiple article will remove multiple attachements from the table) and by calling
                    // the article controller we will delete the physical articles file also
                    foreach (Trade ar in arts) { await artCon.DeleteTrade(ar.tradeId); }

                    // Business Rule: Remove the author role of the account
                    IdentityResult resultRole = await UserManager.RemoveFromRoleAsync(traderId, "Trader");
                    if (!resultRole.Succeeded)
                    {
                        foreach (string err in resultRole.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Role Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // Bussiness Rule: Remove the password when removing the author role               
                    IdentityResult resultPassword = await UserManager.RemovePasswordAsync(traderId);
                    if (!resultPassword.Succeeded)
                    {
                        foreach (string err in resultPassword.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Password Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    // we get the trades as cascading delete of trades will not delete the physical files                       
                    var arts = from art in db.Trades
                               where (art.traderId == traderId)
                               select art;

                    // Business Rule: Remove the account and roles associcated when we have single role account     
                    // removing of author will take care of removing the author's articles and with that the article's attachement 
                    // (multiple article will remove multiple attachements from the table)                
                    db.Users.Remove(trader);

                    // the trade controller method will delete the physical uploaded trade images files
                    foreach (Trade ar in arts) { artCon.DeletePhysicalTrade(ar.tradeId); }
                }
                await db.SaveChangesAsync();
                return Ok(trader);
            }
            catch (Exception Exc)
            {
                string error = Exc.InnerException.Message;

                // TODO // log the exception EXC on the server side
                RollBackDatabaseChanges();
                // prepare the message
                ModelState.AddModelError("Message", "An unexpected error occured during deleting the author account!");
                // TODO logging here an unexpected error has occured
                return BadRequest(ModelState);
            }
        }


        private bool TraderExists(string id)
        {
            return UserManager.FindById(id).Id != string.Empty;
        }

     #endregion


        private void RollBackDatabaseChanges()
        {
            // Undo the changes of the all entries. 
            foreach (DbEntityEntry entry in db.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    // Under the covers, changing the state of an entity from  
                    // Modified to Unchanged first sets the values of all  
                    // properties to the original values that were read from  
                    // the database when it was queried, and then marks the  
                    // entity as Unchanged. This will also reject changes to  
                    // FK relationships since the original value of the FK  
                    // will be restored. 
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    // If the EntityState is the Deleted, reload the date from the database.   
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            }
        }
     
    }
}

