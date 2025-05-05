using Lockton.Surveys.DataAccess.DataContext.Configurations;
using Lockton.Surveys.DataAccess.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DataContext
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<UserModules> UserModules { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistory { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
            this.Database.SetCommandTimeout(60);
            //#if DEBUG
            //this.Database.Migrate();
            //#endif

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GroupConfiguration());            
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserModulesConfiguration());
            modelBuilder.ApplyConfiguration(new PasswordHistoryConfiguration());
        }
    }
}
