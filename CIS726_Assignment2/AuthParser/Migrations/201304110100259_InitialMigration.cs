namespace CIS726_Assignment2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 100),
                        password = c.String(nullable: false),
                        realName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        rolename = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        roleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.roleID, cascadeDelete: true)
                .Index(t => t.userID)
                .Index(t => t.roleID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserRoles", new[] { "roleID" });
            DropIndex("dbo.UserRoles", new[] { "userID" });
            DropForeignKey("dbo.UserRoles", "roleID", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "userID", "dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
