﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.CancelOrder
{
    public class CancelOrderCommandRequest:IRequest<CancelOrderCommandResponse>
    {
        public string OrderCode { get; set; }
    }
}
