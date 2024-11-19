using FotokopiRandevuAPI.Domain.Entities.Common;
using FotokopiRandevuAPI.Domain.Entities.Files;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Domain.Entities.Products;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Contexts
{
    public class fotokopiRandevuAPIDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public fotokopiRandevuAPIDbContext(DbContextOptions options) : base(options)
        { }
        
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<BeAnAgencyRequest> BeAnAgencyRequests { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<AgencyProduct> AgencyProducts { get; set; }

        public DbSet<CopyFile> CopyFiles { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin>().HasNoKey();
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                //_= data.State switch
                //{
                //    EntityState.Added => data.Entity.CreatedDate=DateTime.UtcNow,
                //    //EntityState.Modified=> data.Entity.UpdatedDate=DateTime.UtcNow,
                //};
                if (data.State == EntityState.Added)
                {
                    data.Entity.CreatedDate = DateTime.UtcNow;
                }

            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
