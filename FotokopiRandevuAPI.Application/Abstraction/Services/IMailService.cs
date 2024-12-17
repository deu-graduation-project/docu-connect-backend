using FotokopiRandevuAPI.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Abstraction.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string userId, string resetToken, string userName);
        Task SendOrderCreatedForUserMailAsync(string to, string userName, string orderCode, string agencyName, int kopyaSayisi, int sayfaSayisi, decimal price);
        Task SendOrderCreatedForAgencyMailAsync(string to, string username, string ordercode, string agencyname, int kopyasayisi, int sayfasayisi, decimal price);
        Task SendOrderUpdatedMailAsync(string to, string userName, string orderCode, string agencyName, string? compeletedCode, int kopyaSayisi, int sayfaSayisi, decimal price, OrderState orderState);

    }
}
