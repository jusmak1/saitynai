using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Models.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(user => user.Posts).WithOne(post => post.Creator).HasForeignKey(post => post.PostedBy);
            builder.HasMany(user => user.Comments).
            WithOne(comment => comment.Creator).HasForeignKey(comment => comment.PostedBy);
        }
    }
}
