using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Models.ModelConfigurations
{
    public class PostCategoriesConfiguration : IEntityTypeConfiguration<PostCategories>
    {
        public void Configure(EntityTypeBuilder<PostCategories> builder)
        {
            builder.HasKey(postcomment => new { postcomment.CategoryId, postcomment.PostId });
        }
    }
}
