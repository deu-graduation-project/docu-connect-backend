using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IFileService
    {
        Task<bool> DownloadFileCheck(string fileCode);
    }
}
