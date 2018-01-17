using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

//new
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
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
        //[Required]
        //public int personalDetailsId { get; set; }

        //// Navigation property  
        //public PersonalDetails PersonalDetails { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
              // Add any custom User properties/code here
        }     

        // ** Add authenticationtype as method parameter:
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
              // Note the authenticationType must match the one defined 
              // in CookieAuthenticationOptions.AuthenticationType
              var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
              // Add custom user claims here
              return userIdentity;
        }

    }


    public class ApplicationUserDTO
    {
       
        public string traderId { get; set; }

        [Required]
        public int personalDetailsId { get; set; }

        // Navigation property  
        public PersonalDetails personalDetails { get; set; }
    }


    public class ApplicationUserDetailDTO
    {

        public string traderId { get; set; }

        [Required]
        public int personalDetailsId { get; set; }

        // Navigation property  
        public PersonalDetails personalDetails { get; set; }

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

        public DbSet<Trade> Trades { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Address> Addresses { get; set; }
   
        public DbSet<Image> Images { get; set; }

        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        // Add additional items here as needed
    }

}