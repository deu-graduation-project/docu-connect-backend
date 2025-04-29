using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetAgencies
{
    public class GetAgenciesQueryRequest:IRequest<GetAgenciesQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }

        public string? AgencyName { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? OrderBy { get; set; }
        public string? PaperType { get; set; }
        public string? ColorOption{ get; set; }
        public string? PrintType { get; set; }

    }
}
