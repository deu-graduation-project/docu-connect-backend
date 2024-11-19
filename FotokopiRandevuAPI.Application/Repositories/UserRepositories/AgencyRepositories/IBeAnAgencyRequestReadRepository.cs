using DenemeTakipAPI.Application.Repositories;
using FotokopiRandevuAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Repositories.UserRepositories.AgencyRepositories
{
    public interface IBeAnAgencyRequestReadRepository : IReadRepository<BeAnAgencyRequest>
    {
    }
}
