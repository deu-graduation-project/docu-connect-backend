using FotokopiRandevuAPI.Domain.Entities.Common;
using FotokopiRandevuAPI.Domain.Entities.Files;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Order
{
    public class Order:BaseEntity
    {
        public Customer Customer { get; set; }
        public Agency Agency { get; set; }
        public decimal TotalPrice { get; set; }
        public Comment? Comment { get; set; }
        public OrderState OrderState { get; set; }
        public AgencyProduct AgencyProduct { get; set; }=new AgencyProduct();
        public int KopyaSayısı { get; set; }
        public string OrderCode { get; set; }
        public int SayfaSayısı { get; set; }
        public string?  CompletedCode { get; set; }
        public List<CopyFile> CopyFiles { get; set; } = new List<CopyFile>();
        public DateTime UpdatedDate { get; set; }
    }
    public enum OrderState
    {
        Pending=0,
        Confirmed=1,
        Rejected=2,
        Started=3,
        Finished=4,
        Completed=5,
    }
}
