using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IAuthService
    {
        Task<DTOs.Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);
        Task<DTOs.Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime);
        Task PasswordResetAsync(string emailOrUserName);
        Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
