using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.UserRepositories.AgencyRepositories;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.UserRepositories.BeAnAgencyRepositories
{
    internal class BeAnAgencyRequestWriteRepository : WriteRepository<BeAnAgencyRequest>, IBeAnAgencyRequestWriteRepository
    {
        public BeAnAgencyRequestWriteRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
