using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyCommentAnalysis
{
    public class GetAgencyCommentAnalysisCommandHandler : IRequestHandler<GetAgencyCommentAnalysisCommandRequest, GetAgencyCommentAnalysisCommandResponse>
    {
        readonly IOrderService _orderService;

        public GetAgencyCommentAnalysisCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAgencyCommentAnalysisCommandResponse> Handle(GetAgencyCommentAnalysisCommandRequest request, CancellationToken cancellationToken)
        {
            var response= await _orderService.GetAgencyCommentAnalysis(request.StartDate,request.EndDate,request.GroupBy);
            return new()
            {
                GetAgencyCommentAnalysis = response
            };
        }
    }
}
