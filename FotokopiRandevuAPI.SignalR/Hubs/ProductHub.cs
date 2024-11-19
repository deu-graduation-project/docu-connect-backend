using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.SignalR.Hubs
{
    public class ProductHub:Hub
    {
        public async Task ProductAddedMessage(string agencyId, string customerId, string message)
        {
        }
        public async Task ProductUpdatedMessage(string message)
        {

        }
        public async Task ProductDeletedMessage(string message)
        {
        }
    }
}
