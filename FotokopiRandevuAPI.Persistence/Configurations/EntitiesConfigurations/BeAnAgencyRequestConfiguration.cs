using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Configurations.EntitiesConfigurations
{
    public class BeAnAgencyRequestConfiguration : IEntityTypeConfiguration<BeAnAgencyRequest>
    {
        public void Configure(EntityTypeBuilder<BeAnAgencyRequest> builder)
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
