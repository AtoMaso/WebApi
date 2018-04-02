using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;


namespace WebApi.Models
{
    // project-specific implementations, so here they are:
    public class ApplicationUserLogin : IdentityUserLogin<string> {}
  public class ApplicationUserClaim : IdentityUserClaim<string> {}
  public class ApplicationUserRole : IdentityUserRole<string> {}

  // Must be expressed in terms of our custom Role and other types:
  public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
  {            
     
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
              // Add any custom User properties/code here
        }     

        // ** Add authenticationtype as method parameter:
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
              // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
              var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

              // Add custom user claims here
              return userIdentity;
        }

    }


    public class ApplicationUserListDTO
    {
       // this is going to be used on the list of traders
        public string traderId { get; set; }

        public string email { get; set; }

        public bool emailconfirmed { get; set; }

        public string username { get; set;  }

        public string passwordhash { get; set; }

       
    }


    public class ApplicationUserDetailDTO
    {

        public string traderId { get; set; }

        public PersonalDetailsDTO personalDetails { get; set; }

        //public string traderFirstName { get; set; }

        //public string traderMiddleName { get; set; }

        //public string traderLastName { get; set; }

        //public string traderContactEmail{ get; set; }

        //public string traderContactPhone { get; set; }

        //public string traderContactSocialNetwork { get; set; }

    }


    // Must be expressed in terms of our custom UserRole:
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
  {      
        public ApplicationRole()
        {
              this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name): this()
        {
              this.Name = name;
        }

        // Add any custom Role properties/code here
        // Add Custom Property:
        public string Description { get; set; }
  }

 
  // Most likely won't need to customize these either, but they were needed because we implemented
  // custom versions of all the other types:
  public class ApplicationUserStore: UserStore<ApplicationUser, ApplicationRole, string,  ApplicationUserLogin, ApplicationUserRole,
                                                ApplicationUserClaim>, IUserStore<ApplicationUser, string>, IDisposable
  {
        public ApplicationUserStore(): this(new IdentityDbContext())
        {
              base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context): base(context)
        {
        }
  }


  public class ApplicationRoleStore : RoleStore<ApplicationRole, string, ApplicationUserRole>, IQueryableRoleStore<ApplicationRole, string>,
                                                IRoleStore<ApplicationRole, string>, IDisposable
  {
        public ApplicationRoleStore(): base(new IdentityDbContext())
        {
              base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context) : base(context)
        {
        }
  }


    // Must be expressed in terms of our custom types:
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Trade> Trades { get; set; }

        public System.Data.Entity.DbSet<Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<Image> Images { get; set; }

        public System.Data.Entity.DbSet<PersonalDetails> PersonalDetails { get; set; }

        public System.Data.Entity.DbSet<SocialNetwork> SocialNetworks { get; set; }
         
        public System.Data.Entity.DbSet<Phone> Phones { get; set; }

        public System.Data.Entity.DbSet<SocialNetworkType> SocialNetworkTypes { get; set; }

        public System.Data.Entity.DbSet<PhoneType> PhoneTypes { get; set; }

        public System.Data.Entity.DbSet<AddressType> AddressTypes { get; set; }    

        public System.Data.Entity.DbSet<Category> Categories { get; set; }    

        public System.Data.Entity.DbSet<Email> Emails { get; set; }

        public System.Data.Entity.DbSet<EmailType> EmailTypes { get; set; }

        public System.Data.Entity.DbSet<ProcessMessageType> ProcessMessageTypes { get; set; }

        public System.Data.Entity.DbSet<ProcessMessage> ProcessMessages { get; set; }

        public System.Data.Entity.DbSet<TradeHistory> TradeHistories { get; set; }

        public System.Data.Entity.DbSet<Correspondence> Correspondences { get; set; }

        public System.Data.Entity.DbSet<Place> Places { get; set; }

        public System.Data.Entity.DbSet<State> States { get; set; }

        public System.Data.Entity.DbSet<Subcategory> Subcategories { get; set; }

        public System.Data.Entity.DbSet<Postcode> Postcodes { get; set; }

        public System.Data.Entity.DbSet<Suburb> Suburbs { get; set; }

        public System.Data.Entity.DbSet<WebApi.Models.StatePlacePostcodeSuburb> StatesPlacesPostcodesSuburbs { get; set; }
    }

}