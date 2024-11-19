using FotokopiRandevuAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.CreateAgencyProduct
{
    public class CreateAgencyProductCommandRequest:IRequest<CreateAgencyProductCommandResponse>
    {
        //public CreateAgencyProducts UpdateOrCreateAgencyProducts { get; set; } = new CreateAgencyProducts();
        public List<DTOs.Product.CreateAgencyProduct> UpdateOrCreateAgencyProducts { get; set; } = new List<DTOs.Product.CreateAgencyProduct>();
    }
}
