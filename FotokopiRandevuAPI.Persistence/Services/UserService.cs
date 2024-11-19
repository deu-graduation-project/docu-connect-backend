﻿using Azure.Core;
using DenemeTakipAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Application.Abstraction.Hubs;
using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Application.Repositories.UserRepositories.AgencyRepositories;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IBeAnAgencyRequestWriteRepository _beAnAgencyRequestWriteRepository;
        readonly IBeAnAgencyRequestReadRepository _beAnAgencyRequestReadRepository;
        readonly fotokopiRandevuAPIDbContext _dbContext;
        readonly IUserHubService _userHubService;
        public UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IBeAnAgencyRequestWriteRepository beAnAgencyRequestWriteRepository, IBeAnAgencyRequestReadRepository beAnAgencyRequestReadRepository, fotokopiRandevuAPIDbContext dbContext, IUserHubService userHubService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _beAnAgencyRequestWriteRepository = beAnAgencyRequestWriteRepository;
            _beAnAgencyRequestReadRepository = beAnAgencyRequestReadRepository;
            _dbContext = dbContext;
            _userHubService = userHubService;
        }

        private async Task<AppUser> ContextUser()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                AppUser? user = _userManager.Users.FirstOrDefault(u => u.UserName == username);
                return user;
            }
            throw new Exception("Kullanıcı bulunamadı");
        }
        public async Task UpdateStarRating(string agencyId)
        {
            var agency = await _userManager.Users.OfType<Agency>().Include(u=>u.Comments).FirstOrDefaultAsync(u=>u.Id==agencyId);
            if (agency == null)
                throw new Exception("Böyle bir firma bulunmamaktadır.");

            if (agency.Comments.Any())
            {
                agency.StarRating = agency.Comments.Average(u => u.StarRating);
            }
            else
            {
                agency.StarRating = 0;
            }
            await _userManager.UpdateAsync(agency);
        }
        public async Task<CreateUserResponse> CreateUserAsync(CreateUser createUser)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createUser.UserName,
                Email = createUser.Email,
                Name = createUser.Name,
                Surname = createUser.Surname,
            };
            IdentityResult result = await _userManager.CreateAsync(customer,createUser.Password);
            CreateUserResponse response = new() { Succeeded=result.Succeeded};
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(customer, "customer");
                response.Message = "Kullanıcı başarıyla oluşturuldu.";
            }
            else
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateUserName":
                            response.Message += $"{createUser.UserName} kullanıcı adı zaten kullanılmaktadır.\n";
                            break;
                        case "DuplicateEmail":
                            response.Message += $"{createUser.Email} e-mail adresi zaten kullanılmaktadır.\n";
                            break;
                        case "InvalidEmail":
                            response.Message += $"Girilen e-mail adresi doğru değil.\n";
                            break;
                    }
                }
            return response;
        }
        public async Task AssignToRolesAsync(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetUserRoles(string username)
        {
            AppUser user= await _userManager.FindByIdAsync(username);
            if (user == null)
            {
                var userRoles= await _userManager.FindByEmailAsync(username);
            }
            if(user==null)
                user = await _userManager.FindByIdAsync(username);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToArray();
            }
            return new string[] { };
        }

        public async Task UpdatePassword(string userId, string resetToken, string newPassword)
        {
            var user= await _userManager.FindByIdAsync(userId);
            if(user != null) 
            {
                byte[] tokenBytes= WebEncoders.Base64UrlDecode(resetToken);
                resetToken=Encoding.UTF8.GetString(tokenBytes);
                IdentityResult result=await _userManager.ResetPasswordAsync(user, resetToken,newPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                    throw new Exception("Şifre yenilenirken bir hata oluştu.");
            }
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new Exception("Kullanıcı bulunamadı");
        }

        public async Task<BeAnAgencyResponse> BeAnAgencyRequestAsync(BeAnAgency beAnAgency)
        {
            var generatedId = Guid.NewGuid().ToString();
            var agency = new Agency()
            {
                Id = generatedId,
                UserName = beAnAgency.UserName,
                Email = beAnAgency.Email,
                Name = beAnAgency.Name,
                Surname = beAnAgency.Surname,
                AgencyName = beAnAgency.AgencyName,
                Address = beAnAgency.Address,
                IsConfirmedAgency = false,
            };
            IdentityResult result = await _userManager.CreateAsync(agency, beAnAgency.Password);
            BeAnAgencyResponse response = new() { Succeeded = result.Succeeded };
            if (result.Succeeded)
            {
                BeAnAgencyRequest beAnAgencyRequest = new BeAnAgencyRequest()
                {
                    Id = Guid.NewGuid(),
                    AgencyName = beAnAgency.AgencyName,
                    Address = beAnAgency.Address,
                    State = BeAnAgencyRequestState.Pending,
                    AgencyId = generatedId,
                    Customer = await _userManager.FindByIdAsync(generatedId)
                };

                await _userManager.AddToRoleAsync(agency, "customer");
                bool isRequestSucceeded = await _beAnAgencyRequestWriteRepository.AddAsync(beAnAgencyRequest);
                await _beAnAgencyRequestWriteRepository.SaveAsync();
                await _userHubService.BeAnAgencyAddedMessage("Yeni bir firma olma isteği geldi.");
                if (isRequestSucceeded)
                {
                    return new BeAnAgencyResponse
                    {
                        Message = "Firma olma isteği başarıyla yapılmıştır.",
                        Succeeded = true,
                    };
                }
                else
                {
                    return new BeAnAgencyResponse
                    {
                        Message = "Bir hata ile karşılaşıldı.",
                        Succeeded = false,
                    };
                }
            }
            else
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateUserName":
                            response.Message += $"{beAnAgency.UserName} kullanıcı adı zaten kullanılmaktadır.\n";
                            break;
                        case "DuplicateEmail":
                            response.Message += $"{beAnAgency.Email} e-mail adresi zaten kullanılmaktadır.\n";
                            break;
                        case "InvalidEmail":
                            response.Message += $"Girilen e-mail adresi doğru değil.\n";
                            break;
                    }
                }
            return response;
        }

        public async Task<BeAnAgencyResponse> BeAnAgencyConfirmAsync(BeAnAgencyConfirm beAnAgency)
        {
            var request = await _beAnAgencyRequestReadRepository.GetByIdAsync(beAnAgency.BeAnAgencyRequestId);
            if(request != null)
            {
                var user = await _userManager.FindByIdAsync(request.AgencyId) as Agency;
                if (user != null)
                {
                    if (beAnAgency.IsConfirmed)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "customer");
                        await _userManager.AddToRoleAsync(user, "agency");

                        user.IsConfirmedAgency = true;
                        request.State = BeAnAgencyRequestState.Confirmed;

                        await _beAnAgencyRequestWriteRepository.SaveAsync();

                        return new()
                        {
                            Succeeded = true,
                            Message = "Firma olma talebiniz kabul edilmiştir."
                        };
                    }
                    else
                    {
                        request.State = BeAnAgencyRequestState.Rejected;
                        await _beAnAgencyRequestWriteRepository.SaveAsync();

                        return new()
                        {
                            Succeeded = false,
                            Message = "Firma olma talebiniz kabul edilmemiştir."
                        };
                    }
                }
                else
                {
                    return new()
                    {
                        Succeeded = false,
                        Message = "Bir hata ile karşılaşıldı."
                    };

                }
            }
            return new()
            {
                Succeeded = false,
                Message = "Bir hata ile karşılaşıldı."
            };
        }

        public async Task<GetBeAnAgencyRequestsPaginated> GetBeAnAgencyRequests(int page, int size, string? orderby,string? requestId,string? usernameOrEmail,string? state)
        {
            var query = _beAnAgencyRequestReadRepository.GetAll(false).AsQueryable();
            if (!string.IsNullOrEmpty(state))
            {
                if (Enum.TryParse<BeAnAgencyRequestState>(state, true, out var requestState))
                {
                    query = query.Where(u => u.State == requestState);
                }
                else
                {
                    throw new Exception("Firma olma isteği durumu için geçersiz bir değer girilmiştir.");
;               }
            }

            if (!string.IsNullOrEmpty(requestId))
            {
                query = query.Where(u => u.Id.ToString().ToLower() == requestId.ToLower());
            }

            if (!string.IsNullOrEmpty(usernameOrEmail))
            {
                var lowerSearch = usernameOrEmail.ToLower(); 

                query = query.Where(u => u.Customer.UserName.ToLower().Contains(usernameOrEmail) 
                                      || u.Customer.Email.ToLower().Contains(usernameOrEmail));
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                if (orderby.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(u => u.CreatedDate);
                }
                else if (orderby.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(u => u.CreatedDate);
                }
            }
            else
            {
                query = query.OrderBy(u => u.CreatedDate);
            }
            var totalCount = await query.CountAsync();
            var beAnAgencyRequest= await query.Skip((page-1)*size).Take(size).ToListAsync();
            return new()
            {
                TotalCount = totalCount,
                BeAnAgencyRequests = beAnAgencyRequest,
            };
        }

        public async Task<GetAgenciesPaginated> GetAgencies(int page, int size, string? agencyName, string? province, string? district,string? orderBy)
        {
            var query = _userManager.Users.OfType<Agency>().Where(u=>u.IsConfirmedAgency==true).AsQueryable();
            if (!string.IsNullOrEmpty(agencyName))
                query = query.Where(u => u.AgencyName.Contains(agencyName));
            if(!string.IsNullOrEmpty(province))
                query = query.Where(u => u.Address.Province.ToLower()== province.ToLower());
            if (!string.IsNullOrEmpty(district))
                query = query.Where(u => u.Address.District.ToLower() == district.ToLower());
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderBy.Equals("starRating", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(u => u.StarRating);
                }
            }
            var totalCount = await query.CountAsync();
            var agencies = await query.Skip((page - 1) * size).Take(size).Select(a => new
            {
                AgencyId = a.Id,
                AgencyName = a.AgencyName,
                Province = a.Address.Province,
                District = a.Address.District,
                Extra = a.Address.Extra,
                StarRating=a.StarRating
            }).ToListAsync();

            return new()
            {
                TotalCount = totalCount,
                Agencies = agencies,
            };

        }

        public async Task<GetSingleAgencyResponse> GetSingleAgency(string agencyId)
        {
            var agency =await _userManager.Users.OfType<Agency>().Where(u => u.Id == agencyId && u.IsConfirmedAgency).Include(u=>u.AgencyProducts).Select(u => new
            {
                AgencyName = u.AgencyName,
                AgencyBio = u.AgencyBio,
                Province = u.Address.Province,
                District = u.Address.District,
                AddressExtra = u.Address.Extra,
                AgencyProducts = u.AgencyProducts.Select(p => new
                {
                    PrintType=p.Product.PrintType,
                    ColorOption = p.Product.ColorOption,
                    PaperType = p.Product.PaperType,
                    Price = p.Price,
                    ProductId=p.Id,
                }),
                Comments = u.Comments.Select(c => new
                {
                    CommentText = c.CommentText,
                    StarRating = c.StarRating != 0 ? c.StarRating.ToString() : "Bu firma daha önce bir değerlendirilmedi."
                }),
            }).FirstOrDefaultAsync();
            if (agency == null)
                throw new Exception("Böyle bir onaylı firma bulunamadı.");
            return new()
            {
                Agency = agency
            };
        }
    }
}