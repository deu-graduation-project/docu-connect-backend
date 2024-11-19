using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetSingleAgency
{
    public class GetSingleAgencyQueryRequest:IRequest<GetSingleAgencyQueryResponse>
    {
        public string AgencyId { get; set; }
    }
}
