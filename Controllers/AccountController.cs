﻿using System;
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

namespace WebApi.Controllers
{
    //[Serializable]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext db = new ApplicationDbContext();

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
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
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
                _roleManager = value;
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


        //Added to use it for email confirmation
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
          if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
          {
                ModelState.AddModelError("", "User Id and Code are required");
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

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.oldPassword,
                model.newPassword);
            
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }         

            try
            {
                    // check is author in database as author or member
                    var exist = UserManager.FindByEmail(model.email);
                    // Include the ato user name as a combination between ATO USER NAME and ATO EMAIL account ????????????                  
                    if (exist == null) {
                            // does not exists so create one
                            var newTrader = new ApplicationUser()
                            {                             
                                UserName = model.email,
                                Email = model.email,
                                PhoneNumber = "NO DATA",                           
                            };
                            // Business Rule: An account can be created if there no existing one
                            IdentityResult resultCreate = await UserManager.CreateAsync(newTrader, model.password);
                            if (!resultCreate.Succeeded)
                            {
                                foreach (string err in resultCreate.Errors) { message += err; }
                                ModelState.AddModelError("Trader Create Error", "Trader Create Error:" + message + " Please contact the application administrator.");
                                return BadRequest(ModelState);
                            }
                            // add the role
                            IdentityResult roleResultRole = UserManager.AddToRole(newTrader.Id, "Trader");
                            if (!roleResultRole.Succeeded)
                            {
                                foreach (string err in roleResultRole.Errors) { message += err; }
                                ModelState.AddModelError("Trader Role Error", "Trader Role Error: " + message + " Please contact the application administrator.");                      
                                return BadRequest(ModelState);
                            }
                            // return ok if everything OK
                            return Ok();
                   }    
                   else
                    { 
                        // does exists
                        // Business Rule: add the role to the account if there is no existing role
                        if (UserManager.IsInRole(exist.Id, "Trader"))
                        {
                            ModelState.AddModelError("Exists", "Trader with the credentials provided already exist!");                            
                            return BadRequest(ModelState);
                        }
                        // add the role now
                        IdentityResult roleResultRole = UserManager.AddToRole(exist.Id, "Trader");
                        if (!roleResultRole.Succeeded)
                        {                          
                            foreach (string err in roleResultRole.Errors) { message += err; }
                            ModelState.AddModelError("Trader Role Error", "Trader Role Error: " + message + " Please contact the application administrator.");
                            return BadRequest(ModelState);
                        }
                        // Add the password for the author                     
                        IdentityResult resultPassword = await UserManager.AddPasswordAsync(exist.Id, model.password);
                        if (!resultPassword.Succeeded)
                        {                               
                            foreach (string err in resultPassword.Errors) { message += err; }
                            ModelState.AddModelError("Trader Password Error", "Trader Password Error: " + message + " Please contact the application administrator.");
                            return BadRequest(ModelState);
                        }
                        // return Ok if everything is OK
                        return Ok();
                }            
            }
            catch (Exception)
            {
                RollBackDatabaseChanges();
                ModelState.AddModelError("Trader Unexpected Error", "An unexpected error occured during the creation" +
                                                                " of the account. Please contact the application administrator.");
                // log the exception
                return BadRequest(ModelState);               
            }                                                     
        }
      
     
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
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




        //// MEMBERS ARE DONE EXCEPT UPDATE
        #region "Team Members"

        //// DONE - WORKS
        //// GET: api/account/GetMembers
        //[AllowAnonymous]
        //[Route("GetMembers")]
        //public IHttpActionResult GetMembers()
        //{
        //    IQueryable<ApplicationUser> allusers = db.Users;
        //    IQueryable<Level> levs = db.Levels;
        //    IQueryable<Position> pos = db.Positions;
        //    IQueryable<TeamMember> teammem = db.TeamMembers;
        //    List<ApplicationUserDTO> dtomembers = new List<ApplicationUserDTO>();


        //    try
        //    {
        //        if (allusers != null)
        //        {
        //            foreach (ApplicationUser member in allusers)
        //            {
        //                ApplicationUserDTO dtomember = new ApplicationUserDTO();
        //                dtomember.Id = member.Id;
        //                dtomember.Name = member.Name;
        //                dtomember.AtoUsername = member.AtoUsername;
        //                dtomember.PhoneNumber = member.PhoneNumber;
        //                dtomember.Email = member.Email;
        //                dtomember.Manager = member.Manager;

        //                dtomember.LevelTitle = levs.First(x => x.LevelId == member.LevelId).LevelTitle;
        //                dtomember.PositionTitle = pos.First(x => x.PositionId == member.PositionId).PositionTitle;

        //                dtomember.TeamId = member.TeamId;

        //                dtomembers.Add(dtomember);
        //            }

        //            return Ok(dtomembers);
        //        }
        //        else { return null; }
        //    }
        //    catch (Exception exc)
        //    {
        //        // TODO come up with logging solution
        //        // log the exc here
        //        ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all members!");
        //        return BadRequest();
        //    }

        //}


        //// DONE -WORKS
        //// GET: api/account/GetMembers?teamid=id
        //[AllowAnonymous]
        //[Route("GetMembers")]   
        //public IHttpActionResult GetMembers(int teamid)
        //{           
        //        IQueryable<ApplicationUser> allusers = db.Users;
        //        IQueryable<Level> levs = db.Levels;
        //        IQueryable<Position> pos = db.Positions;
        //        IQueryable<TeamMember> teammembers = db.TeamMembers;
        //        List<ApplicationUserDTO> dtomembers = new List<ApplicationUserDTO>();
        //        if (allusers != null)
        //        {
        //            try
        //            {                   
        //                var members = from member in allusers
        //                                 join teammember in teammembers on member.Id equals teammember.Id
        //                                 where (teammember.TeamId == teamid)
        //                                 select member;

        //                foreach(ApplicationUser member in members)
        //                {
        //                    ApplicationUserDTO dtomener = new ApplicationUserDTO();
        //                    dtomener.Id = member.Id;
        //                    dtomener.Name = member.Name;
        //                    dtomener.AtoUsername = member.AtoUsername;
        //                    dtomener.PhoneNumber = member.PhoneNumber;
        //                    dtomener.Email = member.Email;
        //                    dtomener.Manager = member.Manager;

        //                    dtomener.LevelTitle = levs.First(x => x.LevelId == member.LevelId).LevelTitle;
        //                    dtomener.PositionTitle = pos.First(x => x.PositionId == member.PositionId).PositionTitle;
        //                    // get the teamname of the team we are getting the member from                          
        //                    dtomener.TeamName = db.Teams.First(x => x.TeamId == teamid).TeamName;

        //                    dtomembers.Add(dtomener);
        //                }

        //                return Ok(dtomembers);
        //            }
        //            catch (Exception exc)
        //            {
        //                // TODO come up with loggin solution here
        //                string mess = exc.Message;
        //                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting team's members!");
        //                return BadRequest(ModelState);
        //            }
        //    }
        //    return BadRequest();
        //}


        //// DONE - WORKS we need ti the team id also here
        //// GET api/account/GetMember?id=id      
        //[AllowAnonymous]
        //[ResponseType(typeof(ApplicationUserDetailDTO))]
        //[Route("GetMember")]
        //public IHttpActionResult GetMember(string id, int teamid)
        //{                     
        //    ApplicationUser userfind = db.Users.Find(id);
        //    if (userfind == null)
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        Team team = db.Teams.Find(teamid);
        //        // get the records in the 
        //        var dtomembers = from member in db.Users
        //                         join teammember in db.TeamMembers on member.Id equals teammember.Id
        //                         where (member.Id == id && teammember.TeamId == teamid)
        //                         select member;
        //        ApplicationUser user = dtomembers.First();

        //        ApplicationUserDetailDTO dtomember = new ApplicationUserDetailDTO()
        //        {
        //                Id = user.Id,
        //                Name = user.Name,
        //                AtoUsername = user.AtoUsername,
        //                Email = user.Email,
        //                PhoneNumber = user.PhoneNumber,
        //                Workpoint = user.Workpoint,
        //                Manager = user.Manager,

        //                LevelTitle = db.Levels.First(lev => lev.LevelId == user.LevelId).LevelTitle,
        //                PositionTitle = db.Positions.First(pos => pos.PositionId == user.PositionId).PositionTitle,

        //                LocalityNumber = db.Localities.First(loc => loc.LocalityId == user.LocalityId).Number,
        //                LocalityStreet = db.Localities.First(loc => loc.LocalityId == user.LocalityId).Street,
        //                LocalitySuburb = db.Localities.First(loc => loc.LocalityId == user.LocalityId).Suburb,
        //                LocalityCity = db.Localities.First(loc => loc.LocalityId == user.LocalityId).City,
        //                LocalityPostcode = db.Localities.First(loc => loc.LocalityId == user.LocalityId).Postcode,
        //                LocalityState = db.Localities.First(loc => loc.LocalityId == user.LocalityId).State,

        //                TeamName = team.TeamName
        //            };
        //            return Ok(dtomember);
        //    }
        //    catch (Exception exc)
        //    {
        //        // TODO come up with loggin solution here
        //        string mess = exc.Message;
        //        ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the member!");
        //        return BadRequest(ModelState);
        //    }
        //}


        //// TO DO Update of a member
        //// PUT: api/Account/PutMember/5
        //[ResponseType(typeof(void))]       
        //[Route("PutMember")]
        //public async Task<IHttpActionResult> PutMember(string id, ApplicationUser member)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!id.Equals(member.Id))
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(member).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MemberExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}


        ////DONE - WORKS
        //// POST: api/account/PostMember       
        //[ResponseType(typeof(ApplicationUser))]
        //[HttpPost]
        //[AcceptVerbs("POST")]
        //[Route("PostMember")]
        //public async Task<IHttpActionResult> PostMember([FromBody] ApplicationUser passedMember)
        //{
        //    string message = string.Empty;
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("Unexpected", "The data provided is not valid!");
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {             
        //        // do we have account already
        //        var exist = UserManager.FindByEmail(passedMember.Email);
        //        if (exist == null)
        //        {
        //            // we haven't created new app user so we have no id and we have no username
        //            passedMember.UserName = passedMember.Email;
        //            // no account so we are creating one
        //            IdentityResult resultCreate = await UserManager.CreateAsync(passedMember);
        //            if (!resultCreate.Succeeded)
        //            {                       
        //                foreach(string err in resultCreate.Errors) { message += err; }
        //                // TODO come up with logging solution here                
        //                ModelState.AddModelError("Member Create Error", "Member Create Error: " + message + " Please contact the app admin!");
        //                return BadRequest(ModelState);
        //            }
        //            // add an member role to the account
        //            IdentityResult resultRole = await UserManager.AddToRoleAsync(passedMember.Id, "Member");
        //            if (!resultRole.Succeeded)
        //            {
        //                foreach (string err in resultRole.Errors) { message += err; }
        //                // TODO come up with logging solution here                
        //                ModelState.AddModelError("Member Role Error", "Member Role Error: " + message + " Please contact the app admin!");
        //                return BadRequest(ModelState);
        //            }
        //            // insert the record in the team member table for the team passed
        //            TeamMember tmr = new TeamMember(passedMember.TeamId, passedMember.Id);
        //            db.TeamMembers.Add(tmr);           
        //        }
        //        else
        //        {
        //            // if account has no member role then add one
        //            if (!UserManager.IsInRole(exist.Id, "Member"))
        //            {
        //                // add the member roleto the existing account
        //                UserManager.AddToRole(exist.Id, "Member");
        //            }
        //            // check for existance of the same record done on the client side
        //            // regardless what account it is we insert the record in the team member table for the passed team
        //            TeamMember tmr = new TeamMember(passedMember.TeamId, exist.Id);
        //            db.TeamMembers.Add(tmr);

        //            // we are updating the accout with the new data (it can be an author, or member of another team)
        //            // the passed member has no id, username, password or security stamp so we are getting them 
        //            // from the existing one to be able to update the acccount with the new details
        //            passedMember.Id = exist.Id;
        //            passedMember.UserName = exist.UserName;
        //            passedMember.PasswordHash = exist.PasswordHash;
        //            passedMember.SecurityStamp = exist.SecurityStamp;           
        //            // update the account
        //            await PutMember(exist.Id, passedMember);                  
        //        }
        //        // create a member details DTO to return to the caller        
        //        ApplicationUserDetailDTO resultdto = CreateMemberToRetun(passedMember);
        //        // save the changes to the database
        //        await db.SaveChangesAsync();
        //        // return OK if everything OK
        //        return Ok(resultdto);
        //    }
        //    catch (Exception exc)
        //    {
        //        RollBackDatabaseChanges();
        //        // TODO come up with loggin solution here
        //        string mess = exc.Message;
        //        ModelState.AddModelError("Unexpected", "An unexpected error has occured during creating the member records!");
        //        return BadRequest(ModelState);
        //    }

        //}


        //private ApplicationUserDetailDTO CreateMemberToRetun(ApplicationUser passedUser)
        //{               
        //        // return the existing member back, load all neccessary data                
        //        Level le = db.Levels.Find(passedUser.LevelId);
        //        Position po = db.Positions.Find(passedUser.PositionId);
        //        Locality lo = db.Localities.Find(passedUser.LocalityId);
        //        Team te = db.Teams.Find(passedUser.TeamId);

        //        var dtoMember = new ApplicationUserDetailDTO()
        //        {
        //            Id = passedUser.Id,
        //            Name = passedUser.Name,
        //            AtoUsername = passedUser.AtoUsername,
        //            Email = passedUser.Email,
        //            PhoneNumber = passedUser.PhoneNumber,
        //            Workpoint = passedUser.Workpoint,
        //            Manager = passedUser.Manager,

        //            LevelTitle = le.LevelTitle,
        //            PositionTitle = po.PositionTitle,
        //            TeamName = te.TeamName,

        //            LocalityNumber = lo.Number,
        //            LocalityStreet = lo.Street,
        //            LocalitySuburb = lo.Suburb,
        //            LocalityCity = lo.City,
        //            LocalityPostcode = lo.Postcode,
        //            LocalityState = lo.State
        //        };
        //        return dtoMember;          
        //}


        //// DONE - TEST IT
        //// GET api/account/DeleteMember?id=xx
        //[ResponseType(typeof(ApplicationUser))]
        //[Route("DeleteMember")]
        //public async Task<IHttpActionResult> DeleteMember(string id, int teamid)
        //{
        //    // Member is a type of ApplicationUser
        //    ApplicationUser member = db.Users.Find(id);      
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {                               
        //        // get the member we want to remove from the team           
        //        TeamMember tm = db.TeamMembers.First(x => x.Id == id && x.TeamId == teamid);
        //        if (tm != null)
        //        {
        //            // get all records for the member in the teammember table
        //            IQueryable<TeamMember> tms = db.TeamMembers.Where(x => x.Id == id);

        //            // Business Rule: remove the team member record always, make sure is deleted after the count is taken                 
        //            db.TeamMembers.Remove(tm);

        //            // Business Rule: if account has only member role and is a single team member then remove the member 
        //            // this should remove the member role from the account also and the teammember records also
        //            if (tms.Count() == 1 && member.Roles.Count() == 1) { db.Users.Remove(member); }

        //            // Business Rule: if account has more than one role and it is a member of one team onl, remove the member role 
        //            if (tms.Count() == 1 && member.Roles.Count() > 1) {  UserManager.RemoveFromRole(member.Id, "Member"); }

        //            // submitting the database request
        //            await db.SaveChangesAsync();

        //            // return the removed record
        //            return Ok(member);
        //        }
        //        return null;               
        //    }
        //    catch (Exception exc) {
        //        RollBackDatabaseChanges();
        //        // TODO come up with loggin solution here
        //        string mess = exc.Message;
        //        ModelState.AddModelError("Unexpected", "An unexpected error has occured during removing the member!");
        //        return BadRequest(ModelState);
        //    }
        //}

        //private bool MemberExists(string id)
        //{
        //    return UserManager.FindById(id).Id != string.Empty ;
        //}

        #endregion



        #region "Trade Traders"    

        // DONE - WORKS
        // GET api/account/GetTraders     
        [AllowAnonymous]
        [Route("GetTraders")]
        public IHttpActionResult GetTraders()
        {
            IQueryable<ApplicationUser> allusers = db.Users;
            //IQueryable<Team> te = db.Teams;
            List<ApplicationUserDTO> traders = new List<ApplicationUserDTO>();

            try
            {
                if (allusers != null)
                {
                    foreach (ApplicationUser a in allusers)
                    {
                        if (UserManager.IsInRole(a.Id, "Trader"))
                        {
                            ApplicationUserDTO dto = new ApplicationUserDTO();
                            dto.traderId = a.Id;
                            dto.firstName = a.firstName;
                            dto.secondName = a.secondName;
                            // to come here personal details
                            // security details
                            // contact details
                            traders.Add(dto);
                        }
                    }
                }
                return Ok(traders);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all authors!");
                return BadRequest(ModelState);
            }
        }


        // TODO maybe get authors by team can be done on 
        // list of all users and sorting or filtering by team
        // GET api/account/GetTraders?teamid
        [AllowAnonymous]
        [Route("GetTraders")]
        public IHttpActionResult GetTraders(int teamid)
        {
            IQueryable<ApplicationUser> allusers = db.Users;
            //IQueryable<Team> teams = db.Teams;
            List<ApplicationUserDTO> traders = new List<ApplicationUserDTO>();

            try
            {
                if (allusers != null)
                {
                    foreach (ApplicationUser user in allusers)
                    {
                        if (UserManager.IsInRole(user.Id, "Trader"))
                        {
                            ApplicationUserDTO dto = new ApplicationUserDTO();
                            dto.traderId = user.Id;
                            dto.firstName = user.firstName;
                            dto.secondName = user.secondName;
                            // to come here personal details
                            // security details
                            // contact details
                            traders.Add(dto);
                        }
                    }
                }
                return Ok(traders);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting all authors!");
                return BadRequest(ModelState);
            }
        }



        // DONE - WORKS
        // GET api/account/GetTrader?id=xx
        [AllowAnonymous]
        [ResponseType(typeof(ApplicationUserDetailDTO))]
        [Route("GetTrader")]
        public IHttpActionResult GetTrader(string id)
        {
            ApplicationUser user = db.Users.Find(id);
          

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                if (UserManager.IsInRole(user.Id, "Trader"))
                {
                    ApplicationUserDetailDTO dto = new ApplicationUserDetailDTO();
                    dto.traderId = user.Id;
                    dto.firstName = user.firstName;
                    dto.secondName = user.secondName;
                    // to come here personal details
                    // security details
                    // contact details

                    return Ok(dto);
                }
                ModelState.AddModelError("Unexpected", "The user has not trader role assigned to the account!");
                return BadRequest(ModelState);
            }
            catch (Exception exc)
            {
                // TODO come up with loggin solution here
                string mess = exc.Message;
                ModelState.AddModelError("Unexpected", "An unexpected error has occured during getting the trader!");
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


        // DONE - TEST IT // used when admin creates autho
        // when user registers as Trader the Register method is used
        // GET api/account/PostTrader
        [ResponseType(typeof(ApplicationUser))]
        [Route("PostTrader")]
        public async Task<IHttpActionResult> PostTrader(ApplicationUser passedTrader)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Unexpected", "The data provided is not valid!");
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
                        ModelState.AddModelError("Trader Create Error", "Trader Create Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // add the role
                    IdentityResult resultRole = UserManager.AddToRole(passedTrader.Id, "Trader");
                    if (!resultRole.Succeeded)
                    {
                        foreach (string err in resultRole.Errors) { message += err; }
                        ModelState.AddModelError("Trader Role Error", "Trader Role Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // Add DUMMY PASSWORD for the author                     
                    IdentityResult resultPassword = await UserManager.AddPasswordAsync(passedTrader.Id, "July2015!");
                    if (!resultPassword.Succeeded)
                    {
                        foreach (string err in resultPassword.Errors) { message += err; }
                        ModelState.AddModelError("Trader Password Error", "Trader Password Error: " + message + " Please contact the application administrator.");
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
                        ModelState.AddModelError("Exists", "Trader with the credentials provided already exist!");
                        return BadRequest(ModelState);
                    }
                    // add the role now
                    IdentityResult resultRole = UserManager.AddToRole(exist.Id, "Trader");
                    if (!resultRole.Succeeded)
                    {
                        foreach (string err in resultRole.Errors) { message += err; }
                        ModelState.AddModelError("Trader Role Error", "Trader Role Error: " + message + " Please contact the app admin!");
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
                ModelState.AddModelError("Trader Unexpected Error", "An unexpected error occured during the creation" +
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
                    firstName = passedTrader.firstName,
                    secondName = passedTrader.secondName
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


        // DONE - WORKS
        // GET api/account/DeleteTrader?id=xx
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
                ModelState.AddModelError("Not Found", "The author account can not be found!");
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
                        ModelState.AddModelError("Trader Role Error", "Trader Role Error: " + message + " Please contact the app admin!");
                        return BadRequest(ModelState);
                    }
                    // Bussiness Rule: Remove the password when removing the author role               
                    IdentityResult resultPassword = await UserManager.RemovePasswordAsync(traderId);
                    if (!resultPassword.Succeeded)
                    {
                        foreach (string err in resultPassword.Errors) { message += err; }
                        ModelState.AddModelError("Trader Password Error", "Trader Password Error: " + message + " Please contact the app admin!");
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
                ModelState.AddModelError("Unexpected", "An unexpected error occured during deleting the author account!");
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
