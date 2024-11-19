using FotokopiRandevuAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Configurations
{
    public class AppRoleConfig:IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
            new AppRole
            {
                Id = Guid.Parse("a55c5f9f-4f8c-4848-882f-0bcb3ec62171").ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            },
            new AppRole
            {
                Id = Guid.Parse("128f0e53-f259-411a-b4be-e050e48c199e").ToString(),
                Name = "customer",
                NormalizedName = "CUSTOMER",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            },
            new AppRole
            {
                Id = Guid.Parse("82a8f83f-47b0-468b-9ddf-cc450d84f4ea").ToString(),
                Name = "agency",
                NormalizedName = "AGENCY",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            }
            );
        }
    }
}
