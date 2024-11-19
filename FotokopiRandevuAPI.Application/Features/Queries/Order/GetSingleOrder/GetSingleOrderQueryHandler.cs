using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetSingleOrder
{
    public class GetSingleOrderQueryHandler : IRequestHandler<GetSingleOrderQueryRequest, GetSingleOrderQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetSingleOrderQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetSingleOrderQueryResponse> Handle(GetSingleOrderQueryRequest request, CancellationToken cancellationToken)
        {
            var response=await _orderService.GetSingleOrder(request.OrderCode);
            return new()
            {
                Order = response.Order,
            };
        }
    }
}
