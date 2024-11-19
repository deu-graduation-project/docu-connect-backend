using FotokopiRandevuAPI.Application.Abstraction.Hubs;
using FotokopiRandevuAPI.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IOrderHubService, OrderHubService>();
            serviceCollection.AddTransient<IProductHubService, ProductHubService>();
            serviceCollection.AddTransient<IUserHubService, UserHubService>();


            serviceCollection.AddSignalR();

        }
    }
    
}
