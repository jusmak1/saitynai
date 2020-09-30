using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Models.ModelConfigurations
{
    public class RolesConfigurations : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var roleEnums = Enum.GetValues(typeof(ERole));
            foreach(var roleEnum in roleEnums)
            {
                builder.HasData(new Role { Name = roleEnum.ToString() });
            }
        }
    }
}
