using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Product
{
    public class CreateProduct
    {
        public string PaperType { get; set; }
        public ColorOptions ColorOption { get; set; }
        public PrintTypes PrintType { get; set; }
    }
}
