﻿using FotokopiRandevuAPI.Application.Features.Commands.Order.CancelOrder;
using FotokopiRandevuAPI.Application.Features.Commands.Order.CreateComment;
using FotokopiRandevuAPI.Application.Features.Commands.Order.CreateOrder;
using FotokopiRandevuAPI.Application.Features.Commands.Order.UpdateOrder;
using FotokopiRandevuAPI.Application.Features.Commands.Product.CreateProduct;
using FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyAnalytics;
using FotokopiRandevuAPI.Application.Features.Queries.Order.GetAgencyCommentAnalysis;
using FotokopiRandevuAPI.Application.Features.Queries.Order.GetOrderProductAnalysis;
using FotokopiRandevuAPI.Application.Features.Queries.Order.GetOrders;
using FotokopiRandevuAPI.Application.Features.Queries.Order.GetSingleOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FotokopiRandevuAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateOrder([FromForm]CreateOrderCommandRequest createOrderCommandRequest)
        {
            CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQueryRequest getOrdersQueryRequest)
        {
            GetOrdersQueryResponse response = await _mediator.Send(getOrdersQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetSingleOrder([FromQuery] GetSingleOrderQueryRequest getSingleOrderQueryRequest)
        {
            GetSingleOrderQueryResponse response = await _mediator.Send(getSingleOrderQueryRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "customer,admin,agency")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommandRequest updateOrderCommandRequest)
        {
            UpdateOrderCommandResponse response = await _mediator.Send(updateOrderCommandRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin,agency")]
        public async Task<IActionResult> GetAgencyAnalytics([FromQuery] GetAgencyAnalyticsCommandRequest getAgencyAnalyticsCommandRequest)
        {
            GetAgencyAnalyticsCommandResponse response = await _mediator.Send(getAgencyAnalyticsCommandRequest);
            return Ok(response);
        }
        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> CancelOrder(CancelOrderCommandRequest request)
        {
            CancelOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateComment(CreateCommentCommandRequest request)
        {
            CreateCommentCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin,agency")]
        public async Task<IActionResult> GetOrderProductAnalysis([FromQuery] GetOrderProductAnalysisCommandRequest request)
        {
            GetOrderProductAnalysisCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin,agency")]
        public async Task<IActionResult> GetAgencyCommentAnalysis([FromQuery] GetAgencyCommentAnalysisCommandRequest request)
        {
            GetAgencyCommentAnalysisCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
