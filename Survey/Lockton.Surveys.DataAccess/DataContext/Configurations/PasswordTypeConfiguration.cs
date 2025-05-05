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
    public class PasswordTypeConfiguration : IEntityTypeConfiguration<PasswordType>
    {
        public void Configure(EntityTypeBuilder<PasswordType> builder)
        {
            builder.HasMany<Group>(F => F.Groups).WithOne(D => D.PasswordType).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
