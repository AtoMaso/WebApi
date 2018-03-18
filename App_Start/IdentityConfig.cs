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
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, 
                                ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context.Get<ApplicationDbContext>()));

            
            //Configure validation logic for usernames - i think we have covered this in the client
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
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };           
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
  public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> //      DropCreateDatabaseAlways<ApplicationDbContext> //     
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

                    user = new ApplicationUser { UserName = usernames[i], Email = usernames[i], EmailConfirmed = true };
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

            Category cat1 = new Category(1, "Antiques");
            Category cat2 = new Category(2, "Art");
            Category cat3 = new Category(3, "Baby");
            Category cat4 = new Category(4, "Books");
            Category cat5 = new Category(5, "Children");
            Category cat6 = new Category(6, "Clothing");
            Category cat7 = new Category(7, "Collectables");
            Category cat8 = new Category(8, "Computers");
            Category cat9 = new Category(9, "Electronics");
            Category cat10 = new Category(10, "Games");
            Category cat11 = new Category(11, "Jewellery");
            Category cat12 = new Category(12, "Home");
            Category cat13 = new Category(13, "Garden");
            Category cat14 = new Category(14, "Music");
            Category cat15 = new Category(15, "Pets");
            Category cat16 = new Category(16, "Sport");
            Category cat17 = new Category(17, "Tools");
            Category cat18 = new Category(18, "Tickets");           
            Category cat19 = new Category(19, "Vehicles");
            Category cat20 = new Category(20, "Miscellaneous");


            List<Category> categories = new List<Category>();

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
            categories.Add(cat12);
            categories.Add(cat13);
            categories.Add(cat14);
            categories.Add(cat15);
            categories.Add(cat16);
            categories.Add(cat17);
            categories.Add(cat18);
            categories.Add(cat19);
            categories.Add(cat20);
           

            db.Categories.Add(cat1);
            db.Categories.Add(cat2);
            db.Categories.Add(cat3);
            db.Categories.Add(cat4);
            db.Categories.Add(cat5);
            db.Categories.Add(cat6);
            db.Categories.Add(cat7);
            db.Categories.Add(cat8);
            db.Categories.Add(cat9);
            db.Categories.Add(cat10);
            db.Categories.Add(cat11);
            db.Categories.Add(cat12);
            db.Categories.Add(cat13);
            db.Categories.Add(cat14);
            db.Categories.Add(cat15);
            db.Categories.Add(cat16);
            db.Categories.Add(cat17);
            db.Categories.Add(cat18);
            db.Categories.Add(cat19);
            db.Categories.Add(cat20);
           

            #endregion


            #region "subcategories"
            Subcategory sub1 = new Subcategory(1, "Car", 19);
            Subcategory sub2 = new Subcategory(2, "Track", 10);
            Subcategory sub3 = new Subcategory(3, "Caravan", 19);
            Subcategory sub4 = new Subcategory(4, "Faming", 19);
            Subcategory sub5 = new Subcategory(5, "Motorcycle", 19);
            Subcategory sub6 = new Subcategory(6, "Traler", 19);
            Subcategory sub7 = new Subcategory(7, "Other", 19);

            Subcategory sub8 = new Subcategory(8, "Bags", 6);
            Subcategory sub9 = new Subcategory(9, "Woman's shoes", 6);
            Subcategory sub10 = new Subcategory(10, "Men's shoes", 6);
            Subcategory sub11 = new Subcategory(11, "Women's clothing", 6);
            Subcategory sub12 = new Subcategory(12, "Men's cloathing", 6);

            Subcategory sub13 = new Subcategory(13, "Audio", 9);
            Subcategory sub14 = new Subcategory(14, "Cameras", 9);
            Subcategory sub15 = new Subcategory(15, "Phones", 9);
            Subcategory sub16 = new Subcategory(16, "Tablets", 9);
            Subcategory sub17 = new Subcategory(17, "TV/DVD players", 9);

            Subcategory sub18 = new Subcategory(18, "Laptops", 8);
            Subcategory sub19 = new Subcategory(19, "Pcs", 8);
            Subcategory sub20 = new Subcategory(20, "Consoles", 8);
            Subcategory sub21 = new Subcategory(21, "Games", 8);
            Subcategory sub22 = new Subcategory(22, "Software", 8);

            Subcategory sub23 = new Subcategory(23, "Transport", 18);
            Subcategory sub24 = new Subcategory(24, "Concerts", 18);
            Subcategory sub25 = new Subcategory(25, "Sport", 18);
            Subcategory sub26 = new Subcategory(26, "Theatre", 18);
            Subcategory sub27 = new Subcategory(27, "Movies", 18);
            Subcategory sub28 = new Subcategory(28, "Other", 18);

            Subcategory sub29 = new Subcategory(29, "Miscellaneous", 1);
            Subcategory sub30 = new Subcategory(30, "Miscellaneous", 2);
            Subcategory sub31 = new Subcategory(31, "Miscellaneous", 3);
            Subcategory sub32 = new Subcategory(32, "Miscellaneous", 4);
            Subcategory sub33 = new Subcategory(33, "Miscellaneous", 5);
            Subcategory sub34 = new Subcategory(34, "Miscellaneous", 7);
            Subcategory sub35 = new Subcategory(35, "Miscellaneous", 10);
            Subcategory sub36 = new Subcategory(36, "Miscellaneous", 11);
            Subcategory sub37 = new Subcategory(37, "Miscellaneous", 12);
            Subcategory sub38 = new Subcategory(38, "Miscellaneous", 13);
            Subcategory sub39 = new Subcategory(39, "Miscellaneous", 14);
            Subcategory sub40 = new Subcategory(40, "Miscellaneous", 15);
            Subcategory sub41 = new Subcategory(41, "Miscellaneous", 16);
            Subcategory sub42 = new Subcategory(42, "Miscellaneous", 17);
            Subcategory sub43 = new Subcategory(43, "Miscellaneous", 20);    
          
         


            db.Subcategories.Add(sub1);
            db.Subcategories.Add(sub2);
            db.Subcategories.Add(sub3);
            db.Subcategories.Add(sub4);
            db.Subcategories.Add(sub5);
            db.Subcategories.Add(sub6);
            db.Subcategories.Add(sub7);
            db.Subcategories.Add(sub8);
            db.Subcategories.Add(sub9);
            db.Subcategories.Add(sub10);
            db.Subcategories.Add(sub11);
            db.Subcategories.Add(sub12);
            db.Subcategories.Add(sub13);
            db.Subcategories.Add(sub14);
            db.Subcategories.Add(sub15);
            db.Subcategories.Add(sub16);
            db.Subcategories.Add(sub17);
            db.Subcategories.Add(sub18);
            db.Subcategories.Add(sub19);
            db.Subcategories.Add(sub20);
            db.Subcategories.Add(sub21);
            db.Subcategories.Add(sub22);
            db.Subcategories.Add(sub23);
            db.Subcategories.Add(sub24);
            db.Subcategories.Add(sub25);
            db.Subcategories.Add(sub26);
            db.Subcategories.Add(sub27);
            db.Subcategories.Add(sub28);
            db.Subcategories.Add(sub29);
            db.Subcategories.Add(sub30);
            db.Subcategories.Add(sub31);
            db.Subcategories.Add(sub32);
            db.Subcategories.Add(sub33);
            db.Subcategories.Add(sub34);
            db.Subcategories.Add(sub35);
            db.Subcategories.Add(sub36);
            db.Subcategories.Add(sub37);
            db.Subcategories.Add(sub38);
            db.Subcategories.Add(sub39);
            db.Subcategories.Add(sub40);
            db.Subcategories.Add(sub41);
            db.Subcategories.Add(sub42);
            db.Subcategories.Add(sub43);          
            

            #endregion


            #region "states"

            State st1 = new State(1, "QLD");
            State st2 = new State(2, "NSW");
            State st3 = new State(3, "SA");
            State st4 = new State(4, "WA");
            State st5 = new State(5, "NT");
            State st6 = new State(6, "Victoria");
            State st7 = new State(7, "Tasmania");
            State st8 = new State(8, "ACT");

            db.States.Add(st1);
            db.States.Add(st2);
            db.States.Add(st3);
            db.States.Add(st4);
            db.States.Add(st5);
            db.States.Add(st6);
            db.States.Add(st7);
            db.States.Add(st8);

            #endregion


            #region "places"

            Place pl1 = new Place(1,"Brisbane", 1);
            Place pl2 = new Place(2, "Gympy", 1);
            Place pl3 = new Place(3, "Bundaberg", 1);

            Place pl4 = new Place(4, "Sydney", 2);
            Place pl5 = new Place(5, "Newcastle", 2);
            Place pl6 = new Place(6, "Woolangong", 2);

            Place pl7 = new Place(7, "Adelaide", 3);
            Place pl8 = new Place(8, "Barosa Valey", 3);
            Place pl9 = new Place(9, "Cooper Pidy", 3);

            Place pl10 = new Place(10, "Perth", 4);
            Place pl11 = new Place(11, "Albany", 4);
            Place pl12 = new Place(12, "Broom", 4);

            Place pl13 = new Place(13, "Darwin", 5);
            Place pl14 = new Place(14, "Alice Springs", 5);
            Place pl15 = new Place(15, "Kathrine", 5);

            Place pl16 = new Place(16, "Melbourne", 6);
            Place pl17 = new Place(17, "Bendigo", 6);
            Place pl18 = new Place(18, "Balarat", 6);

            Place pl19 = new Place(19, "Hobart", 7);
            Place pl20 = new Place(20, "Launceston", 7);
            Place pl21 = new Place(21, "Swancea", 7);

            Place pl22 = new Place(22, "Canberra", 8);
            Place pl23 = new Place(23, "Cooms", 8);
            Place pl24 = new Place(24, "Calwell", 8);

            db.Places.Add(pl1);
            db.Places.Add(pl2);
            db.Places.Add(pl3);
            db.Places.Add(pl4);
            db.Places.Add(pl5);
            db.Places.Add(pl6);
            db.Places.Add(pl7);
            db.Places.Add(pl8);
            db.Places.Add(pl9);
            db.Places.Add(pl10);
            db.Places.Add(pl11);
            db.Places.Add(pl12);
            db.Places.Add(pl13);
            db.Places.Add(pl14);
            db.Places.Add(pl15);
            db.Places.Add(pl16);
            db.Places.Add(pl17);
            db.Places.Add(pl18);
            db.Places.Add(pl19);
            db.Places.Add(pl20);
            db.Places.Add(pl21);
            db.Places.Add(pl22);
            db.Places.Add(pl23);
            db.Places.Add(pl24);
            #endregion


            #region "postcode"

            Postcode pc1 = new Postcode(1, "4116", 1); // brisbane
            Postcode pc2 = new Postcode(2, "4117", 1);
            Postcode pc3 = new Postcode(3, "4118", 1);
            Postcode pc4 = new Postcode(4, "4570", 2); // gympy         
            Postcode pc5 = new Postcode(5, "4670", 3); // bundeberg

            Postcode pc6 = new Postcode(6, "2000", 4); // sydney
            Postcode pc7 = new Postcode(7, "2001", 4);
            Postcode pc8 = new Postcode(8, "2002", 4);
            Postcode pc9 = new Postcode(9, "2300", 5); // newcastle         
            Postcode pc10 = new Postcode(10, "2500", 6); // woolangong

            Postcode pc11 = new Postcode(11, "5000", 7); // addelaide
            Postcode pc12 = new Postcode(12, "5001", 7);
            Postcode pc13 = new Postcode(13, "5002", 7);
            Postcode pc14 = new Postcode(14, "5116", 8); // barosa         
            Postcode pc15 = new Postcode(15, "5723", 9); // cooper pidy

            Postcode pc21 = new Postcode(16, "6000", 10); // perth
            Postcode pc22 = new Postcode(17, "6001", 10);
            Postcode pc23 = new Postcode(18, "6002", 10);
            Postcode pc24 = new Postcode(19, "6330", 11); // albany      
            Postcode pc25 = new Postcode(20, "6725", 12); // broom

            Postcode pc16 = new Postcode(21, "0820", 13); // darvin
            Postcode pc17 = new Postcode(22, "0821", 13);
            Postcode pc18 = new Postcode(23, "0822", 13);
            Postcode pc19 = new Postcode(24, "0872", 14); // alice         
            Postcode pc20 = new Postcode(25, "0850", 15); // cathrine

       
            Postcode pc26 = new Postcode(26, "3000", 16); // Melbourne
            Postcode pc27 = new Postcode(27, "3001", 16);
            Postcode pc28 = new Postcode(28, "3002", 16);
            Postcode pc29 = new Postcode(29, "3550", 17); // bendigo         
            Postcode pc30 = new Postcode(30, "3350", 18); // balarat

            Postcode pc31 = new Postcode(31, "7000", 19); // hobart
            Postcode pc32 = new Postcode(32, "7001", 19);
            Postcode pc33 = new Postcode(33, "7002", 19);
            Postcode pc34 = new Postcode(34, "7250", 20); // Launceston         
            Postcode pc35 = new Postcode(35, "7190", 21); // Swancea

            Postcode pc36 = new Postcode(36, "2601", 22); // Canberra
            Postcode pc37 = new Postcode(37, "2602", 22);
            Postcode pc38 = new Postcode(38, "2603", 22);
            Postcode pc39 = new Postcode(39, "2611", 23); // Cooms         
            Postcode pc40 = new Postcode(40, "2905", 24); // Calwell

            db.Postcodes.Add(pc1);
            db.Postcodes.Add(pc2);
            db.Postcodes.Add(pc3);
            db.Postcodes.Add(pc4);
            db.Postcodes.Add(pc5);
            db.Postcodes.Add(pc6);
            db.Postcodes.Add(pc7);
            db.Postcodes.Add(pc8);
            db.Postcodes.Add(pc9);
            db.Postcodes.Add(pc10);


            db.Postcodes.Add(pc11);
            db.Postcodes.Add(pc12);
            db.Postcodes.Add(pc13);
            db.Postcodes.Add(pc14);
            db.Postcodes.Add(pc15);
            db.Postcodes.Add(pc16);
            db.Postcodes.Add(pc17);
            db.Postcodes.Add(pc18);
            db.Postcodes.Add(pc19);
            db.Postcodes.Add(pc20);


            db.Postcodes.Add(pc21);
            db.Postcodes.Add(pc22);
            db.Postcodes.Add(pc23);
            db.Postcodes.Add(pc24);
            db.Postcodes.Add(pc25);
            db.Postcodes.Add(pc26);
            db.Postcodes.Add(pc27);
            db.Postcodes.Add(pc28);
            db.Postcodes.Add(pc29);
            db.Postcodes.Add(pc30);


            db.Postcodes.Add(pc31);
            db.Postcodes.Add(pc32);
            db.Postcodes.Add(pc33);
            db.Postcodes.Add(pc34);
            db.Postcodes.Add(pc35);
            db.Postcodes.Add(pc36);
            db.Postcodes.Add(pc37);
            db.Postcodes.Add(pc38);
            db.Postcodes.Add(pc39);
            db.Postcodes.Add(pc40);
            #endregion


            #region "trades"    

            Trade trade1 = new Trade(1, new DateTime(2015, 11, 1), "Open", ids[0], "Snake","Object Description","Rabbit",   15,40, 1, 1, 1);
            Trade trade2 = new Trade(2, new DateTime(2014, 11, 1),"Open", ids[2],  "Car",  "Object Description", "Track" ,     19, 1, 1, 1, 1);
            Trade trade3 = new Trade(3, new DateTime(2014, 11, 1), "Open", ids[3], "Dress", "Object Description" , "Shoes",  6,11, 1, 1,  2);
            Trade trade4 = new Trade(4, new DateTime(2015, 11, 11), "Open", ids[1], "Shoes", "Object Description",  "Dress", 6, 9 , 1, 2, 4);
            Trade trade5 = new Trade(5, new DateTime(2014, 10, 1), "Open",  ids[0], "TV", "Object Description",  "Phone",      9, 17, 1, 3, 5);
            Trade trade6 = new Trade(6, new DateTime(2017, 08, 11), "Open", ids[2], "Phone", "Object Description" , "TV",     9, 15, 2, 4 , 6);
            Trade trade7 = new Trade(7, new DateTime(2014, 11, 1), "Open", ids[3], "Blanket", "Object Description" , "Sheet", 12, 37, 2, 5, 9);
            Trade trade8 = new Trade(8, new DateTime(2018, 01, 1),"Open", ids[1], "Sheet", "Object Description", "Blanket",   12, 37, 2, 6,  10);

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


            Image image1 = new Image(1, 1, "http://localhost:5700/uploads/images/trade1/trade1_1.jpg", "First Image of the article 1");
            Image image2 = new Image(2, 1, "http://localhost:5700/uploads/images/trade1/trade1_2.jpg", "Second Image of the trade 1");
            Image image3 = new Image(3, 1, "http://localhost:5700/uploads/images/trade1/trade1_3.jpg", "Third Image of the trade 1");
            Image image4 = new Image(4, 2, "http://localhost:5700/uploads/images/trade2/trade2_1.jpg", "First Image of the trade 2");
            Image image5 = new Image(5, 2, "http://localhost:5700/uploads/images/trade2/trade2_2.jpg", "Second Image of the trade2");
            Image image6 = new Image(6, 2, "http://localhost:5700/uploads/images/trade2/trade2_3.jpg", "Third Image of the trade 2");

            Image image7 = new Image(7, 3, "http://localhost:5700/uploads/images/trade3/trade3_1.jpg", "First Image of the article 3");
            Image image8 = new Image(8, 3, "http://localhost:5700/uploads/images/trade3/trade3_2.jpg", "Second Image of the trade 3");
            Image image9 = new Image(9, 3, "http://localhost:5700/uploads/images/trade3/trade3_3.jpg", "Third Image of the trade 3");
            Image image10 = new Image(10, 4, "http://localhost:5700/uploads/images/trade4/trade4_1.jpg", "First Image of the trade 4");
            Image image11 = new Image(11, 4, "http://localhost:5700/uploads/images/trade4/trade4_2.jpg", "Second Image of the trade 4");
            Image image12 = new Image(12, 4, "http://localhost:5700/uploads/images/trade4/trade4_3.jpg", "Third Image of the trade 4");

            Image image13 = new Image(13, 5, "http://localhost:5700/uploads/images/trade5/trade5_1.jpg", "First Image of the article 5");
            Image image14 = new Image(14, 5, "http://localhost:5700/uploads/images/trade5/trade5_2.jpg", "Second Image of the trade 5");
            Image image15= new Image(15, 5, "http://localhost:5700/uploads/images/trade5/trade5_3.jpg", "Third Image of the trade 5");
            Image image16 = new Image(16, 6, "http://localhost:5700/uploads/images/trade6/trade6_1.jpg", "First Image of the trade 6");
            Image image17 = new Image(17, 6, "http://localhost:5700/uploads/images/trade6/trade6_2.jpg", "Second Image of the trade 6");
            Image image18 = new Image(18,6, "http://localhost:5700/uploads/images/trade6/trade6_3.jpg", "Third Image of the trade 6");

            Image image19 = new Image(19, 7, "http://localhost:5700/uploads/images/trade7/trade7_1.jpg", "First Image of the article 7");
            Image image20= new Image(20, 7, "http://localhost:5700/uploads/images/trade7/trade7_2.jpg", "Second Image of the trade 7");
            Image image21= new Image(21, 7, "http://localhost:5700/uploads/images/trade7/trade7_3.jpg", "Third Image of the trade 7");
            Image image22 = new Image(22, 8, "http://localhost:5700/uploads/images/trade8/trade8_1.jpg", "First Image of the trade 8");
            Image image23 = new Image(23, 8, "http://localhost:5700/uploads/images/trade8/trade8_2.jpg", "Second Image of the trade 8");
            Image image24 = new Image(24, 8, "http://localhost:5700/uploads/images/trade8/trade8_3.jpg", "Third Image of the trade 8");

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
            PersonalDetails pd3 = new PersonalDetails(2, "Dana", "L", "Srbinovska", new DateTime(1965, 03, 03), ids[1]);
            PersonalDetails pd2 = new PersonalDetails(3, "Nenad", "M", "Srbinovski", new DateTime(1991, 03, 13), ids[2]);
            PersonalDetails pd4 = new PersonalDetails(4, "Monika", "M", "Srbinovska", new DateTime(1997, 05, 06), ids[3]);

            db.PersonalDetails.Add(pd1);
            db.PersonalDetails.Add(pd2);
            db.PersonalDetails.Add(pd3);
            db.PersonalDetails.Add(pd4);

            #endregion


            #region "securitydetails"

            //SecurityDetails security1 = new SecurityDetails(1, ids[0]);
            //SecurityDetails security2 = new SecurityDetails(2, ids[1]);
            //SecurityDetails security3 = new SecurityDetails(3, ids[2]);
            //SecurityDetails security4 = new SecurityDetails(4, ids[3]);

            //db.SecurityDetails.Add(security1);
            //db.SecurityDetails.Add(security2);
            //db.SecurityDetails.Add(security3);
            //db.SecurityDetails.Add(security4);

            #endregion


            #region "addresstypes"

            AddressType addt1 = new AddressType(1, "Home");        
            AddressType addt2 = new AddressType(2, "Postal");
            AddressType addt3 = new AddressType(3, "Business");
           

            db.AddressTypes.Add(addt1);
            db.AddressTypes.Add(addt2);
            db.AddressTypes.Add(addt3);           

            #endregion


            #region "addresses"           

            Address address1 = new Address(1, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", 1, ids[0], "Yes", "", "");
            Address address2 = new Address(2, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia", 1, ids[1], "No",  "", "");
            Address address3 = new Address(3, "28", "McGregor Street", "UMG", "Brisbane", "4100", "QLD", "Australia",   3, ids[0], "No",  "", "");
            Address address4 = new Address(4, "12", "Hailey Place", "Calamvale", "Brisbane", "4116", "QLD", "Australia",  2,ids[1], "Yes", "", "");

            db.Addresses.Add(address1);
            db.Addresses.Add(address2);
            db.Addresses.Add(address3);
            db.Addresses.Add(address4);
             

            #endregion


            #region "securityanswers"

            SecurityAnswer answer1 = new SecurityAnswer(1, 1, "Ilka", ids[0]);
            SecurityAnswer answer2 = new SecurityAnswer(2, 2, "Vardar", ids[0]);
            SecurityAnswer answer3 = new SecurityAnswer(3, 1, "Alena", ids[1]);
            SecurityAnswer answer4 = new SecurityAnswer(4, 2, "Praha", ids[1]);
            SecurityAnswer answer5 = new SecurityAnswer(5, 1, "Dana", ids[2]);
            SecurityAnswer answer6 = new SecurityAnswer(6, 2, "Broncos", ids[2]);
            SecurityAnswer answer7 = new SecurityAnswer(7, 1, "Dana", ids[3]);
            SecurityAnswer answer8 = new SecurityAnswer(8, 2, "Olimpic", ids[3]);


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
            SocialNetworkType socialt3 = new SocialNetworkType(3, "Twitter");
            SocialNetworkType socialt4 = new SocialNetworkType(4, "Instagram");            



            db.SocialNetworkTypes.Add(socialt1);
            db.SocialNetworkTypes.Add(socialt2);
            db.SocialNetworkTypes.Add(socialt3);
            db.SocialNetworkTypes.Add(socialt4);
           

            #endregion


            #region "socialnetworks"

            SocialNetwork social1 = new SocialNetwork(1, "facebookaccount", 1, ids[0], "Yes");
            SocialNetwork social2 = new SocialNetwork(2, "linkedinaccount", 2, ids[0], "No");
            SocialNetwork social3 = new SocialNetwork(3, "twitteraccount", 3, ids[0],"No");
            SocialNetwork social4 = new SocialNetwork(4, "facebookaccount", 1, ids[1], "Yes");
            SocialNetwork social5 = new SocialNetwork(5, "linkedinaccount", 2, ids[1], "No");
            SocialNetwork social6 = new SocialNetwork(6, "twitteraccount", 3, ids[1], "No");
            SocialNetwork social7 = new SocialNetwork(7, "facebookaccount", 1, ids[2], "Yes");
            SocialNetwork social8 = new SocialNetwork(8, "linkedinaccount", 2, ids[2], "No");
            SocialNetwork social9 = new SocialNetwork(9, "twitteraccount", 3, ids[2],"No");
            SocialNetwork social10 = new SocialNetwork(10, "facebookaccount", 1, ids[3], "Yes");
            SocialNetwork social11 = new SocialNetwork(11, "linkedinaccount", 2, ids[3], "No");
            SocialNetwork social12 = new SocialNetwork(12, "twitteraccount", 3, ids[3], "No");


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

            PhoneType pht1 = new PhoneType(1, "Home");   
            PhoneType pht2 = new PhoneType(2, "Business");         
            PhoneType pht3 = new PhoneType(3, "Mobile");
       

            db.PhoneTypes.Add(pht1);
            db.PhoneTypes.Add(pht2);
            db.PhoneTypes.Add(pht3);
          

            #endregion


            #region "phones"

            Phone phone1 = new Phone(1, 1, "32761415", "061", "07", ids[0], "Yes");
            Phone phone2 = new Phone(2, 2, "31199634", "061", "07", ids[0], "No");
            Phone phone3 = new Phone(3, 3, "0421949379", "061", "07", ids[0],"No");
            Phone phone4 = new Phone(4, 1,"32761415", "061", "07", ids[1],  "Yes");
            Phone phone5 = new Phone(5, 2, "3xxxxxxx", "061", "07", ids[1], "No");
            Phone phone6 = new Phone(6, 3, "0421xxxxxx", "061", "07", ids[1], "No");
            Phone phone7 = new Phone(7, 1, "32761415", "061", "07", ids[2],  "Yes");
            Phone phone8 = new Phone(8, 2, "31199634", "061", "07", ids[2], "No");
            Phone phone9 = new Phone(9, 3, "0421xxxxxx", "061", "07", ids[2], "No");
            Phone phone10 = new Phone(10, 1, "32761415", "061", "07", ids[3],  "Yes");
            Phone phone11 = new Phone(11, 2, "31199634", "061", "07", ids[3], "No");
            Phone phone12 = new Phone(12, 3, "0421xxxxxx", "061", "07", ids[3], "No");


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

            EmailType emt1 = new EmailType(1, "Personal");        
            EmailType emt2 = new EmailType(2, "Business");
            EmailType emt3 = new EmailType(3, "Work");

            db.EmailTypes.Add(emt1);
            db.EmailTypes.Add(emt2);
            db.EmailTypes.Add(emt3);
        

            #endregion


            #region "emails"

            Email em1 = new Email(1, 1, "srbinovskimirko@gmail.com", ids[0], "Yes");
            Email em2 = new Email(2, 2, "srbinovskimirko@icloud.com", ids[0], "No");

            Email em3 = new Email(3, 1, "srbinovskad@gmail.com", ids[1], "Yes");
            Email em4 = new Email(4, 2, "srbinovskad@optusnet.com.au", ids[1], "No");

            Email em5 = new Email(5, 1, "srbinovskin@gmail.com", ids[2], "Yes");
            Email em6 = new Email(6, 2, "srbinovskin@optusnet.com.au", ids[2], "No");

            Email em7 = new Email(7,1, "srbinovskam@gmail.com", ids[3], "Yes");
            Email em8 = new Email(8, 2, "srbinovskam@optusnet.com.au", ids[3], "No");

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
            ProcessMessage pm2 = new ProcessMessage(2, "PMGI", 1, "The application could not retrieve the images. Please contact the application administration!");
            //trades
            ProcessMessage pm3 = new ProcessMessage(3, "PMNOTs", 2, "There are no trades associated with that trader!");
            ProcessMessage pm4 = new ProcessMessage(4, "PMEGTs", 1, "The application could not retrieve the trades. Please contact the application administration!");
            ProcessMessage pm5 = new ProcessMessage(5, "PMEGT", 1, "The application could not retrieve the trade. Please contact the application administration!");
            ProcessMessage pm6 = new ProcessMessage(6, "PMEAT", 1, "The application could not add the new trade. Please contact the application administration!");
            ProcessMessage pm7 = new ProcessMessage(7, "PMSAT", 3, "The new trade with the detail below has been added to the system!");
            ProcessMessage pm8 = new ProcessMessage(8, "PMEUT", 1, "The application could not update the trade. Please contact the application administration!");
            ProcessMessage pm9 = new ProcessMessage(9, "PMSUT", 3, "The trade has been updated!");
            ProcessMessage pm10 = new ProcessMessage(10, "PMEDT", 1, "The application could not remove your trade. Please contact the application administration!");
            ProcessMessage pm11 = new ProcessMessage(11, "PMSDT", 3, "You have successfully deleted your trade!");
            ProcessMessage pm12 = new ProcessMessage(12, "PMEPAT", 2, "You have no privileges to add new trade to the system!");
            ProcessMessage pm13 = new ProcessMessage(13, "PMEPDT", 2, "You have no privileges to delete the trade from the system!");
            ProcessMessage pm14 = new ProcessMessage(14, "PMENTs", 2, "There are no trade in the system!");

            // traders
            ProcessMessage pm15 = new ProcessMessage(15, "PMEGTrs", 1, "The application could not retrieve the traders. Please contact the application administration!");
            ProcessMessage pm16 = new ProcessMessage(16, "PMEGTr", 1, "The application could not retrieve the trader. Please contact the application administration!");
            ProcessMessage pm17 = new ProcessMessage(17, "PMEATr", 1, "The application could not add the new trader. Please contact the application administration!");
            ProcessMessage pm18 = new ProcessMessage(18, "PMSATr", 3, "The new trader has been added to the system!");
            ProcessMessage pm19 = new ProcessMessage(19, "PMEUTr", 1, "The application could not update the trader. Please contact the application administration!");
            ProcessMessage pm20 = new ProcessMessage(20, "PMSUTr", 3, "The trader has been updated!");
            ProcessMessage pm21 = new ProcessMessage(21, "PMEDTr", 1, "The application could not delete the trader. Please contact the application administration!");
            ProcessMessage pm22 = new ProcessMessage(22, "PMSDTr", 3, "The trader has been deleted from the system!");
            ProcessMessage pm23 = new ProcessMessage(23, "PMEPATr", 2, "You have no privileges to add new trader to the system!");
            ProcessMessage pm24 = new ProcessMessage(24, "PMEPDTr", 2, "You have no privileges to delete the trader from the system!");          
            ProcessMessage pm25 = new ProcessMessage(25, "PMSCTr", 3, "You have successfully created trader account!");
            ProcessMessage pm26 = new ProcessMessage(26, "PMENTrs", 2, "There are not traders in the system!");

            // categories
            ProcessMessage pm27 = new ProcessMessage(27, "PMENCs", 2, "There are no categories in the system!");
            ProcessMessage pm28 = new ProcessMessage(28, "PMEGCs", 1, "The application could not retriеve the categories. Please contact the application administration!");
            ProcessMessage pm29 = new ProcessMessage(29, "PMEGC", 1, "The application could not retriеve the category. Please contact the application administration!");
            ProcessMessage pm30 = new ProcessMessage(30, "PMEAC", 1, "The application could not add the new category. Please contact the application administration!");
            ProcessMessage pm31 = new ProcessMessage(31, "PMSAC", 3, "The category with the details below has been added to the application!");
            ProcessMessage pm32 = new ProcessMessage(32, "PMEUC", 1, "The application could not update the category. Please contact the application administration!");
            ProcessMessage pm33 = new ProcessMessage(33, "PMSUC", 3, "The category has been updated!");
            ProcessMessage pm34 = new ProcessMessage(34, "PMEDC", 1, "The application could not delete the category. Please contact the application administration!");
            ProcessMessage pm35 = new ProcessMessage(35, "PMSDC", 3, "The category has been deleted from the system!");
            ProcessMessage pm36 = new ProcessMessage(36, "PMEPAC", 2, "You have no privileges to add new category to the system!");
            ProcessMessage pm37 = new ProcessMessage(37, "PMEPDC", 2, "You have no privileges to delete the category from the system!");
            ProcessMessage pm38 = new ProcessMessage(38, "PMECE", 2, "Category with the details provided already exist in the system!");

            // login
            ProcessMessage pm39 = new ProcessMessage(39, "PMECA", 1, "The application could not finilise your registeration! Please contact the application administration!");
            ProcessMessage pm40 = new ProcessMessage(40, "PMEPCP", 2, "The password and confirmation password do not match!");         
            ProcessMessage pm41 = new ProcessMessage(41, "PMEEE", 1, "A user with the credentials provided already exists!");
            ProcessMessage pm42 = new ProcessMessage(42, "PMELOG", 1, "The application could not log you in! Please contact the application administration!");
            ProcessMessage pm43 = new ProcessMessage(43, "PMEPUI", 2, "The user name or password provided are invalid!");
            ProcessMessage pm44 = new ProcessMessage(44, "PMEPI", 2, "The password must have at least one upper character!");         
            ProcessMessage pm45 = new ProcessMessage(45, "PMSLOG", 3, "You have  succesfully logged in!.");
            ProcessMessage pm46 = new ProcessMessage(46, "PMEANC", 2, "Please check your emails and confirm your account!");

            // correspondence 
            ProcessMessage pm47 = new ProcessMessage(47, "PMENCos", 2, "There is no correspondence for your in the system!");
            ProcessMessage pm48 = new ProcessMessage(48, "PMENCo", 2, "The application can not find the correspondence you are looking for!");
            ProcessMessage pm49 = new ProcessMessage(49, "PMEACo", 1, "The application can not add your correspondence!");
            ProcessMessage pm50 = new ProcessMessage(50, "PMERCos", 1, "The application can not delete the correspondence in question!");

            ProcessMessage pm51 = new ProcessMessage(51, "PMEUEO", 1, "Unexpected error has occured. Please contact the application administration!");
            ProcessMessage pm52 = new ProcessMessage(52, "PMSAAd", 3, "The address has been successfuly added to the system!");
            ProcessMessage pm53 = new ProcessMessage(53, "PMSUAd", 3, "The address has been successfuly updated!");
            ProcessMessage pm54 = new ProcessMessage(54, "PMSAPd", 3, "The personal details have been successfuly added to the system!");
            ProcessMessage pm55 = new ProcessMessage(55, "PMSUPd", 3, "Personal details successfully updated!");
            ProcessMessage pm56 = new ProcessMessage(56, "PMEUAd", 2, "Address details have not been changed!");
            ProcessMessage pm57 = new ProcessMessage(57, "PMEUPd", 2, "Personal details have not been changed!");
            ProcessMessage pm58 = new ProcessMessage(58, "PMEWTN", 2, "Trade id provided is invalid!");
            ProcessMessage pm59 = new ProcessMessage(59, "PMSDPD", 2, "Personal details successfuly deleted!");

            ProcessMessage pm60 = new ProcessMessage(60, "PMEUPh", 2, "Phone account details have not been changed!");
            ProcessMessage pm61 = new ProcessMessage(61, "PMSUPh", 3, "Phone account details successfuly updated!");
            ProcessMessage pm62 = new ProcessMessage(62, "PMSAPh", 3, "New phone account has been successfuly added!");
            ProcessMessage pm63 = new ProcessMessage(63, "PMSDPh", 3, "Phone account has been successfuly deleted!");
            ProcessMessage pm64 = new ProcessMessage(64, "PMEAPh", 1, "Error during adding the new phone account!");
            ProcessMessage pm65 = new ProcessMessage(65, "PMEDPh", 1, "Error during deleting the phone account!");

            ProcessMessage pm66 = new ProcessMessage(66, "PMEUEm", 2, "Email acccount details have not been changed!");
            ProcessMessage pm67 = new ProcessMessage(67, "PMSUEm", 3, "Email account details successfuly updated!");
            ProcessMessage pm68 = new ProcessMessage(68, "PMSAEm", 3, "New email account has been successfuly added!");
            ProcessMessage pm69 = new ProcessMessage(69, "PMSDEm", 3, "Email account has been successfuly deleted!");
            ProcessMessage pm70 = new ProcessMessage(70, "PMEAEm", 1, "Error during adding the new email account!");
            ProcessMessage pm71 = new ProcessMessage(71, "PMEDEm", 1, "Error during deleting the email account!");

            ProcessMessage pm72 = new ProcessMessage(72, "PMEUSo", 2, "Social network account details have not been changed!");
            ProcessMessage pm73 = new ProcessMessage(73, "PMSUSo", 3, "Social network account details successfuly updated!");
            ProcessMessage pm74 = new ProcessMessage(74, "PMSASo", 3, "New social network account has been successfuly added!");
            ProcessMessage pm75 = new ProcessMessage(75, "PMSDSo", 3, "Social network account has been successfuly deleted!");
            ProcessMessage pm76 = new ProcessMessage(76, "PMEASo", 1, "Error during adding the new social account!");
            ProcessMessage pm77 = new ProcessMessage(77, "PMEDSo", 1, "Error during deleting the social account!");
            ProcessMessage pm78 = new ProcessMessage(78, "PMSUPas", 3,"Password successfuly changed!");
            ProcessMessage pm79 = new ProcessMessage(79, "PMEPNM", 2, "Your new password and the confirmation password do not match!!");

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

            db.ProcessMessages.Add(pm51);
            db.ProcessMessages.Add(pm52);
            db.ProcessMessages.Add(pm53);
            db.ProcessMessages.Add(pm54);
            db.ProcessMessages.Add(pm55);
            db.ProcessMessages.Add(pm56);
            db.ProcessMessages.Add(pm57);
            db.ProcessMessages.Add(pm58);
            db.ProcessMessages.Add(pm59);
            db.ProcessMessages.Add(pm60);

            db.ProcessMessages.Add(pm61);
            db.ProcessMessages.Add(pm62);
            db.ProcessMessages.Add(pm63);
            db.ProcessMessages.Add(pm64);
            db.ProcessMessages.Add(pm65);
            db.ProcessMessages.Add(pm66);
            db.ProcessMessages.Add(pm67);
            db.ProcessMessages.Add(pm68);
            db.ProcessMessages.Add(pm69);
            db.ProcessMessages.Add(pm70);

            db.ProcessMessages.Add(pm71);
            db.ProcessMessages.Add(pm72);
            db.ProcessMessages.Add(pm73);
            db.ProcessMessages.Add(pm74);
            db.ProcessMessages.Add(pm75);
            db.ProcessMessages.Add(pm76);
            db.ProcessMessages.Add(pm77);
            db.ProcessMessages.Add(pm78);
            db.ProcessMessages.Add(pm79);
            #endregion


            #region "tradehistory"

            TradeHistory trh1 = new TradeHistory(1,1, new DateTime(2015, 11, 1), "Created", "Owner");
            TradeHistory trh2 = new TradeHistory(2, 1, new DateTime(2015, 11, 10), "Viewed", "Trader");
            TradeHistory trh3 = new TradeHistory(3, 2, new DateTime(2014, 11, 1), "Created","Owner" );
            TradeHistory trh4 = new TradeHistory(4, 2, new DateTime(2014, 11, 10), "Viewed", "External");
            TradeHistory trh5 = new TradeHistory(5, 3, new DateTime(2014, 11, 1), "Created", "Owner");
            TradeHistory trh6 = new TradeHistory(6, 3, new DateTime(2014, 11, 7), "Viewed", "External");
            TradeHistory trh7 = new TradeHistory(7, 4, new DateTime(2015, 11, 11), "Created", "Owner");
            TradeHistory trh8 = new TradeHistory(8, 4, new DateTime(2015, 11, 20), "Viewed", "Trader");
            TradeHistory trh9 = new TradeHistory(9, 5, new DateTime(2014, 10, 1), "Created", "Owner");
            TradeHistory trh10 = new TradeHistory(10, 6, new DateTime(2017, 08, 11), "Created", "Owner" );
            TradeHistory trh11 = new TradeHistory(11, 7, new DateTime(2014, 11, 1), "Created", "Owner");
            TradeHistory trh12 = new TradeHistory(12, 8, new DateTime(2018, 01, 1), "Created", "Owner");
            TradeHistory trh13= new TradeHistory(13, 1, new DateTime(2015, 11, 20), "Viewed", "Trader");
            TradeHistory trh14 = new TradeHistory(14, 1, new DateTime(2015, 12, 1), "Viewed", "External");
            TradeHistory trh15 = new TradeHistory(15, 1, new DateTime(2016, 01, 11), "Viewed", "External");
            TradeHistory trh16 = new TradeHistory(16, 1, new DateTime(2016, 02, 1), "Viewed", "External");
            TradeHistory trh17 = new TradeHistory(17, 1, new DateTime(2016, 03, 1), "Viewed", "Owner");

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


            Correspondence cor1 = new Correspondence(1, "A", "What about trading for this", "New", new DateTime(2018, 2, 1), 1,     ids[3], ids[0], "This will be the detailed content of the message sent");
            Correspondence cor2 = new Correspondence(2, "B", "What about trading for that.", "Read", new DateTime(2018, 2, 1), 2,  ids[2], ids[1], "This will be the detailed content of the message sent");
            Correspondence cor3 = new Correspondence(3, "C", "What about trading for what", "New", new DateTime(2018, 2, 1), 3,   ids[1], ids[2], "This will be the detailed content of the message sent");
            Correspondence cor4 = new Correspondence(4, "D", "Happy to tarde for this.", "New", new DateTime(2018, 2, 1), 4,          ids[0], ids[3], "This will be the detailed content of the message sent");
            Correspondence cor5 = new Correspondence(5, "E", "What about trading for this", "New", new DateTime(2018, 2, 1), 5,     ids[3], ids[0], "This will be the detailed content of the message sent");
            Correspondence cor6 = new Correspondence(6, "E", "What about trading for this", "Read", new DateTime(2018, 2, 1), 6,    ids[2], ids[1], "This will be the detailed content of the message sent");
            Correspondence cor7 = new Correspondence(7, "F", "What about trading for this", "New", new DateTime(2018, 2, 1), 7,     ids[1], ids[2], "This will be the detailed content of the message sent");
            Correspondence cor8 = new Correspondence(8, "G", "Thanks for trading", "New", new DateTime(2018, 2, 1), 8,                 ids[0], ids[3], "This will be the detailed content of the message sent");


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
                // check the email length - this is already done on the client side so we do not need it
                int emaillength = user.Email.Length;
                if (emaillength > 70)
                {
                    var errors = (List<string>)result.Errors;

                    errors.Add("Email can not be longer than 70 characters");

                    result = new IdentityResult(errors);
               }               
              // check the email format - this is already done on the client side so we do not need it
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
