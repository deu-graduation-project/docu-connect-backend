using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        readonly IOrderService _orderService;

        public CreateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _orderService.CreateOrder(new()
            {
                AgencyId = request.AgencyId,
                AgencyProductId = request.AgencyProductId,
                KopyaSayısı=request.KopyaSayısı,
                CopyFiles=request.CopyFiles,
            });
            return new()
            {
                Succeeded = response.Succeeded,
                Message = response.Message
            };
        }
    }
}
