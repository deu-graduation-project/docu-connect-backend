using FotokopiRandevuAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Token
{
    public interface ITokenHandler
    {
        Task<DTOs.Token> CreateAccessToken(AppUser user,int day );

        string CreateRefreshToken();
    }
}
