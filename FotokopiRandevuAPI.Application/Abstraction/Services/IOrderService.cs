using FotokopiRandevuAPI.Application.DTOs.Comment;
using FotokopiRandevuAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrder(CreateOrder createOrder);
        Task<GetOrdersResponse> GetOrders(int page,int size,string? orderCode,string? search,string? orderby,string? state,string orderId);
        Task<GetSingleOrderResponse> GetSingleOrder(string orderCode);
        Task<UpdateOrderResponse> UpdateOrder(string? orderState,CreateComment? createComment, List<string>? removeCommentIds, string? completedCode);
        Task<GetAgencyAnalyticsResponse> GetAgencyAnalytics(string agencyId, string startDate, string endDate, string groupBy = "day");
    }
}
