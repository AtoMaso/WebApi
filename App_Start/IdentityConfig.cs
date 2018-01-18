using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebApi.Models;
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
  public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>// DropCreateDatabaseIfModelChanges<ApplicationDbContext> 
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
            //int[] pdIds = { 1, 2, 3, 4 };


            for (int i = 0; i < usernames.Length; i++)
            {
                var user = userManager.FindByName(usernames[i]);
                if (user == null)
                {

                    user = new ApplicationUser { UserName = usernames[i], Email = usernames[i] };// , personalDetailsId = pdIds[i]};
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


            #region "security questions"

            SecurityQuestion question1 = new SecurityQuestion(1, "Your mum middle name");
            SecurityQuestion question2 = new SecurityQuestion(2, "Your favourite sports club");
            SecurityQuestion question3 = new SecurityQuestion(3, "Your favourite school subject");
            SecurityQuestion question4 = new SecurityQuestion(4, "Your first car brand");
            SecurityQuestion question5 = new SecurityQuestion(5, "Your first job company");
            SecurityQuestion question6 = new SecurityQuestion(6, "Your favourite sport");
            SecurityQuestion question7 = new SecurityQuestion(7, "Your best friend name");
            SecurityQuestion question8 = new SecurityQuestion(8, "Your pet name");


            db.SecurityQuestions.Add(question1);
            db.SecurityQuestions.Add(question2);
            db.SecurityQuestions.Add(question3);
            db.SecurityQuestions.Add(question4);
            db.SecurityQuestions.Add(question5);
            db.SecurityQuestions.Add(question6);
            db.SecurityQuestions.Add(question7);
            db.SecurityQuestions.Add(question8);

            #endregion


            #region "trades"    

            Trade trade1 = new Trade(1, "A CAR for ....", new DateTime(2014, 11, 1), 10, ids[0]);
            Trade trade2 = new Trade(2, "A BIKE for...", new DateTime(2014, 11, 1), 8, ids[2]);
            Trade trade3 = new Trade(3, "A BUS for...", new DateTime(2014, 11, 1), 10, ids[3]);
            Trade trade4 = new Trade(4, "A TABLE for...", new DateTime(2014, 11, 1), 4, ids[1]);

            // seed for trades
            db.Trades.Add(trade1);
            db.Trades.Add(trade2);
            db.Trades.Add(trade3);
            db.Trades.Add(trade4);


            #endregion


            #region "images"


            Image image1 = new Image(1, 1, "http://localhost:5700/uploads/images/1,jpg", "First Image of the article 1");
            Image image2 = new Image(2, 1, "http://localhost:5700/uploads/images/2.jpg", "Second Image of the trade 1");
            Image image3 = new Image(3, 1, "http://localhost:5700/uploads/images/3,jpg", "Third Image of the trade 1");
            Image image4 = new Image(4, 2, "http://localhost:5700/uploads/images/4.jpg", "First Image of the trade 2");
            Image image5 = new Image(5, 2, "http://localhost:5700/uploads/images/5.jpg", "Second Image of the trade2");
            Image image6 = new Image(6, 2, "http://localhost:5700/uploads/images/6.jpg", "Third Image of the trade 2");

            Image image7 = new Image(7, 3, "http://localhost:5700/uploads/images/7.jpg", "First Image of the article 3");
            Image image8 = new Image(8, 3, "http://localhost:5700/uploads/images/8.jpg", "Second Image of the trade 3");
            Image image9 = new Image(9, 3, "http://localhost:5700/uploads/images/9.jpg", "Third Image of the trade 3");
            Image image10 = new Image(10, 4, "http://localhost:5700/uploads/images/10.jpg", "First Image of the trade 4");
            Image image11 = new Image(11, 4, "http://localhost:5700/uploads/images/11.jpg", "Second Image of the trade 4");
            Image image12 = new Image(12, 4, "http://localhost:5700/uploads/images/12.jpg", "Third Image of the trade 4");



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

            PersonalDetails pd1 = new PersonalDetails(1, "Mirko", " ", "Srbinovski", new DateTime(1960, 10, 11), ids[0]);
            PersonalDetails pd3 = new PersonalDetails(2, "Dana", " ", "Srbinovska", new DateTime(1991, 03, 13), ids[1]);
            PersonalDetails pd2 = new PersonalDetails(3, "Nenad", " ", "Srbinovski", new DateTime(1965, 03, 03), ids[2]);
            PersonalDetails pd4 = new PersonalDetails(4, "Monika", " ", "Srbinovska", new DateTime(1997, 05, 06), ids[3]);

            db.PersonalDetails.Add(pd1);
            db.PersonalDetails.Add(pd2);
            db.PersonalDetails.Add(pd3);
            db.PersonalDetails.Add(pd4);

            #endregion


            #region "Contact Details"

            ContactDetails contact1 = new ContactDetails(1, ids[0]);
            ContactDetails contact2 = new ContactDetails(2, ids[1]);
            ContactDetails contact3 = new ContactDetails(3, ids[2]);
            ContactDetails contact4 = new ContactDetails(4, ids[3]);

            db.ContactDetails.Add(contact1);
            db.ContactDetails.Add(contact2);
            db.ContactDetails.Add(contact3);
            db.ContactDetails.Add(contact4);

            #endregion


            #region "Security Details"

            SecurityDetails security1 = new SecurityDetails(1, ids[0]);
            SecurityDetails security2 = new SecurityDetails(2, ids[1]);
            SecurityDetails security3 = new SecurityDetails(3, ids[2]);
            SecurityDetails security4 = new SecurityDetails(4, ids[3]);

            db.SecurityDetails.Add(security1);
            db.SecurityDetails.Add(security2);
            db.SecurityDetails.Add(security3);
            db.SecurityDetails.Add(security4);

            #endregion


            #region "Addresses"           

            Address address1 = new Address(1, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", "home", 1);
            Address address2 = new Address(2, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", "home", 2);
            Address address3 = new Address(3, "28", "McGregor Street", "UMG", "Brisbane", "4100", "QLD", "Australia", "business", 1);
            Address address4 = new Address(4, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", "postal", 1);

            db.Addresses.Add(address1);
            db.Addresses.Add(address2);
            db.Addresses.Add(address3);
            db.Addresses.Add(address4);


            #endregion


            #region "Security  Answers"

            SecurityAnswer answer1 = new SecurityAnswer(1, "Your mum middle name", "Ilka", 1);
            SecurityAnswer answer2 = new SecurityAnswer(2, "Your favourite club", "Vardar", 1);
            SecurityAnswer answer3 = new SecurityAnswer(3, "Your mum middle name", "Alena", 2);
            SecurityAnswer answer4 = new SecurityAnswer(4, "Your favourite club", "Praha", 2);
            SecurityAnswer answer5 = new SecurityAnswer(5, "Your mum middle name", "Dana", 3);
            SecurityAnswer answer6 = new SecurityAnswer(6, "Your favourite club", "Bromcos", 3);
            SecurityAnswer answer7 = new SecurityAnswer(7, "Your mum middle name", "Dana", 4);
            SecurityAnswer answer8 = new SecurityAnswer(8, "Your favourite club", "Olimpic", 4);


            db.SecurityAnswers.Add(answer1);
            db.SecurityAnswers.Add(answer2);
            db.SecurityAnswers.Add(answer3);
            db.SecurityAnswers.Add(answer4);
            db.SecurityAnswers.Add(answer5);
            db.SecurityAnswers.Add(answer6);
            db.SecurityAnswers.Add(answer7);
            db.SecurityAnswers.Add(answer8);


            #endregion


            #region "Social Network"

            SocialNetwork social1 = new SocialNetwork(1, "Facebook", "http://facebook.com/MirkoSrbinovski", 1);
            SocialNetwork social2 = new SocialNetwork(2, "LinkedIn", "http://LinkedIn.com/MirkoSrbinovski", 1);
            SocialNetwork social3 = new SocialNetwork(3, "Twiter", "#MirkoSrbinovski", 1);
            SocialNetwork social4 = new SocialNetwork(4, "Facebook", "http://facebook.com/MirkoSrbinovski", 2);
            SocialNetwork social5 = new SocialNetwork(5, "LinkedIn", "http://LinkedIn.com/MS", 2);
            SocialNetwork social6 = new SocialNetwork(6, "Twiter", "#twiter", 2);
            SocialNetwork social7 = new SocialNetwork(7, "Facebook", "http://facebook.com/MirkoSrbinovski", 3);
            SocialNetwork social8 = new SocialNetwork(8, "LinkedIn", "http://LinkedIn.com/MirkoSrbinovski", 3);
            SocialNetwork social9 = new SocialNetwork(9, "Twiter", "#MirkoSrbinovski", 3);
            SocialNetwork social10 = new SocialNetwork(10, "Facebook", "http://facebook.com/MirkoSrbinovski", 4);
            SocialNetwork social11 = new SocialNetwork(11, "LinkedIn", "http://LinkedIn.com/MS", 4);
            SocialNetwork social12 = new SocialNetwork(12, "Twiter", "#twiter", 4);


            db.SocialNetworks.Add(social1);
            db.SocialNetworks.Add(social2);
            db.SocialNetworks.Add(social3);
            db.SocialNetworks.Add(social4);
            db.SocialNetworks.Add(social5);
            db.SocialNetworks.Add(social6);
            db.SocialNetworks.Add(social7);
            db.SocialNetworks.Add(social8);
            db.SocialNetworks.Add(social9);
            db.SocialNetworks.Add(social10);
            db.SocialNetworks.Add(social11);
            db.SocialNetworks.Add(social12);

            #endregion


            #region "Phones"

            //Phone phone1 = new Phone(1, "Home", "32761415", "061", "07", 1);
            //Phone phone2 = new Phone(2, "Business", "31199634", "061", "07", 1);
            //Phone phone3 = new Phone(3, "Mobile", "0421949379", "061", "07", 1);
            //Phone phone4 = new Phone(4, "Home", "32761415", "061", "07", 2);
            //Phone phone5 = new Phone(5, "Business", "3xxxxxxx", "061", "07", 2);
            //Phone phone6 = new Phone(6, "Mobile", "0421xxxxxx", "061", "07", 2);
            //Phone phone7 = new Phone(7, "Home", "32761415", "061", "07", 3);
            //Phone phone8 = new Phone(8, "Business", "31199634", "061", "07", 3);
            //Phone phone9 = new Phone(9, "Mobile", "0421xxxxxx", "061", "07", 3);
            //Phone phone10 = new Phone(10, "Home", "32761415", "061", "07", 4);
            //Phone phone11 = new Phone(11, "Business", "31199634", "061", "07", 4);
            //Phone phone12 = new Phone(12, "Mobile", "0421xxxxxx", "061", "07", 4);


            //db.Phones.Add(phone1);
            //db.Phones.Add(phone2);
            //db.Phones.Add(phone3);
            //db.Phones.Add(phone4);
            //db.Phones.Add(phone5);
            //db.Phones.Add(phone6);
            //db.Phones.Add(phone7);
            //db.Phones.Add(phone8);
            //db.Phones.Add(phone9);
            //db.Phones.Add(phone10);
            //db.Phones.Add(phone11);
            //db.Phones.Add(phone12);


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
