using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FotokopiRandevuAPI.Persistence.Contexts;

namespace FotokopiRandevuAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<fotokopiRandevuAPIDbContext>
    {
        public fotokopiRandevuAPIDbContext CreateDbContext(string[] args)
        {

            DbContextOptionsBuilder<fotokopiRandevuAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=fotokopiRandevuDatabase;");
            //dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);

            return new(dbContextOptionsBuilder.Options);
        }
    }
}
