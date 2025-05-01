using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteAgencyProducts;
using FotokopiRandevuAPI.Application.Features.Commands.User.AssignToRole;
using FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgency;
using FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgencyConfirm;
using FotokopiRandevuAPI.Application.Features.Commands.User.CreateUser;
using FotokopiRandevuAPI.Application.Features.Commands.User.UpdateAgencyInfos;
using FotokopiRandevuAPI.Application.Features.Commands.User.UpdatePassword;
using FotokopiRandevuAPI.Application.Features.Commands.User.UpdateUserPassword;
using FotokopiRandevuAPI.Application.Features.Queries.User.GetAgencies;
using FotokopiRandevuAPI.Application.Features.Queries.User.GetBeAnAgencyRequests;
using FotokopiRandevuAPI.Application.Features.Queries.User.GetSingleAgency;
using FotokopiRandevuAPI.Application.Features.Queries.User.NewFolder;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.AgencyProducts;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using iText.Kernel.Pdf;
using MediatR;
using FotokopiRandevuAPI.Application.Abstraction.Services; // Added for IOrderService
using FotokopiRandevuAPI.Application.DTOs.Order; // Added for CreateOrderFromWebhookDto
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Added for ILogger
using Stripe.Checkout; // Added for Session
using System.Globalization;
using System.Text.Json;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System.Security.Claims; // Added for JsonSerializer

namespace FotokopiRandevuAPI.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager; // Doğru User tipi (Agency/Customer base'i)
        private readonly IAgencyProductReadRepository _agencyProductReadRepository;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService; // Added
        private readonly ILogger<CheckoutController> _logger; 
        readonly IHttpContextAccessor _httpContextAccessor; // Added for accessing HttpContext

        public CheckoutController(
            UserManager<AppUser> userManager,
            IAgencyProductReadRepository agencyProductReadRepository,
            IConfiguration configuration,
            IOrderService orderService, // Added
            ILogger<CheckoutController> logger
,
            IHttpContextAccessor httpContextAccessor) // Added
        {
            _userManager = userManager;
            _agencyProductReadRepository = agencyProductReadRepository;
            _configuration = configuration;
            _orderService = orderService; // Added
            _logger = logger; // Added
            _httpContextAccessor = httpContextAccessor;
        }

        // --- Helper Metodlar (GenerateCode, SeoHelper vs. önceki örnekteki gibi) ---
        // Note: The TempFileInfo class defined here needs to match the one in CreateOrderFromWebhookDto
        // It might be better to move this definition to the DTO file itself if not already done.
        private async Task<string> GenerateFileCode() { /* ... */ await Task.Delay(1); return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(); }
        private string SeoHelper(string text) { /* ... */ return text.ToLowerInvariant().Replace(" ", "-"); }

        // Implementation to get the currently authenticated customer
        private async Task<Domain.Entities.Identity.Extra.Customer?> GetCurrentUserAsync()
        {

            var user2 = _httpContextAccessor?.HttpContext?.User;
            var userIdClaim = _httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogWarning("Could not find user ID claim in HttpContext.");
                return null; // No user claim found
            }

            var user = await _userManager.FindByIdAsync(userIdClaim);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID claim: {UserId}", userIdClaim);
                return null; // User not found in database
            }

            // Attempt to cast to Customer
            var customer = user as Domain.Entities.Identity.Extra.Customer;
            if (customer == null)
            {
                _logger.LogWarning("User found ({UserId}) but is not a Customer.", userIdClaim);
                // Handle cases where the authenticated user might be an Agency or Admin trying to checkout?
                // For now, return null if not a customer.
            }
            return customer;
        }


        // Frontend'den gelen DTO (IFormFile içerir)
        public class InitiatePaymentRequest
        {
            [FromForm(Name = "agencyProductId")] // Form isimleriyle eşleştiğinden emin olun
            public string AgencyProductId { get; set; }

            [FromForm(Name = "agencyId")]
            public string AgencyId { get; set; }

            [FromForm(Name = "kopyaSayisi")]
            public int KopyaSayısı { get; set; }

            [FromForm(Name = "copyFiles")]
            public List<IFormFile> CopyFiles { get; set; } = new List<IFormFile>();
        }

        // Dosya bilgilerini metadata'da saklamak için (basit versiyon)
        private class TempFileInfo
        {
            public string OriginalName { get; set; }
            public string TempName { get; set; } // Sunucudaki geçici adı
            public string FileCode { get; set; }
            public int PageCount { get; set; }
        }


        [HttpPost("initiate-payment")]
        // IFormFile için [FromForm] şart
        public async Task<IActionResult> InitiatePayment([FromForm] InitiatePaymentRequest request)
        {
            // 1. Doğrulamalar
            var agency = await _userManager.FindByIdAsync(request.AgencyId) as Agency;
            var customer = await GetCurrentUserAsync();
            var agencyProduct = await _agencyProductReadRepository.GetByIdAsync(request.AgencyProductId);

            if (agency == null) return BadRequest(new { Message = "Firma bulunamadı." });
            if (customer == null) return Unauthorized(new { Message = "Kullanıcı girişi gerekli." });
            if (agencyProduct == null) return BadRequest(new { Message = "Ürün bulunamadı." });
            if (request.CopyFiles == null || !request.CopyFiles.Any()) return BadRequest(new { Message = "Dosya yüklenmedi." });
            if (request.KopyaSayısı <= 0) return BadRequest(new { Message = "Kopya sayısı geçersiz." });

            // 2. Dosyaları Geçici Olarak İşle ve Kaydet
            // Geçici dosyaları saklamak için benzersiz bir klasör
            var temporaryDirectoryId = Guid.NewGuid().ToString("N");
            // Güvenlik notu: Bu path'in wwwroot dışında olması daha iyi olabilir ama erişim izni yönetimi gerekir.
            // Şimdilik wwwroot altında tutalım, ama canlıda gözden geçirin.
            string tempPathBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp_files");
            string tempDirectoryPath = Path.Combine(tempPathBase, temporaryDirectoryId);

            List<TempFileInfo> processedFilesInfo = new List<TempFileInfo>();
            int toplamSayfaSayısı = 0;

            try
            {
                Directory.CreateDirectory(tempDirectoryPath); // Geçici klasörü oluştur

                foreach (var file in request.CopyFiles)
                {
                    if (file.Length == 0) continue;

                    FileInfo fileInfo = new FileInfo(file.FileName);
                    var fileCode = await GenerateFileCode();
                    string tempFileName = $"{Guid.NewGuid():N}{fileInfo.Extension}"; // Çakışmayı önlemek için rastgele isim
                    string tempFilePath = Path.Combine(tempDirectoryPath, tempFileName);

                    // Dosyayı geçici konuma kaydet
                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Sayfa sayısını oku
                    int sayfaSayısı = 0;
                    try
                    {
                        using (var pdfStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                        {
                            var pdfReader = new PdfReader(pdfStream);
                            var pdfDocument = new PdfDocument(pdfReader);
                            sayfaSayısı = pdfDocument.GetNumberOfPages();
                            pdfDocument.Close();
                            pdfReader.Close();
                        }
                    }
                    catch (Exception pdfEx)
                    {
                        // PDF okuma hatası -> işlemi durdur, geçici klasörü temizle
                        Directory.Delete(tempDirectoryPath, true); // recursive delete
                        Console.WriteLine($"PDF okuma hatası ({file.FileName}): {pdfEx.Message}");
                        return BadRequest(new { Message = $"'{file.FileName}' geçerli bir PDF dosyası değil veya okunamadı." });
                    }

                    if (sayfaSayısı == 0)
                    {
                        // Boş PDF? -> işlemi durdur, geçici klasörü temizle
                        Directory.Delete(tempDirectoryPath, true);
                        return BadRequest(new { Message = $"'{file.FileName}' dosyasının sayfa sayısı 0." });
                    }


                    toplamSayfaSayısı += sayfaSayısı;
                    processedFilesInfo.Add(new TempFileInfo
                    {
                        OriginalName = file.FileName, // Belki lazım olur?
                        TempName = tempFileName,      // Webhook'ta bulmak için
                        FileCode = fileCode,
                        PageCount = sayfaSayısı
                    });
                }

                if (!processedFilesInfo.Any()) // Hiç geçerli dosya işlenmediyse
                {
                    Directory.Delete(tempDirectoryPath, true);
                    return BadRequest(new { Message = "İşlenecek geçerli dosya bulunamadı." });
                }


                // 3. Fiyat Hesapla
                decimal totalPrice = (decimal)agencyProduct.Price * request.KopyaSayısı * toplamSayfaSayısı;

                // 4. Stripe Checkout Session Oluştur
                var domain = $"{Request.Scheme}://{Request.Host}";
                var metadata = new Dictionary<string, string>
            {
                // Siparişi oluşturmak için GEREKLİ TÜM BİLGİLER:
                { "customer_id", customer.Id },
                { "agency_id", agency.Id },
                { "agency_product_id", agencyProduct.Id.ToString() },
                { "kopya_sayisi", request.KopyaSayısı.ToString() },
                { "toplam_sayfa_sayisi", toplamSayfaSayısı.ToString() },
                 // Fiyatı string olarak sakla (kuruş değil, gerçek değer)
                { "total_price", totalPrice.ToString(CultureInfo.InvariantCulture) },
                // Geçici dosya konumunu ve dosyaların bilgilerini metadata'ya ekle
                { "temp_directory_id", temporaryDirectoryId },
                 // Dosya bilgilerini JSON string olarak saklayalım (limitlere dikkat!)
                { "file_info_json", System.Text.Json.JsonSerializer.Serialize(processedFilesInfo) }
            };

                // Metadata limitlerini kontrol et (Stripe: 50 anahtar, anahtar<40 char, değer<500 char)
                // file_info_json çok büyürse, bu yaklaşım çalışmaz. O zaman dosya bilgilerini
                // temp_directory_id altındaki bir info dosyasına yazıp sadece directory ID'yi saklamak gerekir.
                if (metadata["file_info_json"].Length > 500)
                {
                    Directory.Delete(tempDirectoryPath, true); // Temizle
                    Console.WriteLine("Metadata 'file_info_json' değeri 500 karakteri aştı.");
                    return StatusCode(500, new { Message = "Sipariş verisi Stripe'a sığdırılamadı. Dosya sayısı/isimleri çok fazla olabilir." });
                }


                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "try",
                            UnitAmountDecimal = totalPrice * 100, // Kuruş cinsinden
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Kopya Siparişi - {agency.AgencyName}", // Daha genel bir isim
                                Description = $"{request.KopyaSayısı} Kopya, {toplamSayfaSayısı} Toplam Sayfa"
                            },
                        },
                        Quantity = 1,
                    }
                },
                    Mode = "payment",
                    SuccessUrl = $"http://localhost:3000/order/success?session_id={{CHECKOUT_SESSION_ID}}", // Başarı sonrası yönlendirme
                    CancelUrl = $"http://localhost:3000/cart?payment_canceled=true", // İptal sonrası yönlendirme
                    Metadata = metadata, // TÜM BİLGİ BURADA
                    CustomerEmail = customer.Email,
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                // --- LOCAL TESTING ONLY: Simulate Webhook Call ---
                // WARNING: Do NOT use this block in production. Payment is NOT confirmed here.
                _logger.LogWarning("LOCAL TESTING ONLY: Simulating webhook call immediately after session creation for Session ID: {SessionId}", session.Id);
                try
                {
                    // Use variables directly from the current scope instead of re-parsing metadata
                    var createOrderDto = new CreateOrderFromWebhookDto
                    {
                        CustomerId = customer.Id, // Use customer object from scope
                        AgencyId = agency.Id,     // Use agency object from scope
                        AgencyProductId = agencyProduct.Id.ToString(), // Use agencyProduct object from scope
                        KopyaSayisi = request.KopyaSayısı, // Use request object from scope
                        ToplamSayfaSayisi = toplamSayfaSayısı, // Use calculated value from scope
                        TotalPrice = totalPrice, // Use calculated value from scope
                        TempDirectoryId = temporaryDirectoryId, // Use generated value from scope
                        FileInfos = processedFilesInfo.Select(f => new Application.DTOs.Order.TempFileInfo // Use DTO namespace
                        {
                            OriginalName = f.OriginalName,
                            TempName = f.TempName,
                            FileCode = f.FileCode,
                            PageCount = f.PageCount
                        }).ToList()
                    };

                    _logger.LogInformation("LOCAL TESTING: Calling OrderService.CreateOrderFromWebhookAsync for Session ID: {SessionId}", session.Id);
                    var orderResult = await _orderService.CreateOrderFromWebhookAsync(createOrderDto);

                    if (orderResult.Succeeded)
                    {
                        _logger.LogInformation("LOCAL TESTING: Simulated order creation successful for Session ID: {SessionId}. Message: {Message}", session.Id, orderResult.Message);
                    }
                    else
                    {
                        _logger.LogError("LOCAL TESTING: Simulated order creation failed for Session ID: {SessionId}. Message: {Message}", session.Id, orderResult.Message);
                        // Note: Temporary files might be left behind in this simulated failure scenario.
                    }
                }
                catch (Exception simEx)
                {
                    _logger.LogError(simEx, "LOCAL TESTING: Error during simulated order creation for Session ID: {SessionId}", session.Id);
                }
                // --- END LOCAL TESTING ONLY ---


                // 5. Session ID'yi Frontend'e Gönder (as before)
                return Ok(new { sessionId = session.Id });

            }
            catch (StripeException e)
            {
                // Hata oluşursa geçici klasörü silmeyi dene
                if (Directory.Exists(tempDirectoryPath)) Directory.Delete(tempDirectoryPath, true);
                Console.WriteLine($"Stripe API Hatası: {e.StripeError?.Message ?? e.Message}");
                return BadRequest(new { Message = $"Ödeme başlatılamadı: {e.StripeError?.Message ?? e.Message}" });
            }
            catch (Exception ex)
            {
                // Genel hata, geçici klasörü sil
                if (Directory.Exists(tempDirectoryPath)) Directory.Delete(tempDirectoryPath, true);
                _logger.LogError(ex, "Unhandled exception during payment initiation."); // Log the actual exception
                Console.WriteLine($"Ödeme başlatma hatası: {ex}"); // Keep console log if desired
                return StatusCode(500, new { Message = "Ödeme işlemi başlatılırken beklenmedik bir hata oluştu." });
            }
        }
    }
}
