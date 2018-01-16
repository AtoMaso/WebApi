using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebApi.Models;

// new
using System.Data.Entity;
using System.Web;
using System.Collections.Generic;
using System;

namespace WebApi
{
  public class ApplicationUserManager: UserManager<ApplicationUser, string>
      {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store): base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context.Get<ApplicationDbContext>()));


            //Configure validation logic for usernames
            manager.UserValidator = new MyCustomUserValidator(manager)
            {
              AllowOnlyAlphanumericUserNames = false,
              RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                  RequiredLength = 6,
                  RequireNonLetterOrDigit = false,
                  RequireDigit = true,
                  RequireLowercase = true,
                  RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                  manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
  }



  public class ApplicationRoleManager : RoleManager<ApplicationRole>
  {
    public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore): base(roleStore)
    {
    }

    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    {
      return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
    }
  }


    // TODO THIS IS IMPORTANT The prepopulation of the database is done with the webapiclient app
    // the drop create is used only when we want to recreate complete database
  public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>//   DropCreateDatabaseIfModelChanges<ApplicationDbContext> // 
    {      
        protected override void Seed(ApplicationDbContext context)
        {
            try
            {               
                InitializeIdentityForEF(context);
                base.Seed(context);
            }
            catch( Exception ex)
            {
                string mess = ex.Message;
            }
             
        }


        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
              
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
        


            #region "roles"

            // Some initial values for custom properties:            
            const string adminRoleName = "Admin";
            const string adminRoleDescription = "Admin Role";
            const string traderRoleName = "Trader";
            const string traderRoleDescription = "Trader Role";

            //Create Role Admin if it does not exist
            var adminRole = roleManager.FindByName(adminRoleName);
            if (adminRole == null)
            {
                adminRole = new ApplicationRole(adminRoleName);
                // Set the new custom property:
                adminRole.Description = adminRoleDescription;
                var roleresult = roleManager.Create(adminRole);
            }
            //Create Role Users if it does not exist
            var traderRole = roleManager.FindByName(traderRoleName);
            if (traderRole == null)
            {
                traderRole = new ApplicationRole(traderRoleName);
                // Set the new custom property:
                traderRole.Description = traderRoleDescription;
                var result = roleManager.Create(traderRole);
            }

            #endregion



            #region "users"

            string[] usernames = { "srbinovskim@optusnet.com.au", "srbinovskad@optusnet.com.au", "srbinovskin@optusnet.com.au", "srbinovskam@optusnet.com.au" };
            string[] passwords = {  "Admin1", "Trader1", "Trader2", "Trader3" };
            string[] ids = new string[4];
            int[] pdIds = { 1, 2, 3, 4 };


            for (int i = 0; i < usernames.Length; i++)
            {
                var user = userManager.FindByName(usernames[i]);
                if (user == null)
                {

                    user = new ApplicationUser { UserName = usernames[i], Email = usernames[i] , personalDetailsId = pdIds[i]};
                    // get the random ids of each user and stored them      
                    ids[i] = user.Id;

                    // crate the user      
                    try
                    {
                        var result = userManager.Create(user, passwords[i]);
                        if (result.Succeeded)
                        {
                            result = userManager.SetLockoutEnabled(user.Id, false);
                            if (result.Succeeded)
                            {
                                var rolesForUser = userManager.GetRoles(user.Id);
                                // put me as admin only
                                if (user.Id == ids[0])
                                {
                                    if (!rolesForUser.Contains(adminRole.Name))
                                    {
                                        result = userManager.AddToRole(user.Id, adminRole.Name);
                                    }
                                }
                                // put only me , peter and trung in authors
                                if (user.Id == ids[0] || user.Id == ids[1]  || user.Id == ids[2] || user.Id == ids[3])
                                {
                                    if (!rolesForUser.Contains(traderRole.Name))
                                    {
                                        result = userManager.AddToRole(user.Id, traderRole.Name);
                                    }
                                }                              
                            }
                        }
                        else { break; }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        Exception ex1 = ex.InnerException;
                    }
                }
            }





            #endregion


            #region "categories"

            // Categories seed
            Category cat0 = new Category(1, "NOT ON THE LIST");
            Category cat1 = new Category(2, "TOY");
            Category cat2 = new Category(3, "ELECTRONICS");
            Category cat3 = new Category(4, "FURNITURE");
            Category cat4 = new Category(5, "LAND");
            Category cat5 = new Category(6, "ANIMAL");
            Category cat6 = new Category(7, "KITCHEN");
            Category cat7 = new Category(8, "SPORT");
            Category cat8 = new Category(9, "TOOL");
            Category cat9 = new Category(10, "VEHICLE");

            List<Category> categories = new List<Category>();

            categories.Add(cat0);
            categories.Add(cat1);
            categories.Add(cat2);
            categories.Add(cat3);
            categories.Add(cat4);
            categories.Add(cat5);
            categories.Add(cat6);
            categories.Add(cat7);
            categories.Add(cat8);
            categories.Add(cat9);

            db.Categories.Add(cat0);
            db.Categories.Add(cat1);
            db.Categories.Add(cat2);
            db.Categories.Add(cat3);
            db.Categories.Add(cat4);
            db.Categories.Add(cat5);
            db.Categories.Add(cat6);
            db.Categories.Add(cat7);
            db.Categories.Add(cat8);
            db.Categories.Add(cat9);

            #endregion


            #region "trade"    

            Trade trade1 = new Trade(1, "A CAR for ....", new DateTime(2014, 11, 1), 10, ids[1]);
            Trade trade2 = new Trade(2, "A BIKE for...", new DateTime(2014, 11, 1), 8, ids[2]);
            Trade trade3 = new Trade(3, "A BUS for...", new DateTime(2014, 11, 1), 10, ids[3]);
            Trade trade4 = new Trade(4, "A TABLE for...", new DateTime(2014, 11, 1), 4, ids[1]);

            // seed for trades
            db.Trades.Add(trade1);
            db.Trades.Add(trade2);
            db.Trades.Add(trade3);
            db.Trades.Add(trade4);


            #endregion


            #region "Addresses"           

            Address address1 = new Address(1, "NO DATA", "NOT DATA", "NO DATA", "NO DATA", "NO DATA", "NO DATA", "NO DATA");
            Address local2 = new Address(2, "55", "Elizabeth St.", "City Center", "Brisbane", "4000", "QLD", "Australia");
            Address local3 = new Address(3, "140", "Elizabeth St.", "City Center", "Brisbane", "4000", "QLD", "Australia");
            Address local4 = new Address(4, "10", "Logan Road", "UMG", "Brisbane", "4100", "QLD", "Australia");
            Address local5 = new Address(5, "10", "Gimpy Road", "Chermside", "Brisbane", "4150", "QLD", "Australia");

            db.Addresses.Add(address1);
            db.Addresses.Add(local2);
            db.Addresses.Add(local3);
            db.Addresses.Add(local4);
            db.Addresses.Add(local5);

            #endregion


            #region "images"


            Image image1 = new Image(1, 1, "http://localhost:5700/uploads/AutoResponder.xml", "First Image of the article 1");
            Image image2 = new Image(2, 1, "http://localhost:5700/uploads/text.txt", "Second Image of the trade 1");
            Image image3 = new Image(3, 1, "http://localhost:5700/uploads/doc1.docx", "First Image of the trade 2");
            Image image4 = new Image(4, 2, "http://localhost:5700/uploads/doc2.docx", "First Image of the trade 3");
            Image image5 = new Image(5, 2, "http://localhost:5700/uploads/doc3.docx", "First Image of the trade 4");
            Image image6 = new Image(6, 2, "http://localhost:5700/uploads/doc4.docx", "Second Image of the trade 4");

            Image image7 = new Image(7, 3, "http://localhost:5700/uploads/AutoResponder.xml", "First Image of the article 1");
            Image image8 = new Image(8, 3, "http://localhost:5700/uploads/text.txt", "Second Image of the trade 1");
            Image image9 = new Image(9, 3, "http://localhost:5700/uploads/doc1.docx", "First Image of the trade 2");
            Image image10 = new Image(10, 4, "http://localhost:5700/uploads/doc2.docx", "First Image of the trade 3");
            Image image11 = new Image(11, 4, "http://localhost:5700/uploads/doc3.docx", "First Image of the trade 4");
            Image image12 = new Image(12, 4, "http://localhost:5700/uploads/doc4.docx", "Second Image of the trade 4");



            db.Images.Add(image1);
            db.Images.Add(image2);
            db.Images.Add(image3);
            db.Images.Add(image4);
            db.Images.Add(image5);
            db.Images.Add(image6);
            db.Images.Add(image7);
            db.Images.Add(image8);
            db.Images.Add(image9);
            db.Images.Add(image10);
            db.Images.Add(image11);
            db.Images.Add(image12);

            #endregion


            #region "PersonalDetails"

            PersonalDetails pd1 = new PersonalDetails(1, "Mirko", " ", "Srbinovski", new DateTime(1960, 10, 11));
            PersonalDetails pd3 = new PersonalDetails(2, "Dana", " ", "Srbinovska", new DateTime(1991, 03, 13));
            PersonalDetails pd2 = new PersonalDetails(3, "Nenad", " ", "Srbinovski", new DateTime(1965, 03, 03));          
            PersonalDetails pd4 = new PersonalDetails(4, "Monika", " ", "Srbinovska", new DateTime(1997, 05, 06));
            db.PersonalDetails.Add(pd1);
            db.PersonalDetails.Add(pd2);
            db.PersonalDetails.Add(pd3);
            db.PersonalDetails.Add(pd4);

            #endregion



            db.SaveChanges();
        }
    }


  // custom validation of the email doamin extenssion
  public class MyCustomUserValidator : UserValidator<ApplicationUser>
  {

        List<string> _allowedEmailDomains = new List<string> { "optusnet.com.au"};

        public MyCustomUserValidator(ApplicationUserManager appUserManager): base(appUserManager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
              IdentityResult result = await base.ValidateAsync(user);

              var emailDomain = user.Email.Split('@')[1];
              if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
              {
                    var errors = (List<string>)result.Errors;

                    errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

                    result = new IdentityResult(errors);
              }
              return result;
        }           
  }


  // custom validation of the password which is adding to the current one 
  // which we are inheriting from
  public class MyCustomPasswordValidator : PasswordValidator
  {
      public override async Task<IdentityResult> ValidateAsync(string password)
      {
          IdentityResult result = await base.ValidateAsync(password);

          if (password.Contains("abcdef") || password.Contains("123456"))
          {
              var errors = (List<string>)result.Errors;
              errors.Add("Password can not contain sequence of chars");
              result = new IdentityResult(errors);
          }
          return result;
      }
  }
}
