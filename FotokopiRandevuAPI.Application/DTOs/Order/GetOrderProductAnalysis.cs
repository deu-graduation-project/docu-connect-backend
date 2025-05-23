using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Order
{
    public class GetOrderProductAnalysis
    {
        public List<GetOrderProductAnalysisElement> GetOrderProductAnalysisElements { get; set; } = new();
    }
    public class GetOrderProductAnalysisElement
    {
        public string? PaperType { get; set; }
        public string? ColorOption { get; set; }
        public string? PrintType { get; set; }
        public int Count { get; set; }
    }
}
