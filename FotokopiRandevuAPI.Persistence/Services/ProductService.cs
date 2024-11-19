using FotokopiRandevuAPI.Application.Abstraction.Hubs;
using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.DTOs.Product;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.AgencyProducts;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.Products;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Services
{
    public class ProductService : IProductService
    {
        readonly IProductWriteRepository _productWriteRepository;
        readonly IAgencyProductWriteRepository _agencyProductWriteRepository;
        readonly IProductReadRepository _productReadRepository;
        readonly IAgencyProductReadRepository _agencyProductReadRepository;
        readonly UserManager<AppUser> _userManager;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IProductHubService _productHubService;

        public ProductService(IProductWriteRepository productWriteRepository, IAgencyProductWriteRepository agencyProductWriteRepository, IProductReadRepository productReadRepository, IAgencyProductReadRepository agencyProductReadRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IProductHubService productHubService)
        {
            _productWriteRepository = productWriteRepository;
            _agencyProductWriteRepository = agencyProductWriteRepository;
            _productReadRepository = productReadRepository;
            _agencyProductReadRepository = agencyProductReadRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _productHubService = productHubService;
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

        public async Task<CreateAgencyProductResponse> CreateAgencyProduct(List<CreateAgencyProduct> createAgencyProducts)
        {
            var agency = await ContextUser() as Agency;

            if (agency == null)
            {
                return new()
                {
                    Succeeded = false,
                    Message = "Kullanıcı bulunamadı."
                };
            }
            var agencyProducts = await _agencyProductReadRepository.GetWhere(u => u.Agency == agency).Include(u => u.Product).ToListAsync();
            foreach (var agencyProduct in createAgencyProducts)
            {
                var product = await _productReadRepository.GetByIdAsync(agencyProduct.ProductId);
                if (product != null)
                {
                    //Eğer dışardan gelen product daha önceden agencyProducts üzerinde bulunuyorsa var olan agencyProduct fiyatı değişir.
                    if (agencyProducts.Any(u => u.Product.Id.ToString() == agencyProduct.ProductId))
                    {
                        var existingAgencyProduct =await _agencyProductReadRepository
                            .GetWhere(u=>u.Product.Id.ToString() == agencyProduct.ProductId).FirstOrDefaultAsync();
                        existingAgencyProduct.Price = agencyProduct.Price;
                    }
                    else
                    {
                        await _agencyProductWriteRepository.AddAsync(new()
                        {
                            Agency = agency,
                            Product = product,
                            Price = agencyProduct.Price,
                        });
                    }
                }
                else
                    continue;
            }
            await _agencyProductWriteRepository.SaveAsync();
            await _productHubService.AgencyProductUpdatedMessage("Ürünler başarıyla güncellenmiştir.");
            return new()
            {
                Succeeded = true,
                Message = "Ürünler başarıyla güncellenmiştir.",
            };
        }
        public async Task<CreateProductResponse> CreateProduct(CreateProduct createProduct)
        {
            var response=await _productWriteRepository.AddAsync(new()
            {
                PaperType = createProduct.PaperType,
                ColorOption = createProduct.ColorOption,
                PrintType = createProduct.PrintType,
            });
            if (response)
            {
                await _productWriteRepository.SaveAsync();
                await _productHubService.ProductAddedMessage("Ürün başarıyla oluşturulmuşur.");
                return new()
                {
                    Message = "Ürün başarıyla oluşturulmuştur.",
                    Succeeded = true,
                };
            }
            else
            {
                return new()
                {
                    Succeeded = false,
                    Message = "Ürün oluşturulurken bir hata oluştu."
                };
            }
        }

        public async Task<GetAgencyProductsResponse> GetAgencyProducts(GetAgencyProducts getAgencyProduct)
        {
            //Buna gönderilen kağıt tipi renk seçeneği vs parametrelerine göre fiyat dönsün
            var agencyProducts = await _agencyProductReadRepository.GetWhere(u => u.Agency.Id == getAgencyProduct.AgencyId)
                .Include(u => u.Product).Include(u => u.Agency).Select(u => new
                {
                    Id=u.Id,
                    PaperType=u.Product.PaperType,
                    ColorOption = u.Product.ColorOption,
                    PrintType = u.Product.PrintType,
                    Price=u.Price
                })
                .ToListAsync();
            return new()
            {
                AgencyProducts = agencyProducts
            };
        }

        public async Task<ListProducts> GetProducts(int page, int size)
        {
            var totalCount = await _productReadRepository.GetAll().CountAsync();
            var products = await _productReadRepository.GetAll().Skip((page - 1) * size).Take(size).Select(u => new
            {
                Id=u.Id,
                PaperType=u.PaperType,
                ColorOption=u.ColorOption,
                PrintType=u.PrintType,
            }).ToListAsync();
            return new()
            {
                TotalCount = totalCount,
                Products = products,
            };
        }

        public async Task<DeleteProductsResponse> DeleteProducts(List<string> productIds)
        {
            var products=await _productReadRepository.GetWhere(p=> productIds.Contains(p.Id.ToString())).ToListAsync();
            if (products != null && products.Any())
            {
                _productWriteRepository.RemoveRange(products);
                await _productWriteRepository.SaveAsync();
                await _productHubService.ProductDeletedMessage("Seçilen ürünler başarıyla silinmiştir.");
                return new()
                {
                    Succeeded = true,
                    Message = "Seçilen ürünler silindi."
                };
            }
            else
            {
                return new()
                {
                    Succeeded = false,
                    Message = "Silinecek ürün bulunamadı."
                };
            }
            
        }

        public async Task<DeleteAgencyProductsResponse> DeleteAgencyProducts(List<string> agencyProductIds)
        {

            var user = await ContextUser() ;
            if( user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var agencyProductsToRemove=new List<AgencyProduct>();
                if (userRoles.Contains("admin"))
                {
                    agencyProductsToRemove = await _agencyProductReadRepository
                    .GetWhere(u => agencyProductIds.Contains(u.Id.ToString()))
                    .ToListAsync();
                }
                else
                {
                    agencyProductsToRemove = await _agencyProductReadRepository
                    .GetWhere(u => u.Agency.Id == user.Id && agencyProductIds.Contains(u.Id.ToString()))
                    .ToListAsync();
                }
                    
                if (agencyProductsToRemove.Any())
                {
                    _agencyProductWriteRepository.RemoveRange(agencyProductsToRemove);
                    await _agencyProductWriteRepository.SaveAsync();
                    await _productHubService.AgencyProductDeletedMessage("Seçilen ürünler başarıyla silinmiştir.");
                    return new()
                    {
                        Succeeded = true,
                        Message = "Firmanın seçilen ürünleri silinmiştir."
                    };
                }
                else
                {
                    return new()
                    {
                        Succeeded = false,
                        Message = "Firmanın silinecek ürünü bulunamadı."
                    };
                }
            }
            else
            {
                return new()
                {
                    Succeeded = false,
                    Message = "Kullanıcı bulunamadı"
                };
            }


        }

        
    }
}
