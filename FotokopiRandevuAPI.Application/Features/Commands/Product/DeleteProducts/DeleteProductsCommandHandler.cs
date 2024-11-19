using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteProducts
{
    public class DeleteAgencyProductsCommandHandler:IRequestHandler<DeleteProductsCommandRequest, DeleteProductsCommandResponse>
    {
        readonly IProductService _productService;

        public DeleteAgencyProductsCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<DeleteProductsCommandResponse> Handle(DeleteProductsCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _productService.DeleteProducts(request.ProductIds);
            return new()
            {
                Succeeded = response.Succeeded,
                Message = response.Message
            };
        }
    }
}
