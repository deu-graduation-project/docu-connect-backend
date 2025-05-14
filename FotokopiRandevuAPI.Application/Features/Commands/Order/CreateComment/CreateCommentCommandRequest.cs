using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.CreateComment
{
    public class CreateCommentCommandRequest : IRequest<CreateCommentCommandResponse>
    {
        public int StarRating { get; set; }
        public string CommentText { get; set; }
        public string OrderCode { get; set; }
    }
}
