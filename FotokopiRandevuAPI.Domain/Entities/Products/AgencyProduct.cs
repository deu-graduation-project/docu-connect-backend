using FotokopiRandevuAPI.Domain.Entities.Common;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Products
{
    public class AgencyProduct:BaseEntity
    {
        public Agency Agency { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }

    }
}
