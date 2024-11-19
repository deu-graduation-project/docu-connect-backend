using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Product.GetAgencyProducts
{
    public class GetAgencyProductsQueryResponse
    {
        public object AgencyProducts { get; set; }
    }
}
