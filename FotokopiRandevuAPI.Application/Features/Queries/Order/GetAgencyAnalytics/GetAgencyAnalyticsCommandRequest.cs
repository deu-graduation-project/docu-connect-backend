using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyAnalytics
{
    public class GetAgencyAnalyticsCommandRequest:IRequest<GetAgencyAnalyticsCommandResponse>
    {
        public string? AgencyId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string GroupBy { get; set; }="day";
    }
}
