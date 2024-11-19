using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.DTOs.Product;
using FotokopiRandevuAPI.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductService _productService;

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            if(Enum.TryParse<PrintTypes>(request.PrintType,true,out var printType) && Enum.TryParse<ColorOptions>(request.ColorOption,true,out var colorOption) )
            {
                if (!Enum.IsDefined(typeof(ColorOptions), colorOption))
                {
                    return new()
                    {
                        Succeeded = false,
                        Message = "Renk türü doğru biçimde girilmemiştir."
                    };
                }
                if (!Enum.IsDefined(typeof(PrintTypes), printType))
                {
                    return new()
                    {
                        Succeeded = false,
                        Message = "Basım türü doğru biçimde girilmemiştir."
                    };
                }
                var response = await _productService.CreateProduct(new()
                {
                    PaperType = request.PaperType,
                    ColorOption = colorOption,
                    PrintType = printType,
                });
                return new()
                {
                    Message = response.Message,
                    Succeeded = response.Succeeded,
                };
            }
            else
            {
                return new()
                {
                    Message = "Renk türü ve basım biçimi doğru formatta girilmemiştir.",
                    Succeeded = false,
                };
            }
        }
    }
}
