using FluentValidation;
using FotokopiRandevuAPI.Application.DTOs.Comment;
using FotokopiRandevuAPI.Application.Features.Commands.Order.UpdateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Validators.Comment
{
    public class CreateCommentValidator : AbstractValidator<CreateComment>
    {
        public CreateCommentValidator()
        {
            RuleFor(c => c.StarRating).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5)
                .WithMessage("Yıldız değerlendirmesi 1 ile 5 arasında olmalıdır.")
                .NotEmpty().NotNull().WithMessage("Yıldız değerlendirmesi boş olmamalıdır.");
            RuleFor(c => c.CommentText).MaximumLength(100).WithMessage("Yorum 100 karakterden az olmalıdır.");
            RuleFor(c => c.OrderCode).NotNull().NotEmpty().WithMessage("OrderCode alanı boş olmamalıdır.");
        }
    }
}   
