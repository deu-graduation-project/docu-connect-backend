using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteAgencyProducts
{
    public class DeleteAgencyProductsCommandHandler : IRequestHandler<DeleteAgencyProductsCommandRequest, DeleteAgencyProductsCommandResponse>
    {
        readonly IProductService _productService;

        public DeleteAgencyProductsCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<DeleteAgencyProductsCommandResponse> Handle(DeleteAgencyProductsCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _productService.DeleteAgencyProducts(request.AgencyProductIds);
            return new()
            {
                Succeeded = response.Succeeded,
                Message= response.Message,
            };
        }
    }
}
