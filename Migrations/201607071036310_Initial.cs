namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {                       
                        Id = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 80),
                        DatePublished = c.DateTime(nullable: false),
                        Flash = c.String(nullable: false, maxLength: 200),
                        CategoryId = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Contents", t => t.ContentId, cascadeDelete: false)
                .Index(t => t.CategoryId)
                .Index(t => t.ContentId)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        AtoUsername = c.String(),
                        Manager = c.String(),
                        Workpoint = c.String(),
                        LevelId = c.Int(nullable: false),
                        PositionId = c.Int(nullable: false),
                        LocalityId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Levels", t => t.LevelId, cascadeDelete: false)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: false)
                .Index(t => t.LevelId)
                .Index(t => t.PositionId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        LevelId = c.Int(nullable: false, identity: true),
                        LevelTitle = c.String(),
                    })
                .PrimaryKey(t => t.LevelId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        PositionTitle = c.String(),
                    })
                .PrimaryKey(t => t.PositionId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        ContentId = c.Int(nullable: false, identity: true),
                        ContentText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContentId);
            
            CreateTable(
                "dbo.BusinessLines",
                c => new
                    {
                        BusinessLineId = c.Int(nullable: false, identity: true),
                        BusinessLineName = c.String(),
                    })
                .PrimaryKey(t => t.BusinessLineId);
            
            CreateTable(
                "dbo.Localities",
                c => new
                    {
                        LocalityId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Street = c.String(nullable: false),
                        Suburb = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Postcode = c.Int(nullable: false),
                        State = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LocalityId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(nullable: false),
                        TeamDescription = c.String(nullable: false, maxLength: 300),
                        TeamLeadId = c.String(),
                        ProjectDirectorId = c.String(),
                        ProjectManagerId = c.String(),
                        BusinessLineId = c.Int(nullable: false),
                        LocalityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("dbo.BusinessLines", t => t.BusinessLineId, cascadeDelete: false)
                .ForeignKey("dbo.Localities", t => t.LocalityId, cascadeDelete: false)
                .Index(t => t.BusinessLineId)
                .Index(t => t.LocalityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "LocalityId", "dbo.Localities");
            DropForeignKey("dbo.Teams", "BusinessLineId", "dbo.BusinessLines");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Articles", "ContentId", "dbo.Contents");
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Articles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Teams", new[] { "LocalityId" });
            DropIndex("dbo.Teams", new[] { "BusinessLineId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "PositionId" });
            DropIndex("dbo.AspNetUsers", new[] { "LevelId" });
            DropIndex("dbo.Articles", new[] { "Id" });
            DropIndex("dbo.Articles", new[] { "ContentId" });
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropTable("dbo.Teams");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Localities");
            DropTable("dbo.BusinessLines");
            DropTable("dbo.Contents");
            DropTable("dbo.Categories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Positions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Levels");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Articles");
        }
    }
}
