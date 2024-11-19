using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyAnalytics
{
    public class GetAgencyAnalyticsCommandHandler : IRequestHandler<GetAgencyAnalyticsCommandRequest, GetAgencyAnalyticsCommandResponse>
    {
        readonly IOrderService _orderService;

        public GetAgencyAnalyticsCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAgencyAnalyticsCommandResponse> Handle(GetAgencyAnalyticsCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _orderService.GetAgencyAnalytics(request.AgencyId,request.StartDate,request.EndDate,request.GroupBy);
            return new()
            {
                AgencyAnalytics = response.AgencyAnalytics
            };
        }
    }
}
