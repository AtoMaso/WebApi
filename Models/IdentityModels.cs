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
        // added properties
        public string Name { get; set; }
        public string AtoUsername { get; set; }
        public string Manager { get; set; }
        public string Workpoint { get; set; }
        public int LevelId { get; set; }
        //public virtual Level Level { get; set;  }
        public int PositionId { get; set; }
        //public virtual Position Position { get; set; }
        public int LocalityId { get; set; }
        //public virtual Locality Locality { get; set; }
        public int TeamId { get; set; }
       //public virtual Team Team { get; set; }


        public ApplicationUser()
        {
              this.Id = Guid.NewGuid().ToString();
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
        public string Id { get; set; }       
        public string Name { get; set; }
        public string AtoUsername { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Manager { get; set; }
        public string LevelTitle { get; set; }
        public string PositionTitle { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }

    }


    public class ApplicationUserDetailDTO
    {
        public string Id { get; set; }      
        public string Name { get; set; }
        public string AtoUsername { get; set; }      
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Workpoint { get; set; }
        public string LevelTitle { get; set; }
        public string PositionTitle { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string Manager { get; set; }
        public string LocalityNumber { get; set; }
        public string LocalityStreet { get; set; }
        public string LocalitySuburb { get; set; }
        public string LocalityCity { get; set; }
        public string LocalityPostcode { get; set; }
        public string LocalityState { get; set; }     
       
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

        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BusinessLine> BusinessLines { get; set; }

        public DbSet<Level> Levels { get; set; }

        public DbSet<Locality> Localities { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<Attachement> Attachements { get; set; }
        // Add additional items here as needed
    }

}