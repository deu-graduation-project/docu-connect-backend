using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Product
{
    public class CreateAgencyProduct
    {
        public string ProductId { get; set; }

        public decimal Price { get; set; }
    }
}
