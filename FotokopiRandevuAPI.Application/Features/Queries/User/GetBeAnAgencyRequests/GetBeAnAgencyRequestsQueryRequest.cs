using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetBeAnAgencyRequests
{
    public class GetBeAnAgencyRequestsQueryRequest:IRequest<GetBeAnAgencyRequestsQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string? OrderBy { get; set; }
        public string? UsernameOrEmail { get; set; }
        public string? RequestId { get; set; }
        public string? State { get; set; }


    }
}
