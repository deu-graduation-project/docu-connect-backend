using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Hubs
{
    public interface IProductHubService
    {
        Task ProductAddedMessage(string message);
        Task ProductUpdatedMessage(string message);
        Task ProductDeletedMessage(string message);

        Task AgencyProductUpdatedMessage(string message);
        Task AgencyProductDeletedMessage(string message);

    }
}
