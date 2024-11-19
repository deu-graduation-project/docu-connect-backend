using FotokopiRandevuAPI.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Domain.Entities.Files
{
    public class CopyFile:BaseEntity
    {
        public string FileName { get; set; }
        public string FilePath{ get; set; }
        public string FileCode { get; set; }
        public Guid OrderId { get; set; }
        public Domain.Entities.Order.Order Order { get; set; }
    }
}
