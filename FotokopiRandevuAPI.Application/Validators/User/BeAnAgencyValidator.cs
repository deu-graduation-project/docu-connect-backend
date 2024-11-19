using FluentValidation;
using FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Validators.User
{
    public class BeAnAgencyValidator : AbstractValidator<BeAnAgencyCommandRequest>
    {
        public BeAnAgencyValidator()
        {
            RuleFor(u => u.AgencyName).NotEmpty().NotNull().WithMessage("Firma ismi alanı boş olmamalıdır.")
                .MaximumLength(50).WithMessage("Firma ismi en fazla 50 karakter uzunluğunda olmalıdır.")
                .MinimumLength(5).WithMessage("Firma ismi en az 5 karakter uzunluğunda olmalıdır.");
            RuleFor(u => u.Address.Province).NotNull().NotEmpty().WithMessage("İl alanı boş olmamalıdır.");
            RuleFor(u => u.Address.District).NotNull().NotEmpty().WithMessage("İlçe alanı boş olmamalıdır.");
            RuleFor(u => u.Address.Extra).NotNull().NotEmpty().WithMessage("Adres alanı boş olmamalıdır.")
                .MaximumLength(150).WithMessage("Adres alanı en fazla 150 karakter uzunluğunda olmalıdır");
            RuleFor(u => u.AgencyBio).MaximumLength(150).WithMessage("Firma tanıtımı alanı en fazla 150 karakter uzunluğunda olmalıdır.");
        }
    }
}
