using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Product.GetAgencyProducts
{
    public class GetAgencyProductsQueryRequest:IRequest<GetAgencyProductsQueryResponse>
    {
        public string AgencyId { get; set; }
    }
}
