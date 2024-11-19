using FotokopiRandevuAPI.Domain.Entities.Common;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Order
{
    public class Comment:BaseEntity
    {
        public int StarRating { get; set; }
        public string CommentText { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Customer Customer { get; set; }
        public Agency Agency { get; set; }


    }
}
