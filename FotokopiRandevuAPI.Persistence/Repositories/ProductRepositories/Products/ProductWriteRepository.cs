using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.Products;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Domain.Entities.Products;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.ProductRepositories.Products
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
