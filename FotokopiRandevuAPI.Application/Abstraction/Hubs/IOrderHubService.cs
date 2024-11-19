using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Hubs
{
    public interface IOrderHubService
    {

        Task OrderAddedMessage(string agencyId, string customerId,string message);
        Task OrderUpdatedMessage(string agencyId, string customerId,string message);
    }
}
