using DenemeTakipAPI.Application.Repositories;
using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.AgencyProducts;
using FotokopiRandevuAPI.Domain.Entities.Products;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.ProductRepositories.AgencyProducts
{
    public class AgencyProductWriteRepository : WriteRepository<AgencyProduct>, IAgencyProductWriteRepository
    {
        public AgencyProductWriteRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
