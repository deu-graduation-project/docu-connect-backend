using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Hubs
{
    public interface IUserHubService
    {
        Task BeAnAgencyAddedMessage(string message);
    }
}
