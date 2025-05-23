using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetOrderProductAnalysis
{
    public class GetOrderProductAnalysisCommandHandler : IRequestHandler<GetOrderProductAnalysisCommandRequest, GetOrderProductAnalysisCommandResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderProductAnalysisCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderProductAnalysisCommandResponse> Handle(GetOrderProductAnalysisCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _orderService.GetOrderProductAnalysis(request.StartDate,request.EndDate,request.PaperType,request.ColorOption,request.PrintType);
            return new()
            {
                GetOrderProductAnalysis = response
            };
        }
    }
}
