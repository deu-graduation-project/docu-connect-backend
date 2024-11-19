using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.FileRepositories;
using FotokopiRandevuAPI.Domain.Entities.Files;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.FileRepositories
{
    public class FileWriteRepository : WriteRepository<CopyFile>, IFileWriteRepository
    {
        public FileWriteRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
