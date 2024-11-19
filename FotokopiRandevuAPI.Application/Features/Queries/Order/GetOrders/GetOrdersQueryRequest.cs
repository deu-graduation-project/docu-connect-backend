using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Order.GetOrders
{
    public class GetOrdersQueryRequest:IRequest<GetOrdersQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string? OrderCode { get; set; }
        public string? Search { get; set; }

        public string? OrderBy { get; set; }
        public string? State { get; set; }
        public string? OrderId { get; set; }

    }
}
