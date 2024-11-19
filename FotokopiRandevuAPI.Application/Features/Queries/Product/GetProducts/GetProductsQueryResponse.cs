using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Product.GetProducts
{
    public class GetProductsQueryResponse
    {
        public int TotalCount { get; set; }
        public object Products { get; set; }
    }
}
