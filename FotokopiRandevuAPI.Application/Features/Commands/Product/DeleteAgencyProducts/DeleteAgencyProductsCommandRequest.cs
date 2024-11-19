using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteAgencyProducts
{
    public class DeleteAgencyProductsCommandRequest:IRequest<DeleteAgencyProductsCommandResponse>
    {
        public List<string> AgencyProductIds { get; set; }
    }
}
