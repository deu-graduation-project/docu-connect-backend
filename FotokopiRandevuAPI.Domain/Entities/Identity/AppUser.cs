using FotokopiRandevuAPI.Domain.Entities.Order;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Domain.Entities.Order.Order> Orders { get; set; } = new List<Domain.Entities.Order.Order>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }

    }
}
