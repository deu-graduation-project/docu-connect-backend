using FotokopiRandevuAPI.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Configurations.EntitiesConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(u => u.OrderCode).IsUnique();
            builder.HasMany(o => o.CopyFiles)
                .WithOne(u => u.Order)
                .HasForeignKey(u => u.OrderId);
        }
    }
}
