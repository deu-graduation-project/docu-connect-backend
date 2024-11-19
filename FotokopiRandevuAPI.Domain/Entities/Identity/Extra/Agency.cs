using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Identity.Extra
{
    public class Agency:AppUser
    {
        public string AgencyName { get; set; }
        public Address Address { get; set; }
        public bool IsConfirmedAgency { get; set; }
        public string AgencyBio { get; set; } = "";
        public double StarRating { get; set; } = 0;
        public List<AgencyProduct> AgencyProducts { get; set; }=new List<AgencyProduct>();

    }
}
