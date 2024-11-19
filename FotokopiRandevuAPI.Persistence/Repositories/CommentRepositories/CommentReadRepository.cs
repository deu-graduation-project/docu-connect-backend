using DenemeTakipAPI.Persistence.Repositories;
using FotokopiRandevuAPI.Application.Repositories.CommentRepositories;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Repositories.CommentRepositories
{
    public class CommentReadRepository : ReadRepository<Comment>, ICommentReadRepository
    {
        public CommentReadRepository(fotokopiRandevuAPIDbContext context) : base(context)
        {
        }
    }
}
