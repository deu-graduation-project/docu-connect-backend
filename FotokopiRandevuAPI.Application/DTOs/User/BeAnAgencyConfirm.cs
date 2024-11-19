using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.User
{
    public class BeAnAgencyConfirm
    {
        public string BeAnAgencyRequestId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
