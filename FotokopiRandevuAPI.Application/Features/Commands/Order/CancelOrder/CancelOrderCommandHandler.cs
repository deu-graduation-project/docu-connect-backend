using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommandRequest, CancelOrderCommandResponse>
    {
        readonly IOrderService _orderService;

        public CancelOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CancelOrderCommandResponse> Handle(CancelOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _orderService.CancelOrderAsync(request.OrderCode);
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };
        }
    }
}
