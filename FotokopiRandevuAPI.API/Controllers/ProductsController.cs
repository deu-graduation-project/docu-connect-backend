using FotokopiRandevuAPI.Application.Features.Commands.Product.CreateAgencyProduct;
using FotokopiRandevuAPI.Application.Features.Commands.Product.CreateProduct;
using FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteAgencyProducts;
using FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteProducts;
using FotokopiRandevuAPI.Application.Features.Commands.User.CreateUser;
using FotokopiRandevuAPI.Application.Features.Queries.Product.GetAgencyProducts;
using FotokopiRandevuAPI.Application.Features.Queries.Product.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FotokopiRandevuAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProduct(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "agency")]
        public async Task<IActionResult> CreateAgencyProduct(CreateAgencyProductCommandRequest createProductCommandRequest)
        {
            CreateAgencyProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetAgencyProducts([FromQuery]GetAgencyProductsQueryRequest getAgencyProductsQueryRequest)
        {
            GetAgencyProductsQueryResponse response = await _mediator.Send(getAgencyProductsQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "agency")]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQueryRequest getProductsQueryRequest)
        {
            GetProductsQueryResponse response = await _mediator.Send(getProductsQueryRequest);
            return Ok(response);
        }
        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProducts(DeleteProductsCommandRequest deleteProductsCommandRequest)
        {
            DeleteProductsCommandResponse response = await _mediator.Send(deleteProductsCommandRequest);
            return Ok(response);
        }
        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "agency,admin")]
        public async Task<IActionResult> DeleteAgencyProducts(DeleteAgencyProductsCommandRequest deleteAgencyProductsCommandRequest)
        {
            DeleteAgencyProductsCommandResponse response = await _mediator.Send(deleteAgencyProductsCommandRequest);
            return Ok(response);
        }
    }
}
