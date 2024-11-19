using FotokopiRandevuAPI.Domain.Entities.Files;
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
    public class FileConfiguration : IEntityTypeConfiguration<CopyFile>
    {
        public void Configure(EntityTypeBuilder<CopyFile> builder)
        {
            builder.HasIndex(u => u.FileCode).IsUnique();
        }
    }
}
