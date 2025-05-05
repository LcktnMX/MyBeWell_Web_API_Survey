using Lockton.Surveys.DataAccess.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DataContext.Configurations
{
    public class UserModulesConfiguration : IEntityTypeConfiguration<UserModules>
    {
        public void Configure(EntityTypeBuilder<UserModules> builder)
        {
            builder.HasOne<Module>(F => F.Module).WithMany(D => D.Users).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<User>(F => F.User).WithMany(D => D.Modules).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
