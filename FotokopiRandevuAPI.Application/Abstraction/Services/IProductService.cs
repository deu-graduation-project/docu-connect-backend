using FotokopiRandevuAPI.Application.DTOs.Product;
using FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteProducts;
using FotokopiRandevuAPI.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IProductService
    {
        Task<CreateAgencyProductResponse> CreateAgencyProduct(List<DTOs.Product.CreateAgencyProduct> createAgencyProducts);
        Task<CreateProductResponse> CreateProduct(CreateProduct createProduct);
        Task<DeleteAgencyProductsResponse> DeleteAgencyProducts(List<string> agencyProductIds);
        Task<DeleteProductsResponse> DeleteProducts(List<string> productIds);
        Task<GetAgencyProductsResponse> GetAgencyProducts(GetAgencyProducts getAgencyProduct);
        Task<ListProducts> GetProducts(int page, int size);
    }
}
