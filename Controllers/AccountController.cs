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
using WebApi.Providers;
using System.Web.Http.Results;
using System.Web.Configuration;
using System.Net.Mail;

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
        private PersonalDetailsController pdctr = new PersonalDetailsController();
        private ForgotPasswordsController fpctr = new ForgotPasswordsController();

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


        //public IHttpActionResult ManageAccount(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        string page = "~/html/" + id + ".html";
        //        return new FilePathResult(page, "text/html");
        //    }
        //    return new FilePathResult("~/html/login.html", "text/html");
        //}


        // GET api/account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel UserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                Username = User.Identity.GetUserName(),               
                HasRegistered = externalLogin == null,              
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null              
            };
        }


        //GET api/account
        public IEnumerable<ApplicationUser> GetAll()
        {
            return db.Users;
        }


        // GET api/account/"asaddad"
        public ApplicationUser GetById(string Id)
        {
            return db.Users.Find(Id);
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


        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")] //  this is NEW
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordBindingModel model)
        {          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok("ForgotPasswordConfirmation");
                }

                try
                {                   
                    // use AccessFailedCount user property to store the change password attempts                   
                    ForgotPassword rec = fpctr.GetForgotPasswordByUserid(user.Id);          
                    if(rec == null || rec.createdDt.Day != DateTime.Today.Day)                           
                    {
                                            
                        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        string resetpasswordurl = WebConfigurationManager.AppSettings["ResetPasswordUrl"];
                        string callbackUrl = resetpasswordurl + "?code=" + code;
                        string body = "Please confirm your identity by clicking on this: <a href=\"" + callbackUrl + "\">Link</a>. Confirmation email will be sent to you.";

                        IdentityMessage messageIdentity = new IdentityMessage();
                        messageIdentity.Body = body;
                        messageIdentity.Destination = user.Email;
                        messageIdentity.Subject = "Reset Password";

                        UserManager.EmailService = new EmailService(UserManager, user);
                        await UserManager.EmailService.SendAsync(messageIdentity);

                        return Ok("ForgotPasswordConfirmation");
                    }
                    else
                    {
                        if(rec.createdDt.Day == DateTime.Today.Day)
                        {
                            ModelState.AddModelError("Message", "Changing password multiple times in a day is not permitted!");
                            return BadRequest(ModelState);
                        }                      
                    }
                  
                }
                catch (Exception ex)
                {
                    string str = ex.InnerException.Message;                    
                }
                return BadRequest(ModelState);
            }
            else
            {
                return BadRequest(ModelState);
            }
     
        }


        //GOOD
        // POST api/account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
        {

            if (!ModelState.IsValid)
            {            
                ModelState.AddModelError("Message", "The data provided is invalid!");
                return BadRequest(ModelState);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {              
                ModelState.AddModelError("Message", "User can not be found!");
                return BadRequest(ModelState);
            }

            var result = await UserManager.VerifyUserTokenAsync(user.Id, "ResetPassword", model.Code);          
            if (result)
            {
                // add record in the forgot password table count
                ForgotPassword newRecord = new ForgotPassword();
                newRecord.userId = user.Id;
                newRecord.createdDt = DateTime.Now.ToLocalTime();
                newRecord.attemptsCount = 1;
                await fpctr.PostForgotPassword(newRecord);

                var resultPassword = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);
                if (resultPassword.Succeeded)
                {
                    ApplicationUserListDTO trader = new ApplicationUserListDTO();
                    trader = ((OkNegotiatedContentResult<ApplicationUserListDTO>)GetTraderByTraderId(user.Id)).Content;
                    return Ok<ApplicationUserListDTO>(trader);
                }
                else
                {
                    ModelState.AddModelError("Message", "Error saving your new password. Please contact the application admin!");
                    return BadRequest(ModelState);
                }
                       
            }
            else
            {
                ModelState.AddModelError("Message", "Invalid code!");
                return BadRequest(ModelState);
            }
          
        }


        // GOOD
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Message", "The data provided is invalid!");
                return BadRequest(ModelState);
            }


            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Message", "The old password is invalid!");
                return BadRequest(ModelState);                
            }

            // if all good
            ApplicationUserListDTO trader = new ApplicationUserListDTO();
            trader = ((OkNegotiatedContentResult<ApplicationUserListDTO>)GetTraderByTraderId(User.Identity.GetUserId())).Content;        
            return Ok<ApplicationUserListDTO>(trader);
        }

     
        // GOOD 
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

                                if(model.Email == "srbinovskimirko@gmail.com")
                                {
                                    IdentityResult roleResultRoleAdmin = UserManager.AddToRole(newTrader.Id, "Admin");
                                    if (!roleResultRoleAdmin.Succeeded)
                                    {
                                        foreach (string err in roleResultRoleAdmin.Errors) { message += err; }
                                        ModelState.AddModelError("Message", "Admin Role Error: " + message + " Please contact the application administrator.");
                                        return BadRequest(ModelState);
                                    }
                               }

                         
                                // send an email to the person claiming the tarder account
                                userid = newTrader.Id;
                                string code = UserManager.GenerateEmailConfirmationToken(newTrader.Id);
                                var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = userid, code = code }));
                                string body = "Please confirm your account by clicking this: <a href=\"" + callbackUrl + "\">Link</a>. Email about your account confirmation will be sent to you.";

                                IdentityMessage messageIdentity = new IdentityMessage();
                                messageIdentity.Body = body;
                                messageIdentity.Destination = newTrader.Email;
                                messageIdentity.Subject = "Please confirm your account";

                                UserManager.EmailService = new EmailService(UserManager, newTrader);
                                await UserManager.EmailService.SendAsync(messageIdentity);                   
                                
                                // if everything return OK  
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

                        if(exc.GetType() == typeof(SmtpException))
                        {
                            ModelState.AddModelError("Message", exc.Message);
                        }
                        else
                        {
                            ModelState.AddModelError("Message", "An unexpected error occured during the creation of the account. Please contact the application administrator.");
                        }
                    
                        return BadRequest(ModelState);               
            }                   
        }
      
     
        //GOOD 
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
                // send email that confirmation has been successfull
               
                string loginurl = WebConfigurationManager.AppSettings["LoginUrl"];             
                IdentityMessage messageIdentity = new IdentityMessage();               
                messageIdentity.Body = "You have successfuly confirmed you new trader account. Use the <a href=\"" + loginurl + "\">Link</a> to login to the application.";
                messageIdentity.Destination = UserManager.FindById(userId).Email;
                messageIdentity.Subject = "Confimation successful";

                UserManager.EmailService = new EmailService(UserManager, UserManager.FindById(userId));
                await UserManager.EmailService.SendAsync(messageIdentity);

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


        //NEW ADDED
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        // NEW ADDED
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }


        //public void Update(User userParam, string password = null)
        //{
        //    var user = _context.Users.Find(userParam.Id);

        //    if (user == null)
        //        throw new AppException("User not found");

        //    if (userParam.Username != user.Username)
        //    {
        //        // username has changed so check if the new username is already taken
        //        if (_context.Users.Any(x => x.Username == userParam.Username))
        //            throw new AppException("Username " + userParam.Username + " is already taken");
        //    }

        //    // update user properties
        //    user.FirstName = userParam.FirstName;
        //    user.LastName = userParam.LastName;
        //    user.Username = userParam.Username;

        //    // update password if it was entered
        //    if (!string.IsNullOrWhiteSpace(password))
        //    {
        //        byte[] passwordHash, passwordSalt;
        //        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        //        user.PasswordHash = passwordHash;
        //        user.PasswordSalt = passwordSalt;
        //    }

        //    _context.Users.Update(user);
        //    _context.SaveChanges();
        //}


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
        // GET localhost:5700/api/account/gettraders - this is for the LIST of TRADERS
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
                            ApplicationUserListDTO trddto = new ApplicationUserListDTO();

                            trddto.traderId = user.Id;
                            trddto.username = user.UserName;
                            trddto.email = user.Email;
                            trddto.emailconfirmed = user.EmailConfirmed;
                            trddto.passwordhash = user.PasswordHash;
                            traders.Add(trddto);
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
        [Route("GetTraderByTraderId")]
        public IHttpActionResult GetTraderByTraderId(string traderId)
        {
            ApplicationUser user = db.Users.Find(traderId);
            if (user == null)
            {
                ModelState.AddModelError("Message", "Trader not found!");
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationUserListDTO trddto = new ApplicationUserListDTO();
                trddto.traderId = user.Id;                              
                trddto.username = user.UserName;
                trddto.email = user.Email;
                trddto.emailconfirmed = user.EmailConfirmed;
                trddto.passwordhash = user.PasswordHash;

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
        public async Task<IHttpActionResult> PutTrader(string id, ApplicationUser trader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!id.Equals(trader.Id))
            {
                return BadRequest();
            }

            db.Entry(trader).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraderExists(id))
                {
                    ModelState.AddModelError("Message", "Trader not found!");
                    return BadRequest(ModelState);
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
            TradesController trdCon = new TradesController();

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

                    // Business Rule: Remove the trader role of the account
                    IdentityResult resultRole = await UserManager.RemoveFromRoleAsync(traderId, "Trader");
                    if (!resultRole.Succeeded)
                    {
                        foreach (string err in resultRole.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Role Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // Bussiness Rule: Remove the password when removing the trader role               
                    IdentityResult resultPassword = await UserManager.RemovePasswordAsync(traderId);
                    if (!resultPassword.Succeeded)
                    {
                        foreach (string err in resultPassword.Errors) { message += err; }
                        ModelState.AddModelError("Message", "Trader Password Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
              
                    // deletion of trades will not delete the physical files                       
                    var arts = from art in db.Trades
                               where (art.traderId == traderId)
                               select art;
                                  
                    // the trade controller method will delete the physical uploaded trade images files
                    foreach (Trade ar in arts) { trdCon.DeletePhysicalImages(ar.tradeId); }                 

                    // Business Rule: Remove the account and roles associcated when we have single role account     
                    // removing of trader will take care of removing the traders's trades and with that the images's  
                    // (multiple trades will remove multiple images from the tables)                
                    db.Users.Remove(trader);
                                  
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

