using FotokopiRandevuAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Products
{
    public class Product:BaseEntity
    {
        public string PaperType { get; set; }
        public ColorOptions ColorOption { get; set; }
        public PrintTypes PrintType { get; set; }
        public List<AgencyProduct> AgencyProducts { get; set; } = new List<AgencyProduct>();

    }
    public enum ColorOptions
    {
        SiyahBeyaz=0,
        Renkli=1,
    }
    public enum PrintTypes
    {
        TekYuz = 0,
        CiftYuz = 1,
    }
}
