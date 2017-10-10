using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class LoudWhispererDb : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public LoudWhispererDb() : base("name=DefaultConnection")
        {
            Debug.Write(Database.Connection.ConnectionString);
        }

        public System.Data.Entity.DbSet<WebApi.Models.Article> Articles { get; set; }
        public System.Data.Entity.DbSet<WebApi.Models.Team> Teams { get; set; }     
        public System.Data.Entity.DbSet<WebApi.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<WebApi.Models.Content> Contents { get; set; }
        public System.Data.Entity.DbSet<WebApi.Models.Locality> Localities { get; set; }       
        public System.Data.Entity.DbSet<WebApi.Models.BusinessLine> BusinessLines { get; set; }
        public System.Data.Entity.DbSet<WebApi.Models.Level> Levels { get; set; }
        public System.Data.Entity.DbSet<WebApi.Models.Position> Positions { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.Member> Members { get; set; }
        //public System.Data.Entity.DbSet<WebApi.Models.Author> Authors { get; set; }      
        // security
        //public System.Data.Entity.DbSet<WebApi.Models.User> Users { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.Role> Roles { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.UserRole> UserRoles { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.UserLogin> UserLogins { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.Login> Logins { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.Claim> Claims { get; set; }

        //public System.Data.Entity.DbSet<WebApi.Models.UserClaim> UserClaims { get; set; }
    }
}
