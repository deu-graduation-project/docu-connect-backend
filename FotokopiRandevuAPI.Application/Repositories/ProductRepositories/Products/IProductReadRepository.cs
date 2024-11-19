using DenemeTakipAPI.Application.Repositories;
using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Repositories.ProductRepositories.Products
{
    public interface IProductReadRepository : IReadRepository<Product>
    {
    }
}
