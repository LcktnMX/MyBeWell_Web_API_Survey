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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne<Profile>(F => F.Profile).WithMany(D => D.Users).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Group>(F => F.Group).WithMany(D => D.Users).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
