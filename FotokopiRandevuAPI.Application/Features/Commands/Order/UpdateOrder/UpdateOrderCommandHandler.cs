using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommandRequest, UpdateOrderCommandResponse>
    {
        readonly IOrderService _orderService;

        public UpdateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<UpdateOrderCommandResponse> Handle(UpdateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _orderService.UpdateOrder(request.OrderState,request.removeCommentIds,request.CompletedCode,request.OrderCode);
            return new()
            {
                Succeeded = response.Succeeded,
                Message = response.Message
            };
        }
    }
}
