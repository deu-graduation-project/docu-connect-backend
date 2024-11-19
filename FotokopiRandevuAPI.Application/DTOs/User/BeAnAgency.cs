using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.DTOs.User
{
    public class BeAnAgency
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public string AgencyName { get; set; }
        public Address Address { get; set; }
        public string? AgencyBio { get; set; }

    }
}
