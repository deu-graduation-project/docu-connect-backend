using FotokopiRandevuAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Configurations
{
    public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("c5bc8bb5-0f4f-452a-911c-9844f7e2aac7").ToString(),
                RoleId = Guid.Parse("a55c5f9f-4f8c-4848-882f-0bcb3ec62171").ToString()
            });
        }
    }
}
