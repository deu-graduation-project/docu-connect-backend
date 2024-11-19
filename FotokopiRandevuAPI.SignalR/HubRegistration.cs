﻿using FotokopiRandevuAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/product-hub");
            webApplication.MapHub<OrderHub>("/order-hub");
            webApplication.MapHub<UserHub>("/user-hub");


        }
    }
}
