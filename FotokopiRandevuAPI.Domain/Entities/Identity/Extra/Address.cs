using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Identity.Extra
{
    [Owned]
    public class Address
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Extra { get; set; }

    }
}
