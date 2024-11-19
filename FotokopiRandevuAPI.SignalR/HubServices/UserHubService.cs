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
    public class UserHubService : IUserHubService
    {
        readonly IHubContext<UserHub> _hubContext;

        public UserHubService(IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BeAnAgencyAddedMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctions.BeAnAgencyAddedMessage, message);
        }
    }
}
