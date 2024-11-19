using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.SignalR
{
    public static class ReceiveFunctions
    {
        public const string ProductAddedMessage = "receiveProductAddedMessage";
        public const string ProductUpdatedMessage = "receiveProductUpdatedMessage";
        public const string ProductDeletedMessage = "receiveProductDeletedMessage";

        public const string OrderAddedMessage = "receiveOrderAddedMessage";
        public const string OrderUpdatedMessage = "receiveOrderUpdatedMessage";

        public const string BeAnAgencyAddedMessage = "receiveBeAnAgencyAddedMessage";

    }
}
