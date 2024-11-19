using FluentValidation;
using FotokopiRandevuAPI.Application.DTOs.Order;
using FotokopiRandevuAPI.Application.Features.Commands.Order.CreateOrder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Validators.Order
{
    public class CreateOrderValidator:AbstractValidator<CreateOrderCommandRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(o => o.KopyaSayısı).NotEmpty().NotNull().WithMessage("Kopya sayısı kısmı boş olmamalıdır.")
                .Must(o => o >= 1).WithMessage("Kopya sayısı 1 veya fazla olmalıdır.");
            RuleFor(o => o.AgencyId).NotEmpty().NotNull().WithMessage("AgencyId kısmı boş olmamalıdır.");
            RuleFor(o => o.AgencyProductId).NotEmpty().NotNull().WithMessage("AgencyProductId kısmı boş olmamalıdır.");
            RuleFor(o => o.CopyFiles).NotEmpty().NotNull().WithMessage("Dosya kısmı boş olmamalıdır.");
        }
        

    }
}
