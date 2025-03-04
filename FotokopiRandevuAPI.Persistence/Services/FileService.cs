using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.Repositories.FileRepositories;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Services
{
    public class FileService : IFileService
    {
        readonly UserManager<AppUser> _userManager;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IFileReadRepository _fileReadRepository;
        public FileService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IFileReadRepository fileReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _fileReadRepository = fileReadRepository;
        }

        private async Task<AppUser> ContextUser()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                AppUser? user = _userManager.Users.FirstOrDefault(u => u.UserName == username);
                return user;
            }
            throw new Exception("Kullanıcı bulunamadı");
        }

        public async Task<bool> DownloadFileCheck(string fileCode)
        {
            
            var user = await ContextUser();
            if (string.IsNullOrEmpty(fileCode))
                throw new Exception("File Code boş olmamalıdır.");
            var file = await _fileReadRepository.GetWhere(u => u.FileCode == fileCode).Include(u => u.Order).FirstOrDefaultAsync();
            if (file.Order.Agency == user || file.Order.Customer == user )
                return true;
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if(userRoles.Contains("admin"))
                    return true;
            }
            throw new Exception("Bu dosyayı indirmek için siparişin size ait olması gerekmektedir.");
           
        }
    }
}
