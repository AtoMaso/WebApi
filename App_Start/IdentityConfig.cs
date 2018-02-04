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
  public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext> //      DropCreateDatabaseIfModelChanges<ApplicationDbContext> //  
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

            for (int i = 0; i < usernames.Length; i++)
            {
                var user = userManager.FindByName(usernames[i]);
                if (user == null)
                {

                    user = new ApplicationUser { UserName = usernames[i], Email = usernames[i] };
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
            ObjectCategory cat1 = new ObjectCategory(1, "Not on the list");
            ObjectCategory cat2 = new ObjectCategory(2, "Animal");
            ObjectCategory cat3 = new ObjectCategory(3, "Clothing");
            ObjectCategory cat4 = new ObjectCategory(4, "Electronics");
            ObjectCategory cat5 = new ObjectCategory(5, "Furniture");
            ObjectCategory cat6 = new ObjectCategory(6, "Real Estate");
            ObjectCategory cat7 = new ObjectCategory(7, "Manchester");
            ObjectCategory cat8 = new ObjectCategory(8, "Sport accessory");
            ObjectCategory cat9 = new ObjectCategory(9, "Toy");
            ObjectCategory cat10 = new ObjectCategory(10, "Tool");
            ObjectCategory cat11 = new ObjectCategory(11, "Vehicle");
            

            List<ObjectCategory> categories = new List<ObjectCategory>();

            categories.Add(cat1);
            categories.Add(cat2);
            categories.Add(cat3);
            categories.Add(cat4);
            categories.Add(cat5);
            categories.Add(cat6);
            categories.Add(cat7);
            categories.Add(cat8);
            categories.Add(cat9);
            categories.Add(cat10);
            categories.Add(cat11);

            db.ObjectCategories.Add(cat1);
            db.ObjectCategories.Add(cat2);
            db.ObjectCategories.Add(cat3);
            db.ObjectCategories.Add(cat4);
            db.ObjectCategories.Add(cat5);
            db.ObjectCategories.Add(cat6);
            db.ObjectCategories.Add(cat7);
            db.ObjectCategories.Add(cat8);
            db.ObjectCategories.Add(cat9);
            db.ObjectCategories.Add(cat10);
            db.ObjectCategories.Add(cat11);

            #endregion



            #region "trades"    

            Trade trade1 = new Trade(1, new DateTime(2015, 11, 1), "Open", ids[0]);
            Trade trade2 = new Trade(2, new DateTime(2014, 11, 1),"Open", ids[2]);
            Trade trade3 = new Trade(3, new DateTime(2014, 11, 1), "Open", ids[3]);
            Trade trade4 = new Trade(4, new DateTime(2015, 11, 11), "Open", ids[1]);
            Trade trade5 = new Trade(5, new DateTime(2014, 10, 1), "Open",  ids[0]);
            Trade trade6 = new Trade(6, new DateTime(2017, 08, 11), "Open", ids[2]);
            Trade trade7 = new Trade(7, new DateTime(2014, 11, 1), "Open", ids[3]);
            Trade trade8 = new Trade(8, new DateTime(2018, 01, 1),"Open", ids[1]);

            // seed for trades
            db.Trades.Add(trade1);
            db.Trades.Add(trade2);
            db.Trades.Add(trade3);
            db.Trades.Add(trade4);
            db.Trades.Add(trade5);
            db.Trades.Add(trade6);
            db.Trades.Add(trade7);
            db.Trades.Add(trade8);

            #endregion


            #region "tradeobjects"

                                                            // (tradeObjectId, tradeObjectDescription, tradeObjectCategoryId, tradeId) TODO check all records
            TradeObject tro1 = new TradeObject(1,"Snake"     ,2   , 1);  
            TradeObject tro2 = new TradeObject(2,"Car"         ,11 , 2);
            TradeObject tro3 = new TradeObject(3, "Dress"     , 3  , 3);
            TradeObject tro4 = new TradeObject(4, "Shoes"    , 3   , 4);
            TradeObject tro5 = new TradeObject(5, "TV"         , 4   , 5);
            TradeObject tro6 = new TradeObject(6, "Phone"    , 4   , 6);
            TradeObject tro7 = new TradeObject(7, "Blanket"  , 7   , 7);
            TradeObject tro8 = new TradeObject(8, "Sheet"    , 7    , 8);
         
            db.TradeObjects.Add(tro1);
            db.TradeObjects.Add(tro2);
            db.TradeObjects.Add(tro3);
            db.TradeObjects.Add(tro4);
            db.TradeObjects.Add(tro5);
            db.TradeObjects.Add(tro6);
            db.TradeObjects.Add(tro7);
            db.TradeObjects.Add(tro8);


            #endregion


            #region "tradeforobject"

                                                                      // (tradeForObjectId, tradeForObjectDescription, objectCategoryId, tradeId) TODO check all records
            TradeForObject trfo1 = new TradeForObject(1, "Truck", 11, 1);
            TradeForObject trfo2 = new TradeForObject(2, "Rabbit", 2, 1);
            TradeForObject trfo3 = new TradeForObject(3, "Shoes", 3, 2);
            TradeForObject trfo4 = new TradeForObject(4, "Dress", 3, 2);
            TradeForObject trfo5 = new TradeForObject(5, "Phone", 4, 3);
            TradeForObject trfo6 = new TradeForObject(6, "TV", 4, 3);
            TradeForObject trfo7 = new TradeForObject(7, "Sheets", 7, 4);
            TradeForObject trfo8 = new TradeForObject(8, "Blanket", 7, 4);
            TradeForObject trfo9 = new TradeForObject(9, "Phone", 4, 5);
            TradeForObject trfo10 = new TradeForObject(10, "TV", 4, 6);
            TradeForObject trfo11 = new TradeForObject(11, "Sheets", 7, 7);
            TradeForObject trfo12 = new TradeForObject(12, "Blanket", 7, 8);


            db.TradeForObjects.Add(trfo1);
            db.TradeForObjects.Add(trfo2);
            db.TradeForObjects.Add(trfo3);
            db.TradeForObjects.Add(trfo4);
            db.TradeForObjects.Add(trfo5);
            db.TradeForObjects.Add(trfo6);
            db.TradeForObjects.Add(trfo7);
            db.TradeForObjects.Add(trfo8);
            db.TradeForObjects.Add(trfo9);
            db.TradeForObjects.Add(trfo10);
            db.TradeForObjects.Add(trfo11);
            db.TradeForObjects.Add(trfo12);

            #endregion


            #region "securityquestions"

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


            #region "images"


            Image image1 = new Image(1, 1, "http://localhost:5700/uploads/images/trade1/trade1.1.jpg", "First Image of the article 1");
            Image image2 = new Image(2, 1, "http://localhost:5700/uploads/images/trade1/trade1.2.jpg", "Second Image of the trade 1");
            Image image3 = new Image(3, 1, "http://localhost:5700/uploads/images/trade1/trade1.3.jpg", "Third Image of the trade 1");
            Image image4 = new Image(4, 2, "http://localhost:5700/uploads/images/trade2/trade2.1.jpg", "First Image of the trade 2");
            Image image5 = new Image(5, 2, "http://localhost:5700/uploads/images/trade2/trade2.2.jpg", "Second Image of the trade2");
            Image image6 = new Image(6, 2, "http://localhost:5700/uploads/images/trade2/trade2.3.jpg", "Third Image of the trade 2");

            Image image7 = new Image(7, 3, "http://localhost:5700/uploads/images/trade3/trade3.1.jpg", "First Image of the article 3");
            Image image8 = new Image(8, 3, "http://localhost:5700/uploads/images/trade3/trade3.2.jpg", "Second Image of the trade 3");
            Image image9 = new Image(9, 3, "http://localhost:5700/uploads/images/trade3/trade3.2.jpg", "Third Image of the trade 3");
            Image image10 = new Image(10, 4, "http://localhost:5700/uploads/images/trade4/trade4.1.jpg", "First Image of the trade 4");
            Image image11 = new Image(11, 4, "http://localhost:5700/uploads/images/trade4/trade4.2.jpg", "Second Image of the trade 4");
            Image image12 = new Image(12, 4, "http://localhost:5700/uploads/images/trade4/trade4.3.jpg", "Third Image of the trade 4");

            Image image13 = new Image(13, 5, "http://localhost:5700/uploads/images/trade5/trade5.1.jpg", "First Image of the article 5");
            Image image14 = new Image(14, 5, "http://localhost:5700/uploads/images/trade5/trade5.2.jpg", "Second Image of the trade 5");
            Image image15= new Image(15, 5, "http://localhost:5700/uploads/images/trade5/trade5.3.jpg", "Third Image of the trade 5");
            Image image16 = new Image(16, 6, "http://localhost:5700/uploads/images/trade6/trade6.1.jpg", "First Image of the trade 6");
            Image image17 = new Image(17, 6, "http://localhost:5700/uploads/images/trade6/trade6.2.jpg", "Second Image of the trade 6");
            Image image18 = new Image(18,6, "http://localhost:5700/uploads/images/trade6/trade6.3.jpg", "Third Image of the trade 6");

            Image image19 = new Image(19, 7, "http://localhost:5700/uploads/images/trade7/trade7.1.jpg", "First Image of the article 7");
            Image image20= new Image(20, 7, "http://localhost:5700/uploads/images/trade7/trade7.2.jpg", "Second Image of the trade 7");
            Image image21= new Image(21, 7, "http://localhost:5700/uploads/images/trade7/trade7.3.jpg", "Third Image of the trade 7");
            Image image22 = new Image(22, 8, "http://localhost:5700/uploads/images/trade8/trade8.1.jpg", "First Image of the trade 8");
            Image image23 = new Image(23, 8, "http://localhost:5700/uploads/images/trade8/trade8.2.jpg", "Second Image of the trade 8");
            Image image24 = new Image(24, 8, "http://localhost:5700/uploads/images/trade8/trade8.3.jpg", "Third Image of the trade 8");

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
            db.Images.Add(image13);
            db.Images.Add(image14);
            db.Images.Add(image15);
            db.Images.Add(image16);
            db.Images.Add(image17);
            db.Images.Add(image18);
            db.Images.Add(image19);
            db.Images.Add(image20);
            db.Images.Add(image21);
            db.Images.Add(image22);
            db.Images.Add(image23);
            db.Images.Add(image24);

            #endregion


            #region "personaldetails"

            PersonalDetails pd1 = new PersonalDetails(1, "Mirko", "S", "Srbinovski", new DateTime(1960, 10, 11), ids[0]);
            PersonalDetails pd3 = new PersonalDetails(2, "Dana", "L", "Srbinovska", new DateTime(1991, 03, 13), ids[1]);
            PersonalDetails pd2 = new PersonalDetails(3, "Nenad", "M", "Srbinovski", new DateTime(1965, 03, 03), ids[2]);
            PersonalDetails pd4 = new PersonalDetails(4, "Monika", "M", "Srbinovska", new DateTime(1997, 05, 06), ids[3]);

            db.PersonalDetails.Add(pd1);
            db.PersonalDetails.Add(pd2);
            db.PersonalDetails.Add(pd3);
            db.PersonalDetails.Add(pd4);

            #endregion


            #region "contactdetails"

            ContactDetails contact1 = new ContactDetails(1, ids[0]);
            ContactDetails contact2 = new ContactDetails(2, ids[1]);
            ContactDetails contact3 = new ContactDetails(3, ids[2]);
            ContactDetails contact4 = new ContactDetails(4, ids[3]);

            db.ContactDetails.Add(contact1);
            db.ContactDetails.Add(contact2);
            db.ContactDetails.Add(contact3);
            db.ContactDetails.Add(contact4);

            #endregion


            #region "securitydetails"

            SecurityDetails security1 = new SecurityDetails(1, ids[0]);
            SecurityDetails security2 = new SecurityDetails(2, ids[1]);
            SecurityDetails security3 = new SecurityDetails(3, ids[2]);
            SecurityDetails security4 = new SecurityDetails(4, ids[3]);

            db.SecurityDetails.Add(security1);
            db.SecurityDetails.Add(security2);
            db.SecurityDetails.Add(security3);
            db.SecurityDetails.Add(security4);

            #endregion


            #region "addresstypes"

            AddressType addt1 = new AddressType(1, "Home Address 1");
            AddressType addt2 = new AddressType(2, "Home Address 2");
            AddressType addt3 = new AddressType(3, "Business Address 1");
            AddressType addt4 = new AddressType(4, "Business Address 2");
            AddressType addt5 = new AddressType(5, "Postal Address 1");
            AddressType addt6 = new AddressType(6, "Postal Address 2");

            db.AddressTypes.Add(addt1);
            db.AddressTypes.Add(addt2);
            db.AddressTypes.Add(addt3);
            db.AddressTypes.Add(addt4);
            db.AddressTypes.Add(addt5);
            db.AddressTypes.Add(addt6);

            #endregion


            #region "addresses"           

            Address address1 = new Address(1, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", 1, 1);
            Address address2 = new Address(2, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", 1, 2);
            Address address3 = new Address(3, "28", "McGregor Street", "UMG", "Brisbane", "4100", "QLD", "Australia", 3, 1);
            Address address4 = new Address(4, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", 5, 1);

            db.Addresses.Add(address1);
            db.Addresses.Add(address2);
            db.Addresses.Add(address3);
            db.Addresses.Add(address4);


            #endregion


            #region "securityanswers"

            SecurityAnswer answer1 = new SecurityAnswer(1, 1, "Ilka", 1);
            SecurityAnswer answer2 = new SecurityAnswer(2, 2, "Vardar", 1);
            SecurityAnswer answer3 = new SecurityAnswer(3, 1, "Alena", 2);
            SecurityAnswer answer4 = new SecurityAnswer(4, 2, "Praha", 2);
            SecurityAnswer answer5 = new SecurityAnswer(5, 1, "Dana", 3);
            SecurityAnswer answer6 = new SecurityAnswer(6, 2, "Broncos", 3);
            SecurityAnswer answer7 = new SecurityAnswer(7, 1, "Dana", 4);
            SecurityAnswer answer8 = new SecurityAnswer(8, 2, "Olimpic", 4);


            db.SecurityAnswers.Add(answer1);
            db.SecurityAnswers.Add(answer2);
            db.SecurityAnswers.Add(answer3);
            db.SecurityAnswers.Add(answer4);
            db.SecurityAnswers.Add(answer5);
            db.SecurityAnswers.Add(answer6);
            db.SecurityAnswers.Add(answer7);
            db.SecurityAnswers.Add(answer8);


            #endregion


            #region "socialnetworkstypes"

            SocialNetworkType socialt1 = new SocialNetworkType(1, "Facebook");
            SocialNetworkType socialt2 = new SocialNetworkType(2, "LinkedIn");
            SocialNetworkType socialt3 = new SocialNetworkType(3, "Twither");
            SocialNetworkType socialt4 = new SocialNetworkType(4, "Instagram");
            SocialNetworkType socialt5 = new SocialNetworkType(5, "Whatever");



            db.SocialNetworkTypes.Add(socialt1);
            db.SocialNetworkTypes.Add(socialt2);
            db.SocialNetworkTypes.Add(socialt3);
            db.SocialNetworkTypes.Add(socialt4);
            db.SocialNetworkTypes.Add(socialt5);

            #endregion


            #region "socialnetworks"

            SocialNetwork social1 = new SocialNetwork(1, "facebookaccount", 1, 1);
            SocialNetwork social2 = new SocialNetwork(2, "linkedinaccount", 2, 1);
            SocialNetwork social3 = new SocialNetwork(3, "twitteraccount", 3, 1);
            SocialNetwork social4 = new SocialNetwork(4, "facebookaccount", 1, 2);
            SocialNetwork social5 = new SocialNetwork(5, "linkedinaccount", 2, 2);
            SocialNetwork social6 = new SocialNetwork(6, "twitteraccount", 3, 2);
            SocialNetwork social7 = new SocialNetwork(7, "facebookaccount", 1, 3);
            SocialNetwork social8 = new SocialNetwork(8, "linkedinaccount", 2, 3);
            SocialNetwork social9 = new SocialNetwork(9, "twitteraccount", 3, 3);
            SocialNetwork social10 = new SocialNetwork(10, "facebookaccount", 1, 4);
            SocialNetwork social11 = new SocialNetwork(11, "linkedinaccount", 2, 4);
            SocialNetwork social12 = new SocialNetwork(12, "twitteraccount", 3, 4);


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


            #region "phonetype"

            PhoneType pht1 = new PhoneType(1, "Home Phone 1");
            PhoneType pht2 = new PhoneType(2, "Home Phone 2");
            PhoneType pht3 = new PhoneType(3, "Business Phone 1");
            PhoneType pht4 = new PhoneType(4, "Business Phone 2");
            PhoneType pht5 = new PhoneType(5, "Mobile Phone 1");
            PhoneType pht6 = new PhoneType(6, "Mobile Phone 2");
            PhoneType pht7 = new PhoneType(7, "Prefered");

            db.PhoneTypes.Add(pht1);
            db.PhoneTypes.Add(pht2);
            db.PhoneTypes.Add(pht3);
            db.PhoneTypes.Add(pht4);
            db.PhoneTypes.Add(pht5);
            db.PhoneTypes.Add(pht6);
            db.PhoneTypes.Add(pht7);

            #endregion


            #region "phones"

            Phone phone1 = new Phone(1, 1, "32761415", "061", "07", 1);
            Phone phone2 = new Phone(2, 2, "31199634", "061", "07", 1);
            Phone phone3 = new Phone(3, 3, "0421949379", "061", "07", 1);
            Phone phone4 = new Phone(4, 1,"32761415", "061", "07", 2);
            Phone phone5 = new Phone(5, 2, "3xxxxxxx", "061", "07", 2);
            Phone phone6 = new Phone(6, 3, "0421xxxxxx", "061", "07", 2);
            Phone phone7 = new Phone(7, 1, "32761415", "061", "07", 3);
            Phone phone8 = new Phone(8, 2, "31199634", "061", "07", 3);
            Phone phone9 = new Phone(9, 3, "0421xxxxxx", "061", "07", 3);
            Phone phone10 = new Phone(10, 1, "32761415", "061", "07", 4);
            Phone phone11 = new Phone(11, 2, "31199634", "061", "07", 4);
            Phone phone12 = new Phone(12, 3, "0421xxxxxx", "061", "07", 4);


            db.Phones.Add(phone1);
            db.Phones.Add(phone2);
            db.Phones.Add(phone3);
            db.Phones.Add(phone4);
            db.Phones.Add(phone5);
            db.Phones.Add(phone6);
            db.Phones.Add(phone7);
            db.Phones.Add(phone8);
            db.Phones.Add(phone9);
            db.Phones.Add(phone10);
            db.Phones.Add(phone11);
            db.Phones.Add(phone12);


            #endregion


            #region "emailtypes"

            EmailType emt1 = new EmailType(1, "Prefered");
            EmailType emt2 = new EmailType(2, "Not Prefered");
            EmailType emt3 = new EmailType(3, "Email 2");
            EmailType emt4 = new EmailType(4, "Email 3");

            db.EmailTypes.Add(emt1);
            db.EmailTypes.Add(emt2);
            db.EmailTypes.Add(emt3);
            db.EmailTypes.Add(emt4);

            #endregion


            #region "emails"

            Email em1 = new Email(1, 1, "srbinovskimirko@gmail.com", 1);
            Email em2 = new Email(2, 2, "srbinovskimirko@icloud.com", 1);

            Email em3 = new Email(3, 1, "srbinovskad@gmail.com", 2);
            Email em4 = new Email(4, 2, "srbinovskad@optusnet.com.au", 2);

            Email em5 = new Email(5, 1, "srbinovskin@gmail.com", 3);
            Email em6 = new Email(6, 2, "srbinovskin@optusnet.com.au", 3);

            Email em7 = new Email(7,1, "srbinovskam@gmail.com", 4);
            Email em8 = new Email(8, 2, "srbinovskam@optusnet.com.au", 4);

            db.Emails.Add(em1);
            db.Emails.Add(em2);
            db.Emails.Add(em3);
            db.Emails.Add(em4);
            db.Emails.Add(em5);
            db.Emails.Add(em6);
            db.Emails.Add(em7);
            db.Emails.Add(em8);

            #endregion


            #region "messagetypes"

            ProcessMessageType pmt1 = new ProcessMessageType(1, "error");
            ProcessMessageType pmt2 = new ProcessMessageType(2, "warning");
            ProcessMessageType pmt3 = new ProcessMessageType(3, "success");

            db.ProcessMessageTypes.Add(pmt1);
            db.ProcessMessageTypes.Add(pmt2);
            db.ProcessMessageTypes.Add(pmt3);

            #endregion


            #region "processmessages"

            //(int id, string msc, int typeId, string meText)
            ProcessMessage pm1 = new ProcessMessage(1, "PM0", 1, "No message!");
            ProcessMessage pm2 = new ProcessMessage(2, "PMGI", 1, "The application could not retrieve the images. Please contact the application administration.");
            //trades
            ProcessMessage pm3 = new ProcessMessage(3, "PMNOTs", 2, "There are no trades associated with that trader.");
            ProcessMessage pm4 = new ProcessMessage(4, "PMEGTs", 1, "The application could not retrieve the trades. Please contact the application administration.");
            ProcessMessage pm5 = new ProcessMessage(5, "PMEGT", 1, "The application could not retrieve the trade. Please contact the application administration.");
            ProcessMessage pm6 = new ProcessMessage(6, "PMEAT", 1, "The application could not add the new trade. Please contact the application administration.");
            ProcessMessage pm7 = new ProcessMessage(7, "PMSAT", 3, "The new trade with the detail below has been added to the apllication store.");
            ProcessMessage pm8 = new ProcessMessage(8, "PMEUT", 1, "The application could not update the trade. Please contact the application administration.");
            ProcessMessage pm9 = new ProcessMessage(9, "PMSUT", 3, "The trade has been updated.");
            ProcessMessage pm10 = new ProcessMessage(10, "PMERT", 1, "The application could not remove the trade. Please contact the application administration.");
            ProcessMessage pm11 = new ProcessMessage(11, "PMSRT", 3, "The trade and all associated images have been removed from the application store!");
            ProcessMessage pm12 = new ProcessMessage(12, "PMEPAT", 2, "You have no privileges to add new trade to the application!");
            ProcessMessage pm13 = new ProcessMessage(13, "PMEPRT", 2, "You have no privileges to remove the trade from the application!");
            ProcessMessage pm14 = new ProcessMessage(14, "PMENTs", 2, "There are no trade in the system!");

            // traders
            ProcessMessage pm15 = new ProcessMessage(15, "PMEGTrs", 1, "The application could not retrieve the traders. Please contact the application administration.");
            ProcessMessage pm16 = new ProcessMessage(16, "PMEGTr", 1, "The application could not retrieve the trader. Please contact the application administration.");
            ProcessMessage pm17 = new ProcessMessage(17, "PMEATr", 1, "The application could not add the new trader. Please contact the application administration.");
            ProcessMessage pm18 = new ProcessMessage(18, "PMSATr", 3, "The new trader has been added to the application.");
            ProcessMessage pm19 = new ProcessMessage(19, "PMEUTr", 1, "The application could not update the trader. Please contact the application administration.");
            ProcessMessage pm20 = new ProcessMessage(20, "PMSUTr", 3, "The trader has been updated.");
            ProcessMessage pm21 = new ProcessMessage(21, "PMERTr", 1, "The application could not remove the trader. Please contact the application administration.");
            ProcessMessage pm22 = new ProcessMessage(22, "PMSRTr", 3, "The trader has been removed from the application!");
            ProcessMessage pm23 = new ProcessMessage(23, "PMEPATr", 2, "You have no privileges to add new trader to the application!");
            ProcessMessage pm24 = new ProcessMessage(24, "PMEPRTr", 2, "You have no privileges to remove the trader from the application!");          
            ProcessMessage pm25 = new ProcessMessage(25, "PMSCTr", 3, "You have successfully created trader account.");
            ProcessMessage pm26 = new ProcessMessage(26, "PMENTrs", 2, "There are not traders in the sustem.");

            // categories
            ProcessMessage pm27 = new ProcessMessage(27, "PMENCs", 2, "There are no categories in the application!");
            ProcessMessage pm28 = new ProcessMessage(28, "PMEGCs", 1, "The application could not retriеve the categories. Please contact the application administration.");
            ProcessMessage pm29 = new ProcessMessage(29, "PMEGC", 1, "The application could not retriеve the category. Please contact the application administration.");
            ProcessMessage pm30 = new ProcessMessage(30, "PMEAC", 1, "The application could not add the new category. Please contact the application administration.");
            ProcessMessage pm31 = new ProcessMessage(31, "PMSAC", 3, "The category with the details below has been added to the application!");
            ProcessMessage pm32 = new ProcessMessage(32, "PMEUC", 1, "The application could not update the category. Please contact the application administration.");
            ProcessMessage pm33 = new ProcessMessage(33, "PMSUC", 3, "The category has been updated!");
            ProcessMessage pm34 = new ProcessMessage(34, "PMERC", 1, "The application could not remove the category. Please contact the application administration.");
            ProcessMessage pm35 = new ProcessMessage(35, "PMSRC", 3, "The category has been removed from the application!");
            ProcessMessage pm36 = new ProcessMessage(36, "PMEPAC", 2, "You have no privileges to add new category to the application!");
            ProcessMessage pm37 = new ProcessMessage(37, "PMEPRC", 2, "You have no privileges to remove the category from the application!");
            ProcessMessage pm38 = new ProcessMessage(38, "PMECE", 2, "Category with the details provided already exist in this application!");

            // login
            ProcessMessage pm39 = new ProcessMessage(39, "PMECA", 1, "The application could not finilise your registeration! Please contact the application administration.");
            ProcessMessage pm40 = new ProcessMessage(40, "PMEPCP", 2, "The password and confirmation password do not match.");         
            ProcessMessage pm41 = new ProcessMessage(41, "PMEEE", 1, "A user with the email provided already exists.");
            ProcessMessage pm42 = new ProcessMessage(42, "PMELOG", 1, "The application could not log you in! Please contact the application administration.");
            ProcessMessage pm43 = new ProcessMessage(43, "PMEPUI", 2, "The user name or password provided are invalid.");
            ProcessMessage pm44 = new ProcessMessage(44, "PMEPI", 2, "The password must have at least one upper character.");         
            ProcessMessage pm45 = new ProcessMessage(45, "PMSLOG", 3, "You have  succesfully logged in.");

            // correspondence 
            ProcessMessage pm46 = new ProcessMessage(46, "PMENCos", 2, "There is no correspondence for your in the system!");
            ProcessMessage pm47 = new ProcessMessage(47, "PMENCo", 2, "The application can not find the correspondence you are looking for!");
            ProcessMessage pm48 = new ProcessMessage(48, "PMEACo", 1, "The application can not add your correspondence!");
            ProcessMessage pm49 = new ProcessMessage(49, "PMERCos", 1, "The application can not delete the correspondence in question!");

            ProcessMessage pm50 = new ProcessMessage(50, "PMEUEO", 1, "Unexpected error has occured. Please contact the application administration!");        


            db.ProcessMessages.Add(pm1);
            db.ProcessMessages.Add(pm2);
            db.ProcessMessages.Add(pm3);
            db.ProcessMessages.Add(pm4);
            db.ProcessMessages.Add(pm5);
            db.ProcessMessages.Add(pm6);
            db.ProcessMessages.Add(pm7);
            db.ProcessMessages.Add(pm8);
            db.ProcessMessages.Add(pm9);
            db.ProcessMessages.Add(pm10);

            db.ProcessMessages.Add(pm11);
            db.ProcessMessages.Add(pm12);
            db.ProcessMessages.Add(pm13);
            db.ProcessMessages.Add(pm14);
            db.ProcessMessages.Add(pm15);
            db.ProcessMessages.Add(pm16);
            db.ProcessMessages.Add(pm17);
            db.ProcessMessages.Add(pm18);
            db.ProcessMessages.Add(pm19);
            db.ProcessMessages.Add(pm20);

            db.ProcessMessages.Add(pm21);
            db.ProcessMessages.Add(pm22);
            db.ProcessMessages.Add(pm23);
            db.ProcessMessages.Add(pm24);
            db.ProcessMessages.Add(pm25);
            db.ProcessMessages.Add(pm26);
            db.ProcessMessages.Add(pm27);
            db.ProcessMessages.Add(pm28);
            db.ProcessMessages.Add(pm29);
            db.ProcessMessages.Add(pm30);

            db.ProcessMessages.Add(pm31);
            db.ProcessMessages.Add(pm32);
            db.ProcessMessages.Add(pm33);
            db.ProcessMessages.Add(pm34);
            db.ProcessMessages.Add(pm35);
            db.ProcessMessages.Add(pm36);
            db.ProcessMessages.Add(pm37);
            db.ProcessMessages.Add(pm38);
            db.ProcessMessages.Add(pm39);
            db.ProcessMessages.Add(pm40);

            db.ProcessMessages.Add(pm41);
            db.ProcessMessages.Add(pm42);
            db.ProcessMessages.Add(pm43);
            db.ProcessMessages.Add(pm44);
            db.ProcessMessages.Add(pm45);
            db.ProcessMessages.Add(pm46);
            db.ProcessMessages.Add(pm47);
            db.ProcessMessages.Add(pm48);
            db.ProcessMessages.Add(pm49);
            db.ProcessMessages.Add(pm50);



            #endregion


            #region "tradehistory"

            TradeHistory trh1 = new TradeHistory(1,1, new DateTime(2015, 11, 1), "Created");
            TradeHistory trh2 = new TradeHistory(2, 1, new DateTime(2015, 11, 10), "Viewed");
            TradeHistory trh3 = new TradeHistory(3, 2, new DateTime(2014, 11, 1), "Created");
            TradeHistory trh4 = new TradeHistory(4, 2, new DateTime(2014, 11, 10), "Viewed");
            TradeHistory trh5 = new TradeHistory(5, 3, new DateTime(2014, 11, 1), "Created");
            TradeHistory trh6 = new TradeHistory(6, 3, new DateTime(2014, 11, 7), "Viewed");
            TradeHistory trh7 = new TradeHistory(7, 4, new DateTime(2015, 11, 11), "Created");
            TradeHistory trh8 = new TradeHistory(8, 4, new DateTime(2015, 11, 20), "Viewed");
            TradeHistory trh9 = new TradeHistory(9, 5, new DateTime(2014, 10, 1), "Created");
            TradeHistory trh10 = new TradeHistory(10, 6, new DateTime(2017, 08, 11), "Created");
            TradeHistory trh11 = new TradeHistory(11, 7, new DateTime(2014, 11, 1), "Created");
            TradeHistory trh12 = new TradeHistory(12, 8, new DateTime(2018, 01, 1), "Created");
            TradeHistory trh13= new TradeHistory(13, 1, new DateTime(2015, 11, 20), "Viewed");
            TradeHistory trh14 = new TradeHistory(14, 1, new DateTime(2015, 12, 1), "Viewed");
            TradeHistory trh15 = new TradeHistory(15, 1, new DateTime(2016, 01, 11), "Viewed");
            TradeHistory trh16 = new TradeHistory(16, 1, new DateTime(2016, 02, 1), "Viewed");
            TradeHistory trh17 = new TradeHistory(17, 1, new DateTime(2016, 03, 1), "Closed");

            db.TradeHistories.Add(trh1);
            db.TradeHistories.Add(trh2);
            db.TradeHistories.Add(trh3);
            db.TradeHistories.Add(trh4);
            db.TradeHistories.Add(trh5);
            db.TradeHistories.Add(trh6);
            db.TradeHistories.Add(trh7);
            db.TradeHistories.Add(trh8);
            db.TradeHistories.Add(trh9);
            db.TradeHistories.Add(trh10);
            db.TradeHistories.Add(trh11);
            db.TradeHistories.Add(trh12);
            db.TradeHistories.Add(trh13);
            db.TradeHistories.Add(trh14);
            db.TradeHistories.Add(trh15);
            db.TradeHistories.Add(trh16);
            db.TradeHistories.Add(trh17);

            #endregion



            #region "correspondence"


            Correspondence cor1 = new Correspondence(1, "A", "What about trading for this", "New", new DateTime(2018, 2, 1), 1, ids[0]);
            Correspondence cor2 = new Correspondence(2, "B", "What about trading for that.", "Red", new DateTime(2018, 2, 1), 2, ids[1]);
            Correspondence cor3 = new Correspondence(3, "C", "What about trading for what", "New", new DateTime(2018, 2, 1), 3, ids[2]);
            Correspondence cor4 = new Correspondence(4, "D", "Happy to tarde for this.", "New", new DateTime(2018, 2, 1), 4, ids[3]);
            Correspondence cor5 = new Correspondence(5, "E", "What about trading for this", "New", new DateTime(2018, 2, 1), 5, ids[0]);
            Correspondence cor6 = new Correspondence(6, "E", "What about trading for this", "Red", new DateTime(2018, 2, 1), 6, ids[1]);
            Correspondence cor7 = new Correspondence(7, "F", "What about trading for this", "New", new DateTime(2018, 2, 1), 7, ids[2]);
            Correspondence cor8 = new Correspondence(8, "G", "Thanks for trading", "New", new DateTime(2018, 2, 1), 8, ids[3]);


            db.Correspondences.Add(cor1);
            db.Correspondences.Add(cor2);
            db.Correspondences.Add(cor3);
            db.Correspondences.Add(cor4);
            db.Correspondences.Add(cor5);
            db.Correspondences.Add(cor6);
            db.Correspondences.Add(cor7);
            db.Correspondences.Add(cor8);

            #endregion


            db.SaveChanges();
        }
    }


  // custom validation of the email doamin extenssion
  public class MyCustomUserValidator : UserValidator<ApplicationUser>
  {

        //List<string> _allowedEmailDomains = new List<string> { "optusnet.com.au"};

        public MyCustomUserValidator(ApplicationUserManager appUserManager): base(appUserManager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
              IdentityResult result = await base.ValidateAsync(user);

              //var emailDomain = user.Email.Split('@')[1];
              //if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
              //{
              //      var errors = (List<string>)result.Errors;

              //      errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

              //      result = new IdentityResult(errors);
              //}
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
