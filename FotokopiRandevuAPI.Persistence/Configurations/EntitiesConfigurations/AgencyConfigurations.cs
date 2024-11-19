using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Configurations.EntitiesConfigurations
{
    public class AgencyConfigurations : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.OwnsOne(a => a.Address, address =>
            {
                address.Property(a => a.Province).HasColumnName("Province");
                address.Property(a => a.District).HasColumnName("District");
                address.Property(a => a.Extra).HasColumnName("Extra");
            });
        }
    }
}
