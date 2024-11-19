using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.SignalR.Hubs
{
    public class OrderHub:Hub
    {
        public async Task OrderAddedMessage(string agencyId,string customerId,string message)
        {
        }
        public async Task OrderUpdatedMessage()
        {

        }
    }
}
