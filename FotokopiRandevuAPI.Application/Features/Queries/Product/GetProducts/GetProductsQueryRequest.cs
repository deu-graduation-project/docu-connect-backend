using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Product.GetProducts
{
    public class GetProductsQueryRequest:IRequest<GetProductsQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }

    }
}
