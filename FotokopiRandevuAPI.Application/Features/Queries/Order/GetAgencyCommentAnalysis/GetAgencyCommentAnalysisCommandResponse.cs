using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyCommentAnalysis
{
    public class GetAgencyCommentAnalysisCommandResponse
    {
        public List<DTOs.Order.GetAgencyCommentAnalysis> GetAgencyCommentAnalysis { get; set; }
    }
}
