using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Application.Repositories.UserRepositories.AgencyRepositories;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.UserRepositories.BeAnAgencyRepositories
{
    internal class BeAnAgencyRequestReadRepository : ReadRepository<BeAnAgencyRequest>, IBeAnAgencyRequestReadRepository
    {
        public BeAnAgencyRequestReadRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
