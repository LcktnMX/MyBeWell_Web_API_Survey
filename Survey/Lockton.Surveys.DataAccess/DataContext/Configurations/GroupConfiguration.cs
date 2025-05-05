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
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasMany<User>(F => F.Users).WithOne(D => D.Group).OnDelete(DeleteBehavior.NoAction);            
        }
    }
}
