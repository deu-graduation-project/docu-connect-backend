using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetSingleOrder
{
    public class GetSingleOrderQueryRequest:IRequest<GetSingleOrderQueryResponse>
    {
        public string OrderCode { get; set; }
    }
}
