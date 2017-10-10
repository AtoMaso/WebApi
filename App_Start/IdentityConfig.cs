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
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string,
                                                                            ApplicationUserLogin, ApplicationUserRole,
                                                                            ApplicationUserClaim>(context.Get<ApplicationDbContext>()));

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
  public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>  // DropCreateDatabaseAlways<ApplicationDbContext> //     
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
            const string authorRoleName = "Author";
            const string authorRoleDescription = "Author Role";
            const string memberRoleName = "Member";
            const string memberRoleDescription = "Member Role";          

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
            var authorRole = roleManager.FindByName(authorRoleName);
            if (authorRole == null)
            {
                authorRole = new ApplicationRole(authorRoleName);
                // Set the new custom property:
                authorRole.Description = authorRoleDescription;
                var result = roleManager.Create(authorRole);
            }
            //Create Role Users if it does not exist
            var memberRole = roleManager.FindByName(memberRoleName);
            if (memberRole == null)
            {
                memberRole = new ApplicationRole(memberRoleName);
                // Set the new custom property:
                memberRole.Description = memberRoleDescription;
                var memberRoleresult = roleManager.Create(memberRole);
            }


            #endregion


            #region "levels"

            Level le0 = new Level("NOT ON THE LIST");
            Level le1 = new Level("APS1");
            Level le2 = new Level("APS2");
            Level le3 = new Level("APS3");
            Level le4 = new Level("APS4");
            Level le5 = new Level("APS5");
            Level le6 = new Level("APS6");
            Level le7 = new Level("EL1");
            Level le8 = new Level("EL2.1");
            Level le9 = new Level("EL2.2");
            Level le10 = new Level("SES");
            Level le11 = new Level("Contractor");
            Level le12 = new Level("External");
            Level le13 = new Level("Graduate");
            Level le14 = new Level("Coop");

            db.Levels.Add(le0);
            db.Levels.Add(le1);
            db.Levels.Add(le2);
            db.Levels.Add(le3);
            db.Levels.Add(le4);
            db.Levels.Add(le5);
            db.Levels.Add(le6);
            db.Levels.Add(le7);
            db.Levels.Add(le8);
            db.Levels.Add(le9);
            db.Levels.Add(le10);
            db.Levels.Add(le11);
            db.Levels.Add(le12);
            db.Levels.Add(le13);
            db.Levels.Add(le14);

            #endregion


            #region "positions"

            Position po0 = new Position("NOT ON THE LIST");
            Position po1 = new Position("Business Analyst");
            Position po2 = new Position("Senior Business Analyst");
            Position po3 = new Position("System Analyst");
            Position po4 = new Position("Senior System Analyst");
            Position po5 = new Position("Application Developer");
            Position po6 = new Position("Senior Application Developer");
            Position po7 = new Position("Technical Specialist");
            Position po8 = new Position("Senior Technical Specialist");
            Position po9 = new Position("Technical Team Lead");
            Position po10 = new Position("Team Lead");
            Position po11 = new Position("Project Manager");
            Position po12 = new Position("Project Director");
            Position po13 = new Position("CEO");
            Position po14 = new Position("Technical Architect");
            Position po15 = new Position("Application Architect");
            Position po16 = new Position("Solution Architect");
            Position po17 = new Position("Test Manager");
            Position po18 = new Position("Tester");
            Position po19 = new Position("Senior Tester");
            Position po20 = new Position("Project Officer");
            Position po21 = new Position("Personal Assistent");
            Position po22 = new Position("Senior Personal Assistent");
            Position po23 = new Position("Application Designer");
            Position po24 = new Position("Senior Application Designer");
            Position po25 = new Position("Build Team Lead");
            Position po26 = new Position("Scrum Master");
            Position po27 = new Position("Graduate Manager");
            Position po28 = new Position("Mainframe Developer");
            Position po29 = new Position("Senior Mainframe Developer");

            db.Positions.Add(po0);
            db.Positions.Add(po1);
            db.Positions.Add(po2);
            db.Positions.Add(po3);
            db.Positions.Add(po4);
            db.Positions.Add(po5);
            db.Positions.Add(po6);
            db.Positions.Add(po7);
            db.Positions.Add(po8);
            db.Positions.Add(po9);
            db.Positions.Add(po10);
            db.Positions.Add(po11);
            db.Positions.Add(po12);
            db.Positions.Add(po13);
            db.Positions.Add(po14);
            db.Positions.Add(po15);
            db.Positions.Add(po16);
            db.Positions.Add(po17);
            db.Positions.Add(po18);
            db.Positions.Add(po19);
            db.Positions.Add(po20);
            db.Positions.Add(po21);
            db.Positions.Add(po22);
            db.Positions.Add(po23);
            db.Positions.Add(po24);
            db.Positions.Add(po25);
            db.Positions.Add(po26);
            db.Positions.Add(po27);
            db.Positions.Add(po28);
            db.Positions.Add(po29);

            #endregion


            #region "businesslines"

            // Business Line seed

            BusinessLine bl0 = new BusinessLine("NOT ON THE LIST");
            BusinessLine bl1 = new BusinessLine("Channels & Online");
            BusinessLine bl2 = new BusinessLine("Major Releases");
            BusinessLine bl3 = new BusinessLine("DES");

            db.BusinessLines.Add(bl0);
            db.BusinessLines.Add(bl1);
            db.BusinessLines.Add(bl2);
            db.BusinessLines.Add(bl3);

            #endregion


            #region "localities"           

            Locality local0 = new Locality( "NO DATA", "NOT ON THE LIST", "NO DATA", "NO DATA", "NO DATA", "NO DATA");
            Locality local1 = new Locality("55", "Elizabeth St.", "City Center", "Brisbane", "4000", "QLD");
            Locality local2 = new Locality("140", "Elizabeth St.", "City Center", "Brisbane", "4000", "QLD");
            Locality local3 = new Locality("10", "Logan Road", "UMG", "Brisbane", "4100", "QLD");
            Locality local4 = new Locality("10", "Gimpy Road", "Chermside", "Brisbane", "4150", "QLD");
            Locality local5 = new Locality("22", "Gengy St.", "Canberra City", "Canberra", "2600", "ACT");

            db.Localities.Add(local0);
            db.Localities.Add(local1);
            db.Localities.Add(local2);
            db.Localities.Add(local3);
            db.Localities.Add(local4);
            db.Localities.Add(local5);

            #endregion


            #region "categories"

            // Categories seed
            Category cat0 = new Category(1, "NOT ON THE LIST");
            Category cat1 = new Category(2, "Software Article");
            Category cat2 = new Category(3, "Software Snippet");
            Category cat3 = new Category(4, "General Anouncement");
            Category cat4 = new Category(5, "Business Article");
            Category cat5 = new Category(6, "Business Analysis");
            Category cat6 = new Category(7, "Software Script");
            Category cat7 = new Category(6, "Business Document");
            Category cat8 = new Category(7, "General Gossip");
            Category cat9 = new Category(6, "Techncal Document");
       

            db.Categories.Add(cat0);
            db.Categories.Add(cat1);
            db.Categories.Add(cat2);
            db.Categories.Add(cat3);
            db.Categories.Add(cat4);
            db.Categories.Add(cat5);
            db.Categories.Add(cat6);
            db.Categories.Add(cat7);
            db.Categories.Add(cat8);
            db.Categories.Add(cat0);


            #endregion


            #region "users"

            string[] usernames = { "NOTON.THELIST@ato.gov.au",
                                              "Mirko.Srbinovski@ato.gov.au" , "Peter.Puglisi@ato.gov.au" ,"Trung.Ton@ato.gov.au",
                                              "Anand.Badiani@ato.gov.au" , "Carol.Dolery@ato.gov.au" ,"Ian.Taylor@ato.gov.au",
                                              "Khanh-Hop.Troung@ato.gov.au" , "Paul.Nightingale@ato.gov.au" ,"Shahab.Jafri@ato.gov.au"
                                            };
            string[] passwords = { "Noton60",
                                            "Admin60", "Author60", "User60",
                                            "User60","User60", "User60",
                                            "User60", "User60", "User60" };

            string[] names = {"NOTON THELIST", "Mirko Srbinovski", "Peter Puglisi", "Trung Ton",
                                       "Anand Badiani", "Carol Dollery", "Ian Taylor",
                                        "Khanh-Hop Truong" , "Paul Nightingale", "Shahab Jafri"};                  
              
            string[] ids = new string[10];

            string[] atousernames ={"uuuuu",
                                                "ubqw8", "ucdd8", "ucij5",
                                                "ubh3z", "uchq7", "uahce",
                                                "ua9k7", "uaien", "uahce" };

            string[] managers = { "NOT ON THE LIST",
                                              "Paul Nightingale", "Mirko Srbinovski", "Mirko Srbinovski",
                                              "Ian Taylor", "Ian Taylor", "Paul Nightingale",
                                              "Paul Nightingale", "Shahab Jafri", "Narda Phillips"};

            string[] workpoints = { "99.99",
                                                "4.039", "4.036", "4.035",
                                                "x.xxx", "x.xxx", "x.xxx",
                                                "x.xxx", "x.xxx", "x.xxx"};

            int[] levels = {1,
                                  8,12,6,
                                  5,6,8,
                                  8,9,10 };

            int[] positions = {1,
                                      9,7,7,
                                      4,5,11,
                                      9,12,13};

            int[] localities = {1,
                                       2,2,2,
                                        2,2,2,
                                        6,2,6};

            string[] phones = {"1234567890",
                                        "07 311 99634", "07 311 99972", "07 3xx xxxxx",
                                        "07 3xx xxxxx" ,"07 3xx xxxxx" ,"07 3xx xxxxx" ,
                                        "02 6xx xxxxx" , "07 3xx xxxxx" ,"02 6xx xxxxx"};

            int[] teams = { 1,
                                    2,2,2,
                                    3,3,3,
                                    4,4,4};

            for (int i = 0; i < usernames.Length; i++)
            {
                var user = userManager.FindByName(usernames[i]);
                if (user == null)
                {
                                     
                    user = new ApplicationUser { UserName = usernames[i], Email = usernames[i] };
                    // get the random ids of each user and stored them      
                    ids[i] = user.Id;
                    user.Name = names[i];
                    user.AtoUsername = atousernames[i];
                    user.Manager = managers[i];
                    user.PhoneNumber = phones[i];
                    user.Workpoint = workpoints[i];
                    user.LevelId = levels[i];
                    user.PositionId = positions[i];
                    user.LocalityId = localities[i];
                    user.TeamId = teams[i];
                 

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
                                if (user.Id == ids[1]) {
                                    if (!rolesForUser.Contains(adminRole.Name))
                                    {
                                        result = userManager.AddToRole(user.Id, adminRole.Name);
                                    }
                                }
                                // put only me , peter and trung in authors
                                if (user.Id == ids[1] || user.Id == ids[2]  || user.Id == ids[3] )
                                {
                                    if (!rolesForUser.Contains(authorRole.Name))
                                    {
                                        result = userManager.AddToRole(user.Id, authorRole.Name);
                                    }
                                }
                                // put the rest of them in members role
                                if (!rolesForUser.Contains(memberRole.Name))
                                {
                                    result = userManager.AddToRole(user.Id, memberRole.Name);
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

                        
            #region "arttachements"
            // teammeberseeds

            Attachement att1 = new Attachement("111177e8-977f-4d25-bee2-13e56bc0b429", "http://localhost:5700/uploads/AutoResponder.xml", "First Attachement of the article 1");
            Attachement att2 = new Attachement("111177e8-977f-4d25-bee2-13e56bc0b429", "http://localhost:5700/uploads/text.txt", "Second Attachement of the article 1");
            Attachement att3 = new Attachement("222676e8-977f-4d25-bee2-13e56bc0b429", "http://localhost:5700/uploads/doc1.docx", "First Attachement of the article 2");
            Attachement att4 = new Attachement("3335a6e8-977f-4d25-bee2-13e56bc0b429", "http://localhost:5700/uploads/doc2.docx", "First Attachement of the article 3");
            Attachement att5 = new Attachement("44417734-977f-4345-bee2-13e56bc0b429", "http://localhost:5700/uploads/doc3.docx", "First Attachement of the article 4");
            Attachement att6 = new Attachement("44417734-977f-4345-bee2-13e56bc0b429", "http://localhost:5700/uploads/doc4.docx", "Second Attachement of the article 4");


            db.Attachements.Add(att1);
            db.Attachements.Add(att2);
            db.Attachements.Add(att3);
            db.Attachements.Add(att4);
            db.Attachements.Add(att5);
            db.Attachements.Add(att6);

            #endregion         


            #region articles"    

            Article art1 = new Article("111177e8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 1 Mirko.",
                   "This is the flash information about the article 1, flash information regarding the article.",
                   "This is the content of article 1.",
                    new DateTime(2014,11,1), 2, ids[1]);

            Article art2 = new Article("222676e8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 2 Dana .",
                    "This is the flash information about the article 1, flash information regarding the article.",
                     "This is the content of article 2.",
                     new DateTime(2014, 11, 1), 2, ids[2]);

            Article art3 = new Article("3335a6e8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 3 Nenad.",
                "This is the flash information about the article 3, flash information regarding the article.",
                 "This is the content of article 3.",
                  new DateTime(2014, 11, 1), 3, ids[3]);

            Article art4 = new Article("44417734-977f-4345-bee2-13e56bc0b429", "A Title of the article 4 Monika.",
                    "This is the flash information about the article 4, flash information regarding the article.",
                     "This is the content of article 4.",
                     new DateTime(2014, 11, 1), 4, ids[1]);

            Article art5 = new Article("5555a6e8-977f-4d25-bee2-13e56bqqq429","A Title of the article 5 Mirko.",
                "This is the flash information about the article 5, flash information regarding the article.",
                 "This is the content of article 5.",
                 new DateTime(2014, 11, 1), 5, ids[2]);

            Article art6 = new Article("666wwwe8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 6 Dana.",
                    "This is the flash information about the article 6, flash information regarding the article.",
                     "This is the content of article 6.",
                     new DateTime(2014, 11, 1), 6, ids[3]);

            Article art7 = new Article("7775a6e8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 7 Nenad.",
                "This is the flash information about the article 7, flash information regarding the article.",
                 "This is the content of article 7.",
                 new DateTime(2014, 11, 1), 2, ids[1]);

            Article art8 = new Article("8885a6e8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 8 Monika.",
                    "This is the flash information about the article 8, flash information regarding the article.",
                     "This is the content of article 8.",
                     new DateTime(2014, 11, 1), 2, ids[2]);

            Article art9 = new Article("9995a6e8-977f-4d25-bee2-13e56bc0b429","A Title of the article 9 Mirko.",
                "This is the flash information about the article 9, flash information regarding the article.",
                 "This is the content of article 9.",
                 new DateTime(2014, 11, 1), 4, ids[3]);

            Article art10 = new Article("101045a6e8-977f-4d25-bee2-13e56bc0b429", "A Title of the article 10 Dana.",
                    "This is the flash information about the article 10, flash information regarding the article.",
                     "This is the content of article 10.",
                      new DateTime(2014, 11, 1), 3, ids[1]);

            Article art11 = new Article("111145a6e8-977f-4d25-bee2-13e56bc0b429","A Title of the article 11 Nenad.",
                "This is the flash information about the article 11, flash information regarding the article.",
                 "This is the content of article 11.",
                  new DateTime(2014, 11, 1), 4, ids[2]);

            Article art12 = new Article("1212a6e8 - 977f - 4d25 - bee2 - 13e56bc0b429","A Title of the article 12 Monika.",
                    "This is the flash information about the article 12, flash information regarding the article.",
                     "This is the content of article 12.",
                      new DateTime(2014, 11, 1), 2, ids[3]);

            // seed for Articles
            db.Articles.Add(art1);
            db.Articles.Add(art2);
            db.Articles.Add(art3);
            db.Articles.Add(art4);
            db.Articles.Add(art5);
            db.Articles.Add(art6);
            db.Articles.Add(art7);
            db.Articles.Add(art8);
            db.Articles.Add(art9);
            db.Articles.Add(art10);
            db.Articles.Add(art11);
            db.Articles.Add(art12);

            #endregion


            #region "teams"         
            // Team seeds
            Team team1 = new Team(

                  "NOT ON THE LIST",
                  "DEfault team",
                  ids[0],
                  ids[0],
                  ids[0],
                  1,
                  1);
            Team team2 = new Team(
                    
                    "Prefill-Technical",
                    "The Pre-fill Midrange team develops and support all pre-filling web services " +
                    " and authentication web services interfaces used by multilple consuming applications.",
                    ids[9],
                    ids[8],
                    ids[1],
                    2,
                    2);
            Team team3 = new Team
               (
                   
                   "Prefill-Business",
                   "The Pre-fill Business team is provides system analysis and testing support for pre-filling web services " +
                    " and authentication web services used by multilple consuming applications.",
                    ids[9],
                    ids[8],
                    ids[6],
                      2,
                      2
                );
            Team team4 = new Team
            (
                
                 "Prefill-Mainframe",
                "Pre-fill mainframe team is develops and support all pre-filling web services " +
                " mainframe interfaces used by multilple consuming applications.",
                  ids[9],
                  ids[8],
                  ids[7],
                5,
                2
            );

            db.Teams.Add(team1);
            db.Teams.Add(team2);
            db.Teams.Add(team3);
            db.Teams.Add(team4);


            #endregion


            #region "teammember"
            // teammeberseeds

            TeamMember tm1 = new TeamMember(2 ,ids[1]);
            TeamMember tm2 = new TeamMember(2, ids[2]);
            TeamMember tm3 = new TeamMember(2, ids[3]);
            TeamMember tm4 = new TeamMember(2, ids[8]);
            TeamMember tm5 = new TeamMember(2, ids[9]);

            TeamMember tm6 = new TeamMember(3, ids[4]);
            TeamMember tm12 = new TeamMember(3, ids[5]);
            TeamMember tm13 = new TeamMember(3, ids[6]);
            TeamMember tm7 = new TeamMember(3, ids[8]);
            TeamMember tm8 = new TeamMember(3, ids[9]);


            TeamMember tm9 = new TeamMember(4, ids[7]);
            TeamMember tm10 = new TeamMember(4, ids[8]);
            TeamMember tm11 = new TeamMember(4, ids[9]);

            db.TeamMembers.Add(tm1);
            db.TeamMembers.Add(tm2);
            db.TeamMembers.Add(tm3);
            db.TeamMembers.Add(tm4);
            db.TeamMembers.Add(tm5);
            db.TeamMembers.Add(tm6);
            db.TeamMembers.Add(tm7);
            db.TeamMembers.Add(tm8);
            db.TeamMembers.Add(tm9);
            db.TeamMembers.Add(tm10);
            db.TeamMembers.Add(tm11);
            db.TeamMembers.Add(tm12);
            db.TeamMembers.Add(tm13);

            #endregion



            db.SaveChanges();
        }
    }


  // custom validation of the email doamin extenssion
  public class MyCustomUserValidator : UserValidator<ApplicationUser>
  {

        List<string> _allowedEmailDomains = new List<string> { "ato.gov.au"};

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
