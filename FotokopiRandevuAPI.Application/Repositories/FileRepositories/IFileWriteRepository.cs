using DenemeTakipAPI.Application.Repositories;
using FotokopiRandevuAPI.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Repositories.FileRepositories
{
    public interface IFileWriteRepository:IWriteRepository<CopyFile>
    {
    }
}
