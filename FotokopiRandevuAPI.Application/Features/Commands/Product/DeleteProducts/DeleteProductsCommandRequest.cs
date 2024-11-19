using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteProducts
{
    public class DeleteProductsCommandRequest:IRequest<DeleteProductsCommandResponse>
    {
        public List<string> ProductIds { get; set; }
    }
}
