using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.CreateAgencyProduct
{
    public class CreateAgencyProductCommandHandler : IRequestHandler<CreateAgencyProductCommandRequest, CreateAgencyProductCommandResponse>
    {
        readonly IProductService _productService;

        public CreateAgencyProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<CreateAgencyProductCommandResponse> Handle(CreateAgencyProductCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _productService.CreateAgencyProduct(request.UpdateOrCreateAgencyProducts);
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };
        }
    }
}
