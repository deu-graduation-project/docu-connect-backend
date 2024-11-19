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
    public class FileReadRepository : ReadRepository<CopyFile>, IFileReadRepository
    {
        public FileReadRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
