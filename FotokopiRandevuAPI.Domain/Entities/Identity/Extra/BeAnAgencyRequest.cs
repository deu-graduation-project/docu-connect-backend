using FotokopiRandevuAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Identity.Extra
{
    public class BeAnAgencyRequest:BaseEntity
    {
        public string AgencyName { get; set; }
        public string AgencyId { get; set; }
        public AppUser Customer { get; set; }
        public BeAnAgencyRequestState State { get; set; }
        public Address Address { get; set; }

    }
    public enum BeAnAgencyRequestState
    {
        Pending = 0,
        Confirmed=1,
        Rejected=2
    }
}
