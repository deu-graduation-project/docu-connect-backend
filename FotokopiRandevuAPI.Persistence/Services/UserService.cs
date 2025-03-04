using Azure.Core;
using DenemeTakipAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Application.Abstraction.Hubs;
using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.DTOs;
using FotokopiRandevuAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Application.Repositories.CommentRepositories;
using FotokopiRandevuAPI.Application.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Application.Repositories.UserRepositories.AgencyRepositories;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Products;
using FotokopiRandevuAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly IUserHubService _userHubService;
        readonly ICommentReadRepository _commentReadRepository;
        readonly IOrderReadRepository _orderReadRepository;
        public UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IBeAnAgencyRequestWriteRepository beAnAgencyRequestWriteRepository, IBeAnAgencyRequestReadRepository beAnAgencyRequestReadRepository, fotokopiRandevuAPIDbContext dbContext, IUserHubService userHubService, IWebHostEnvironment webHostEnvironment, IOrderReadRepository orderReadRepository, ICommentReadRepository commentReadRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _beAnAgencyRequestWriteRepository = beAnAgencyRequestWriteRepository;
            _beAnAgencyRequestReadRepository = beAnAgencyRequestReadRepository;
            _dbContext = dbContext;
            _userHubService = userHubService;
            _webHostEnvironment = webHostEnvironment;
            _orderReadRepository = orderReadRepository;
            _commentReadRepository = commentReadRepository;
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
        public async Task UpdateStarRating(string agencyId)
        {
            var agency = await _userManager.Users.OfType<Agency>().Include(u => u.Comments).FirstOrDefaultAsync(u => u.Id == agencyId);
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
            IdentityResult result = await _userManager.CreateAsync(customer, createUser.Password);
            CreateUserResponse response = new() { Succeeded = result.Succeeded };
            if (result.Succeeded)
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
            AppUser user = await _userManager.FindByIdAsync(username);
            if (user == null)
            {
                var userRoles = await _userManager.FindByEmailAsync(username);
            }
            if (user == null)
                user = await _userManager.FindByIdAsync(username);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToArray();
            }
            return new string[] { };
        }
        public async Task<SucceededMessageResponse> UpdatePassword(string userId, string resetToken, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
            if (user != null)
            {
                byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                resetToken = Encoding.UTF8.GetString(tokenBytes);
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    return new()
                    {
                        Succeeded = true,
                        Message = "Şifreniz başarıyla yenilenmiştir."
                    };
                }
                else
                {
                    var errorMessages = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        switch (error.Code)
                        {
                            case "PasswordTooShort":
                                errorMessages.Add("Yeni şifre çok kısa. En az 6 karakter olmalıdır.");
                                break;
                            case "PasswordRequiresNonAlphanumeric":
                                errorMessages.Add("Yeni şifre en az bir özel karakter içermelidir.");
                                break;
                            case "PasswordRequiresDigit":
                                errorMessages.Add("Yeni şifre en az bir rakam içermelidir.");
                                break;
                            case "PasswordRequiresLower":
                                errorMessages.Add("Yeni şifre en az bir küçük harf içermelidir.");
                                break;
                            case "PasswordRequiresUpper":
                                errorMessages.Add("Yeni şifre en az bir büyük harf içermelidir.");
                                break;
                            case "InvalidToken":
                                errorMessages.Add("Geçersiz veya süresi dolmuş token. Lütfen tekrar deneyin.");
                                break;
                            default:
                                errorMessages.Add("Şifre değiştirilirken bilinmeyen bir hata oluştu.");
                                break;
                        }
                    }

                    var detailedErrorMessage = string.Join(" ", errorMessages);

                    return new()
                    {
                        Succeeded = false,
                        Message = $"{detailedErrorMessage}"
                    };
                }
            }
            return new()
            {
                Succeeded = false,
                Message = "Kullanıcı bulunamadı."
            };
        }
        public async Task<SucceededMessageResponse> UpdateUserPassword(string currentPassword, string newPassword)
        {
            var user = await ContextUser();
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {

                return new()
                {
                    Succeeded = true,
                    Message = "Şifreniz başarıyla değiştirilmiştir."
                };
            }
            else
            {
                var errorMessages = new List<string>();
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordMismatch":
                            errorMessages.Add("Mevcut şifre yanlış.");
                            break;
                        case "PasswordTooShort":
                            errorMessages.Add("Yeni şifre çok kısa. En az 6 karakter olmalıdır.");
                            break;
                        case "PasswordRequiresNonAlphanumeric":
                            errorMessages.Add("Yeni şifre en az bir özel karakter içermelidir.");
                            break;
                        case "PasswordRequiresDigit":
                            errorMessages.Add("Yeni şifre en az bir rakam içermelidir.");
                            break;
                        case "PasswordRequiresLower":
                            errorMessages.Add("Yeni şifre en az bir küçük harf içermelidir.");
                            break;
                        case "PasswordRequiresUpper":
                            errorMessages.Add("Yeni şifre en az bir büyük harf içermelidir.");
                            break;
                        default:
                            errorMessages.Add("Şifre değiştirilirken bilinmeyen bir hata oluştu.");
                            break;
                    }
                }
                var detailedErrorMessage = string.Join(" ", errorMessages);

                return new()
                {
                    Succeeded = false,
                    Message = $"{detailedErrorMessage}"
                };
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

            string? profilePhotoPath = null;
            if (beAnAgency.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profilePhotos");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(beAnAgency.ProfilePhoto.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await beAnAgency.ProfilePhoto.CopyToAsync(stream);
                }

                profilePhotoPath = $"/profilePhotos/{fileName}";
            }
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
                ProfilePhotoPath = profilePhotoPath
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
            if (request != null)
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

        public async Task<GetBeAnAgencyRequestsPaginated> GetBeAnAgencyRequests(int page, int size, string? orderby, string? requestId, string? usernameOrEmail, string? state)
        {
            var query = _beAnAgencyRequestReadRepository.GetAll(false).Include(u=>u.Customer).AsQueryable();
            if (!string.IsNullOrEmpty(state))
            {
                if (Enum.TryParse<BeAnAgencyRequestState>(state, true, out var requestState))
                {
                    query = query.Where(u => u.State == requestState);
                }
                else
                {
                    throw new Exception("Firma olma isteği durumu için geçersiz bir değer girilmiştir.");
                    ;
                }
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
            var beAnAgencyRequest = await query
               .Skip((page - 1) * size)
               .Take(size)
               .ToListAsync();

            var result = new List<object>();
            foreach (var u in beAnAgencyRequest)
            {
                byte[] profilePhoto = null;
                var user = await _userManager.FindByIdAsync(u.AgencyId);
                var agency = user as Agency;

                if (agency != null && !string.IsNullOrEmpty(agency.ProfilePhotoPath))
                {
                    var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, agency.ProfilePhotoPath.TrimStart('/'));
                    if (File.Exists(fullPath))
                    {
                        profilePhoto = await File.ReadAllBytesAsync(fullPath);
                    }
                }

                result.Add(new
                {
                    BeAnAgencyRequestId = u.Id,
                    AgencyName = u.AgencyName,
                    AgencyId = u.AgencyId,
                    BeAnAgencyRequestState = u.State,
                    Address = u.Address,
                    Name = u.Customer?.Name,
                    Surname = u.Customer?.Surname,
                    Email = u.Customer?.Email,
                    ProfilePhoto = profilePhoto != null ? Convert.ToBase64String(profilePhoto) : null
                }); 
            }

            return new()
            {
                TotalCount = totalCount,
                BeAnAgencyRequests = result,
            };
        }

        public async Task<GetAgenciesPaginated> GetAgencies(int page, int size, string? agencyName, string? province, string? district, string? orderBy)
        {
            var query = _userManager.Users.OfType<Agency>().Where(u => u.IsConfirmedAgency == true).AsQueryable();
            if (!string.IsNullOrEmpty(agencyName))
                query = query.Where(u => u.AgencyName.Contains(agencyName));
            if (!string.IsNullOrEmpty(province))
                query = query.Where(u => u.Address.Province.ToLower() == province.ToLower());
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
                StarRating = a.StarRating,
                ProfilePhoto = !string.IsNullOrEmpty(a.ProfilePhotoPath) && File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, a.ProfilePhotoPath.TrimStart('/')))
    ? Convert.ToBase64String(File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, a.ProfilePhotoPath.TrimStart('/'))))
    : null 
            }).ToListAsync();

            return new()
            {
                TotalCount = totalCount,
                Agencies = agencies,
            };

        }

        public async Task<GetSingleAgencyResponse> GetSingleAgency(string agencyId)
        {
            var agency = await _userManager.Users.OfType<Agency>().Where(u => u.Id == agencyId && u.IsConfirmedAgency).Include(u => u.AgencyProducts).Select(u => new
            {
                AgencyId = u.Id,
                AgencyName = u.AgencyName,
                AgencyBio = u.AgencyBio,
                Province = u.Address.Province,
                District = u.Address.District,
                AddressExtra = u.Address.Extra,
                AgencyProducts = u.AgencyProducts.Select(p => new
                {
                    PrintType = p.Product.PrintType,
                    ColorOption = p.Product.ColorOption,
                    PaperType = p.Product.PaperType,
                    Price = p.Price,
                    ProductId = p.Id,
                }),
                Comments = u.Comments.Select(c => new
                {
                    CommentText = c.CommentText,
                    StarRating = c.StarRating != 0 ? c.StarRating.ToString() : "Bu firma daha önce bir değerlendirilmedi."
                }),
                ProfilePhoto = !string.IsNullOrEmpty(u.ProfilePhotoPath) && File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, u.ProfilePhotoPath.TrimStart('/')))
    ? Convert.ToBase64String(File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, u.ProfilePhotoPath.TrimStart('/'))))
    : null
            }).FirstOrDefaultAsync();
            if (agency == null)
                throw new Exception("Böyle bir onaylı firma bulunamadı.");
            return new()
            {
                Agency = agency
            };
        }

        public async Task<SucceededMessageResponse> UpdateAgencyInfos(string? name, string? surname, string? agencyName, string? province, string? district, string? extra, string? agencyBio, IFormFile? ProfilePhoto)
        {
            var agency = await ContextUser() as Agency;

            if (agency == null)
            {
                return new()
                {
                    Succeeded = false,
                    Message = "Firma bulunamadı."
                };
            }
            if (name != null)
                agency.Name = name;
            if (surname != null)
                agency.Surname = surname;
            if (agencyName != null)
                agency.AgencyName = agencyName;
            if (province != null)
                agency.Address.Province = province;
            if (district != null)
                agency.Address.District = district;
            if (extra != null)
                agency.Address.Extra = extra;
            if (agencyBio != null)
                agency.AgencyBio = agencyBio;
            if (ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profilePhotos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(agency.ProfilePhotoPath))
                {
                    string existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, agency.ProfilePhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfilePhoto.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePhoto.CopyToAsync(stream);
                }
                agency.ProfilePhotoPath = $"/profilePhotos/{fileName}";
            }
            var result = await _userManager.UpdateAsync(agency);
            if (!result.Succeeded)
            {
                return new SucceededMessageResponse
                {
                    Succeeded = false,
                    Message = "Firma bilgileri güncellenirken hata oluştu."
                };
            }

            return new SucceededMessageResponse
            {
                Succeeded = true,
                Message = "Firma bilgileri başarıyla güncellendi."
            };

        }

        public async Task<UserById> GetUserById(string? userId)
        {
            var user = await ContextUser();
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("admin"))
            {
                var searchedUser = await _userManager.FindByIdAsync(userId);
                if (searchedUser == null)
                    throw new Exception("Kullanıcı bulunamadı.");
                var userComments = await _commentReadRepository.GetWhere(u => u.Order.Customer.Id == searchedUser.Id).Select(u => new
                {
                    CommentText = u.CommentText,
                    StarRating = u.StarRating
                }).ToListAsync();
                var userOrders = await _orderReadRepository.GetWhere(u => u.Customer.Id == searchedUser.Id).Include(u => u.AgencyProduct).ThenInclude(u => u.Product).Select(u => new
                {
                    OrderCode = u.OrderCode,
                    OrderState = u.OrderState.ToString(),
                    TotalPrice = u.TotalPrice,
                    KopyaSayısı = u.KopyaSayısı,
                    SayfaSayısı = u.SayfaSayısı,
                    AgencyName = u.Agency.AgencyName,
                    CustomerName = u.Customer.UserName,
                    Product = new
                    {
                        Price = u.AgencyProduct.Price,
                        PrintType = u.AgencyProduct.Product.PrintType,
                        PaperType = u.AgencyProduct.Product.PaperType,
                        ColorOption = u.AgencyProduct.Product.ColorOption,
                    },
                    CopyFiles = u.CopyFiles.Select(c => new
                    {
                        FileName = c.FileName,
                        FilePath = c.FilePath
                    })
                }).ToListAsync();
                return new UserById()
                {
                    UserId = searchedUser.Id,
                    UserName = searchedUser.UserName,
                    Name = searchedUser.Name,
                    Surname = searchedUser.Surname,
                    Email = searchedUser.Email,
                    EmailConfirmed = searchedUser.EmailConfirmed,
                    UserComments = userComments,
                    UserOrders = userOrders
                };
            }
            else if (userRoles.Contains("customer"))
            {
                var userComments = await _commentReadRepository.GetWhere(u => u.Order.Customer.Id == user.Id).Select(u => new
                {
                    CommentText = u.CommentText,
                    StarRating = u.StarRating
                }).ToListAsync();
                var userOrders = await _orderReadRepository.GetWhere(u => u.Customer.Id == user.Id).Include(u => u.AgencyProduct).ThenInclude(u => u.Product).Select(u => new
                {
                    OrderCode = u.OrderCode,
                    OrderState = u.OrderState.ToString(),
                    TotalPrice = u.TotalPrice,
                    KopyaSayısı = u.KopyaSayısı,
                    SayfaSayısı = u.SayfaSayısı,
                    AgencyName = u.Agency.AgencyName,
                    CustomerName = u.Customer.UserName,
                    Product = new
                    {
                        Price = u.AgencyProduct.Price,
                        PrintType = u.AgencyProduct.Product.PrintType,
                        PaperType = u.AgencyProduct.Product.PaperType,
                        ColorOption = u.AgencyProduct.Product.ColorOption,
                    },
                    CopyFiles = u.CopyFiles.Select(c => new
                    {
                        FileName = c.FileName,
                        FilePath = c.FilePath
                    })
                }).ToListAsync();
                return new UserById()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    UserComments = userComments,
                    UserOrders = userOrders
                };
            }
            else throw new Exception("Bu istek için gerekli role sahip değilsiniz.");
        }
    }
}
