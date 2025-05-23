using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetOrderProductAnalysis
{
    public class GetOrderProductAnalysisCommandRequest:IRequest<GetOrderProductAnalysisCommandResponse>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? PaperType { get; set; }
        public string? ColorOption { get; set; }
        public string? PrintType { get; set; }
    }
}
