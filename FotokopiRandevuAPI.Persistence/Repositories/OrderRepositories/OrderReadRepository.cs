using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.OrderRepositories
{
    public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository
    {
        public OrderReadRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
