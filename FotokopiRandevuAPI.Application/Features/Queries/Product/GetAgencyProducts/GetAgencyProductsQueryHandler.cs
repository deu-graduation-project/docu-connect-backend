using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.Product.GetAgencyProducts
{
    public class GetAgencyProductsQueryHandler : IRequestHandler<GetAgencyProductsQueryRequest, GetAgencyProductsQueryResponse>
    {
        readonly IProductService _productService;

        public GetAgencyProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetAgencyProductsQueryResponse> Handle(GetAgencyProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var response =await _productService.GetAgencyProducts(new()
            {
                AgencyId = request.AgencyId,
            });
            return new()
            {
                AgencyProducts = response.AgencyProducts
            };
        }
    }
}
