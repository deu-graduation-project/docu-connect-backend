using System.Collections.Generic;

namespace FotokopiRandevuAPI.Application.DTOs.Order
{
    public class CreateOrderFromWebhookDto
    {
        public string CustomerId { get; set; }
        public string AgencyId { get; set; }
        public string AgencyProductId { get; set; }
        public int KopyaSayisi { get; set; }
        public int ToplamSayfaSayisi { get; set; }
        public decimal TotalPrice { get; set; }
        public string TempDirectoryId { get; set; }
        public List<TempFileInfo> FileInfos { get; set; }
    }

    // Keep this internal structure consistent with WebhooksController and OrderService usage
    public class TempFileInfo
    {
        public string OriginalName { get; set; } // May not be needed for order creation itself
        public string TempName { get; set; }     // Name in the temporary directory
        public string FileCode { get; set; }     // Pre-generated file code
        public int PageCount { get; set; }      // Already calculated
    }
}
