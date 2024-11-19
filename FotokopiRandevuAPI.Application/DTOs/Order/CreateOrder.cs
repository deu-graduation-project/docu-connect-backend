using FotokopiRandevuAPI.Domain.Entities.Files;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Order
{
    public class CreateOrder
    {
        public string AgencyProductId { get; set; }
        public string AgencyId { get; set; }

        public int KopyaSayısı { get; set; }
        public List<IFormFile> CopyFiles { get; set; } = new List<IFormFile>();


    }
}
