using FotokopiRandevuAPI.Application.Abstraction.Hubs;
using FotokopiRandevuAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        readonly IHubContext<ProductHub> _hubContext;

        public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task AgencyProductUpdatedMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctions.ProductAddedMessage, message);
        }

        public async Task AgencyProductDeletedMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctions.ProductAddedMessage, message);

        }

        public async Task ProductAddedMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctions.ProductAddedMessage, message);
        }

        public async Task ProductDeletedMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctions.ProductDeletedMessage, message);
        }

        public async Task ProductUpdatedMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctions.ProductUpdatedMessage, message);
        }
    }
}
