using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetBeAnAgencyRequests
{
    public class GetBeAnAgencyRequestsQueryResponse
    {
        public int TotalCount { get; set; }
        public object BeAnAgencyRequests { get; set; }
    }
}
