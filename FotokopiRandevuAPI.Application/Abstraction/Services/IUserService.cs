using DenemeTakipAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Application.DTOs;
using FotokopiRandevuAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUserAsync(CreateUser createUser);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        //Task<UserList> GetAllUsers(int page, int size, string? nameOrEmail);
        Task<string[]> GetUserRoles(string username);
        Task AssignToRolesAsync(string userId, string[] roles);
        //Task<UserListSingle> GetUserById(string userId);
        Task<SucceededMessageResponse> UpdatePassword(string userId, string resetToken, string newPassword);

        Task<SucceededMessageResponse> UpdateUserPassword(string currentPassword, string newPassword);
        Task<BeAnAgencyResponse> BeAnAgencyRequestAsync(BeAnAgency beAnAgency);
        Task<BeAnAgencyResponse> BeAnAgencyConfirmAsync(BeAnAgencyConfirm beAnAgency);

        Task<GetBeAnAgencyRequestsPaginated> GetBeAnAgencyRequests(int page, int size, string? orderby,string? requestId, string? search, string? state);
        Task<GetAgenciesPaginated> GetAgencies(int page, int size,string? agencyName,string? province,string? district, string? orderBy);
        Task<GetSingleAgencyResponse> GetSingleAgency(string agencyId);
        Task UpdateStarRating(string agencyId);
    }
}
