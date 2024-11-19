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
    public class OrderHubService:IOrderHubService
    {
        readonly IHubContext<OrderHub> _hubContext;

        public OrderHubService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task OrderAddedMessage(string agencyId, string customerId, string message)
        {
            await _hubContext.Clients.Users(agencyId,customerId).SendAsync(ReceiveFunctions.OrderAddedMessage,message);
        }

        public async Task OrderUpdatedMessage(string agencyId, string customerId, string message)
        {
            await _hubContext.Clients.Users(agencyId, customerId).SendAsync(ReceiveFunctions.OrderUpdatedMessage, message);
        }
    }
}
