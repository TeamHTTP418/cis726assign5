namespace CIS726_Assignment2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Xml;
    using System.Reflection;
    using WebMatrix.WebData;
    using AuthParser.Models;
    using System.Web.Helpers;



    internal sealed class Configuration : DbMigrationsConfiguration<AuthParser.Models.AccountDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AuthParser.Models.AccountDBContext context)
        {
            context.Users.AddOrUpdate(u => u.ID ,new User(){
                ID = 1,
                username = "admin",
                password = Crypto.HashPassword("admin"),
                realName = "Administrator",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 2,
                username = "advisor",
                password = Crypto.HashPassword("advisor"),
                realName = "Advisor",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 3,
                username = "csUndergrad",
                password = Crypto.HashPassword("csUndergrad"),
                realName = "CS Undergrad",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 4,
                username = "seUndergrad",
                password = Crypto.HashPassword("seUndergrad"),
                realName = "SE Undergrad",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 5,
                username = "isUndergrad",
                password = Crypto.HashPassword("isUndergrad"),
                realName = "IS Undergrad",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 6,
                username = "msGrad",
                password = Crypto.HashPassword("msGrad"),
                realName = "MS Grad",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 7,
                username = "mseGrad",
                password = Crypto.HashPassword("mseGrad"),
                realName = "MSE Grad",
            });
            context.Users.AddOrUpdate(u => u.ID, new User()
            {
                ID = 8,
                username = "phdGrad",
                password = Crypto.HashPassword("phdGrad"),
                realName = "PhD Grad",
            });
            context.Roles.AddOrUpdate(r => r.ID, new Role()
            {
                ID = 1,
                rolename = "Administrator"
            });
            context.Roles.AddOrUpdate(r => r.ID, new Role()
            {
                ID = 2,
                rolename = "Advisor"
            });
            context.SaveChanges();

            context.UserRoles.AddOrUpdate(r => r.ID, new UserRoles()
            {
                ID = 1,
                userID = 1,
                roleID = 1,
            });
            context.UserRoles.AddOrUpdate(r => r.ID, new UserRoles()
            {
                ID = 2,
                userID = 2,
                roleID = 2,
            });
            context.SaveChanges();
        }
    }
}
