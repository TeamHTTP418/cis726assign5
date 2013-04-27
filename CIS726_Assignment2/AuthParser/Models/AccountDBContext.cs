using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AuthParser.Models
{
    public class AccountDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>().HasRequired(p => p.user).WithMany().HasForeignKey(r => r.userID);
            modelBuilder.Entity<UserRoles>().HasRequired(p => p.role).WithMany().HasForeignKey(r => r.roleID);
        }
    }
}