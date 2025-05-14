using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Order.UpdateOrder
{
    public class UpdateOrderCommandRequest:IRequest<UpdateOrderCommandResponse>
    {
        public string? OrderState { get; set; }
        public string OrderCode { get; set; }
        public List<string>? removeCommentIds { get; set; }=new List<string>();
        public string? CompletedCode { get; set; }
    }
}
