using FluentValidation;
using FotokopiRandevuAPI.Application.DTOs.Product;
using FotokopiRandevuAPI.Application.Features.Commands.Product.CreateAgencyProduct;
using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Validators.Product
{
    public class CreateAgencyProductValidator : AbstractValidator<CreateAgencyProduct>
    {
        public CreateAgencyProductValidator()
        {
            RuleFor(p => p.Price).NotNull().NotEmpty().WithMessage("Fiyat bilgisi boş olmamalıdır.")
                .Must(p => p > 0).WithMessage("Fiyat 0 dan büyük olmamalıdır.");
            RuleFor(p => p.ProductId).NotNull().NotEmpty().WithMessage("Product id kısmı boş olmamalıdır.");

        }
    }
}
