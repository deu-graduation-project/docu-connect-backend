﻿using FotokopiRandevuAPI.Application.DTOs;
using FotokopiRandevuAPI.Application.DTOs.Comment;
using FotokopiRandevuAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrder(CreateOrder createOrder);
        Task<GetOrdersResponse> GetOrders(int page,int size,string? orderCode,string? search,string? orderby,string? state,string orderId);
        Task<GetSingleOrderResponse> GetSingleOrder(string orderCode);
        Task<UpdateOrderResponse> UpdateOrder(string? orderState, List<string>? removeCommentIds, string? completedCode, string orderCode);
        Task<GetAgencyAnalyticsResponse> GetAgencyAnalytics(string agencyId, string startDate, string endDate, string groupBy = "day");
        Task<CreateOrderResponse> CreateOrderFromWebhookAsync(CreateOrderFromWebhookDto createOrderDto); // Added for webhook
        Task<SucceededMessageResponse> CancelOrderAsync(string orderCode);
        Task<SucceededMessageResponse> CreateComment(CreateComment? createComment);
        Task<GetOrderProductAnalysis> GetOrderProductAnalysis(DateTime startDate, DateTime endDate, string? paperType,string? colorOption,string? printType);
        Task<List<GetAgencyCommentAnalysis>> GetAgencyCommentAnalysis(DateTime startDate, DateTime endDate,string? groupBy);
    }
}
