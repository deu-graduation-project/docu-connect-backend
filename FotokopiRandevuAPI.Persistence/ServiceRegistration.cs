using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.Repositories.CommentRepositories;
using FotokopiRandevuAPI.Application.Repositories.FileRepositories;
using FotokopiRandevuAPI.Application.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.AgencyProducts;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.Products;
using FotokopiRandevuAPI.Application.Repositories.UserRepositories.AgencyRepositories;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Persistence.Contexts;
using FotokopiRandevuAPI.Persistence.Repositories.CommentRepositories;
using FotokopiRandevuAPI.Persistence.Repositories.FileRepositories;
using FotokopiRandevuAPI.Persistence.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Persistence.Repositories.ProductRepositories.AgencyProducts;
using FotokopiRandevuAPI.Persistence.Repositories.ProductRepositories.Products;
using FotokopiRandevuAPI.Persistence.Repositories.UserRepositories.BeAnAgencyRepositories;
using FotokopiRandevuAPI.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            //services.AddDbContext<fotokopiRandevuAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddDbContext<fotokopiRandevuAPIDbContext>(options => options.UseNpgsql("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=fotokopiRandevuDatabase;"));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<fotokopiRandevuAPIDbContext>()
                .AddDefaultTokenProviders(); 

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<IBeAnAgencyRequestReadRepository, BeAnAgencyRequestReadRepository>();
            services.AddScoped<IBeAnAgencyRequestWriteRepository, BeAnAgencyRequestWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IAgencyProductReadRepository, AgencyProductReadRepository>();
            services.AddScoped<IAgencyProductWriteRepository, AgencyProductWriteRepository>();


            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<ICommentReadRepository, CommentReadRepository>();
            services.AddScoped<ICommentWriteRepository, CommentWriteRepository>();

        }
    }

}
