using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Product
{
    public class CreateAgencyProducts
    {
        public List<CreateAgencyProduct> UpdateOrCreateAgencyProducts { get; set; }= new List<CreateAgencyProduct>();
    }
}
