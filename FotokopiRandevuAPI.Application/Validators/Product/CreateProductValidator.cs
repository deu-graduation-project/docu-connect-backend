using FluentValidation;
using FotokopiRandevuAPI.Application.DTOs.Product;
using FotokopiRandevuAPI.Application.Features.Commands.Product.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Validators.Product
{
    public class CreateProductValidator:AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(p=>p.PrintType).NotEmpty().NotNull().WithMessage("Basım türü boş olmamalıdır.");
            RuleFor(p => p.PaperType).NotEmpty().NotNull().WithMessage("Kağıt türü boş olmamalıdır.");
            RuleFor(p => p.ColorOption).NotEmpty().NotNull().WithMessage("Renk türü boş olmamalıdır.");
        }
    }
}
