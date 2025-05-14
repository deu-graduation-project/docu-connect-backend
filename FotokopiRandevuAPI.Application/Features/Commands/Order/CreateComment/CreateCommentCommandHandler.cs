using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        readonly IOrderService _orderService;

        public CreateCommentCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var response =await _orderService.CreateComment(new()
            {
                CommentText = request.CommentText,
                OrderCode = request.OrderCode,
                StarRating = request.StarRating,
            });
            return new()
            {
                Message= response.Message,
                Succeeded=response.Succeeded,
            };
        }
    }
}
