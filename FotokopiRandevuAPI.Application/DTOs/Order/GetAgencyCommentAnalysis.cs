using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.Order
{
    public class GetAgencyCommentAnalysis
    {
        public int Count { get; set; }
        public DateTime Period{ get; set; }
        public float AverageStar { get; set; }
        public int TotalUserCount { get; set; }
    }
}
