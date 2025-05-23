using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyCommentAnalysis
{
    public class GetAgencyCommentAnalysisCommandRequest:IRequest<GetAgencyCommentAnalysisCommandResponse>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string?  GroupBy{ get; set; }
    }
}
