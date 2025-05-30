﻿using Azure;
using FotokopiRandevuAPI.Application.Abstraction.Hubs;
using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.DTOs;
using FotokopiRandevuAPI.Application.DTOs.Comment;
using FotokopiRandevuAPI.Application.DTOs.Order;
using FotokopiRandevuAPI.Application.Repositories.CommentRepositories;
using FotokopiRandevuAPI.Application.Repositories.FileRepositories;
using FotokopiRandevuAPI.Application.Repositories.OrderRepositories;
using FotokopiRandevuAPI.Application.Repositories.ProductRepositories.AgencyProducts;
using FotokopiRandevuAPI.Domain.Entities.Files;
using FotokopiRandevuAPI.Domain.Entities.Identity;
using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using FotokopiRandevuAPI.Domain.Entities.Order;
using FotokopiRandevuAPI.Domain.Entities.Products;
using FotokopiRandevuAPI.Persistence.Repositories.OrderRepositories;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence.Services
{
    public class OrderService:IOrderService
    {
        readonly IOrderReadRepository _orderReadRepository;
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly UserManager<AppUser> _userManager;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IAgencyProductReadRepository _agencyProductReadRepository;
        readonly ICommentWriteRepository _commentWriteRepository;
        readonly ICommentReadRepository _commentReadRepository;
        readonly IOrderHubService _orderHubService;
        readonly IFileReadRepository _fileReadRepository;
        readonly IUserService _userService;
        readonly IMailService _mailService;
        public OrderService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IAgencyProductReadRepository agencyProductReadRepository, IFileWriteRepository fileWriteRepository, ICommentWriteRepository commentWriteRepository, ICommentReadRepository commentReadRepository, IOrderHubService orderHubService, IFileReadRepository fileReadRepository, IUserService userService, IMailService mailService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _agencyProductReadRepository = agencyProductReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _commentReadRepository = commentReadRepository;
            _orderHubService = orderHubService;
            _fileReadRepository = fileReadRepository;
            _userService = userService;
            _mailService = mailService;
        }
        private string SeoHelper(string text)
        {
            text = text.Replace("ş", "s");
            text = text.Replace("Ş", "s");
            text = text.Replace("I", "i");
            text = text.Replace("İ", "i");
            text = text.Replace("Ö", "o");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "u");
            text = text.Replace("ü", "u");
            text = text.Replace("Ç", "c");
            text = text.Replace("ç", "c");
            text = text.Replace("Ğ", "g");
            text = text.Replace("ğ", "g");
            text = text.Replace(" ", "-");
            text = text.Replace("---", "-");
            text = text.Replace("?", "");
            text = text.Replace("/", "");
            text = text.Replace("'", "");
            text = text.Replace("#", "");
            text = text.Replace("%", "");
            text = text.Replace(".", "");
            text = text.Replace("&", "");
            text = text.Replace("*", "");
            text = text.Replace("!", "");
            text = text.Replace("@", "");
            text = text.Replace("+", "");
            text = text.ToLower();
            text = text.Trim();
            string endocededUrl = (text ?? "").ToLower();
            endocededUrl = Regex.Replace(endocededUrl, @"&+", "and");
            endocededUrl = endocededUrl.Replace("'", "");
            endocededUrl = Regex.Replace(endocededUrl, @"[^a-z0-9]","-");
            endocededUrl = Regex.Replace(endocededUrl, @"-+", "-");
            endocededUrl = endocededUrl.Trim('-');

            return endocededUrl;
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
        private async Task<string> GenerateOrderCode()
        {
            string newCode;
            bool exist;
            do
            {
                newCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                exist = await _orderReadRepository.GetWhere(u => u.OrderCode == newCode).AnyAsync();
            }
            while (exist);
            return newCode;
        }
        private async Task<string> GenerateFileCode()
        {
            string newCode;
            bool exist;
            do
            {
                newCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                exist = await _fileReadRepository.GetWhere(u => u.FileCode == newCode).AnyAsync();
            }
            while (exist);
            return newCode;
        }
        private async Task<string> GenerateOrderCompletedCode()
        {
            string newCode;
            bool exist;
            do
            {
                newCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                exist = await _orderReadRepository.GetWhere(u => u.CompletedCode == newCode).AnyAsync();
            }
            while (exist);
            return newCode;
        }
        public async Task<CreateOrderResponse> CreateOrder(CreateOrder createOrder)
        {
            var agency = await _userManager.FindByIdAsync(createOrder.AgencyId) as Agency;
            var customer = await ContextUser() as Customer;
            var agencyProduct = await _agencyProductReadRepository.GetByIdAsync(createOrder.AgencyProductId);
            

            if (agency == null)
                return new()
                {
                    Succeeded = false,
                    Message = "Firma bulunamadı."
                };

            if (agencyProduct == null)
                return new()
                {
                    Succeeded = false,
                    Message = "Ürün bulunamadı."
                };

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            var orderId = Guid.NewGuid();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            int toplamSayfaSayısı = 0;
            List<CopyFile> copyFiles = new List<CopyFile>();

            foreach (var file in createOrder.CopyFiles)
            {
                FileInfo fileInfo = new FileInfo(file.FileName);
                var fileCode = await GenerateFileCode();
                string fileName = SeoHelper(Path.GetFileNameWithoutExtension(file.FileName))+"-"+fileCode+ fileInfo.Extension;
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                int sayfaSayısı = 0;
                using (var pdfStream = new FileStream(fileNameWithPath, FileMode.Open))
                {
                    PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfStream));
                    sayfaSayısı = pdfDocument.GetNumberOfPages();
                }

                toplamSayfaSayısı += sayfaSayısı;

                var copyFile = new CopyFile
                {
                    FilePath = fileNameWithPath,
                    FileName = fileName,
                    OrderId = orderId,
                    FileCode = fileCode,
                };

                copyFiles.Add(copyFile);
            }
            var totalPrice = agencyProduct.Price * createOrder.KopyaSayısı * toplamSayfaSayısı;
            var orderCode = await GenerateOrderCode();
            var order = new Order
            {
                Agency = agency,
                Customer = customer,
                OrderCode = orderCode,
                KopyaSayısı = createOrder.KopyaSayısı,
                TotalPrice = totalPrice,
                OrderState = OrderState.Pending,
                AgencyProduct = agencyProduct,
                SayfaSayısı=toplamSayfaSayısı,
                CopyFiles = copyFiles,
                UpdatedDate=DateTime.UtcNow
            };

            var response = await _orderWriteRepository.AddAsync(order);

            if (response)
            {
                await _orderWriteRepository.SaveAsync();
                await _mailService.SendOrderCreatedForAgencyMailAsync(agency.Email, customer.UserName, orderCode, agency.AgencyName, createOrder.KopyaSayısı, toplamSayfaSayısı, totalPrice);
                await _mailService.SendOrderCreatedForUserMailAsync(customer.Email, customer.UserName, orderCode, agency.AgencyName, createOrder.KopyaSayısı, toplamSayfaSayısı, totalPrice);
                await _orderHubService.OrderAddedMessage(agency.Id, customer.Id, "Sipariş firma onayı için gönderilmiştir. Siparişlerim sayfasından takip edebilirsiniz.");                             
                return new()
                {
                    Message = "Sipariş firma onayı için gönderilmiştir. Siparişlerim sayfasından takip edebilirsiniz.",
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

        public async Task<GetOrdersResponse> GetOrders(int page, int size, string? orderCode, string? search, string? orderby, string? state, string orderId)
        {
            //Null point hatasına bak.
            var user=await ContextUser();
            if (user == null)
                throw new Exception("Siparişler alınırken bir hata ile karşılaşıldı.");
            var userRoles= await _userManager.GetRolesAsync(user);
            var query= _orderReadRepository.GetWhere(u=>u.Agency==user || u.Customer==user).Include(u => u.Customer).Include(u => u.Agency).AsQueryable();
            object orders;
            if (userRoles.Contains("admin"))
                query = _orderReadRepository.GetAll(false).Include(u => u.Customer).Include(u => u.Agency).AsQueryable();

            if (!string.IsNullOrEmpty(orderCode))
            {
                query = query.Where(u => u.OrderCode == orderCode);
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                query = query.Where(u => u.Id.ToString() == orderId);
            }
            if (!string.IsNullOrEmpty(search))
            {
                var loweredSearch = search.ToLower();
                query = query
                    .Where(u => u.Agency.AgencyName.ToLower() == loweredSearch || u.Customer.UserName.ToLower()== loweredSearch || 
                    u.Agency.UserName.ToLower()==loweredSearch || u.Agency.Name.ToLower()==loweredSearch);
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (Enum.TryParse<OrderState>(state, true, out var requestState))
                {
                    query = query.Where(u => u.OrderState == requestState);
                }
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
            if (userRoles.Contains("agency"))
            {
                orders= await query.Skip((page - 1) * size).Take(size).Select(u => new
                {
                    OrderCode=u.OrderCode,
                    AgencyName=u.Agency.AgencyName,
                    CustomerUserName=u.Customer.UserName,
                    TotalPrice=u.TotalPrice,
                    SayfaSayısı=u.SayfaSayısı,
                    KopyaSayısı=u.KopyaSayısı,
                    //CommentText = u.Comment != null ? u.Comment.CommentText : null,
                    //CommentStarRating = u.Comment != null ? (int?)u.Comment.StarRating : null,
                    //CommentCreatedDate = u.Comment != null ? (DateTime?)u.Comment.CreatedDate : null,
                    CreatedDate =u.CreatedDate,
                    OrderState=u.OrderState,
                    ProductPaperType = u.AgencyProduct.Product.PaperType,
                    ProductColorOption = u.AgencyProduct.Product.ColorOption,
                    ProductPrintType = u.AgencyProduct.Product.PrintType,
                    PricePerPage = u.AgencyProduct.Price,
                    CopyFile = u.CopyFiles.Select(c => new
                    {
                        FileName=c.FileName,
                        FileCode=c.FileCode,
                    }),
                    UpdatedDate =u.UpdatedDate,
                }).ToListAsync();
            }
            else if (userRoles.Contains("customer"))
            {
                orders = await query.Skip((page - 1) * size).Include(u=>u.AgencyProduct).Take(size).Select(u => new
                {
                    OrderCode = u.OrderCode,
                    AgencyName = u.Agency.AgencyName,
                    CustomerUserName = u.Customer.UserName,
                    TotalPrice = u.TotalPrice,
                    SayfaSayısı = u.SayfaSayısı,
                    KopyaSayısı = u.KopyaSayısı,
                    //CommentText = u.Comment != null ? u.Comment.CommentText : null,
                    //CommentStarRating = u.Comment != null ? (int?)u.Comment.StarRating : null,
                    //CommentCreatedDate = u.Comment != null ? (DateTime?)u.Comment.CreatedDate : null,
                    CreatedDate = u.CreatedDate,
                    OrderState = u.OrderState,
                    ProductPaperType=u.AgencyProduct.Product.PaperType,
                    ProductColorOption = u.AgencyProduct.Product.ColorOption,
                    ProductPrintType = u.AgencyProduct.Product.PrintType,
                    PricePerPage= u.AgencyProduct.Price,
                    CompletedCode = u.CompletedCode,
                    CopyFile = u.CopyFiles.Select(c => new
                    {
                        FileName = c.FileName,
                        FileCode=c.FileCode
                    }),
                    UpdatedDate = u.UpdatedDate,
                }).ToListAsync();
            }
            else if (userRoles.Contains("admin"))
            {
                orders = await query.Skip((page - 1) * size).Take(size).Select(u => new
                {
                    OrderId = u.Id,
                    OrderCode = u.OrderCode,
                    AgencyName = u.Agency.AgencyName,
                    CustomerUserName = u.Customer.UserName,
                    Price = u.TotalPrice,
                    SayfaSayısı = u.SayfaSayısı,
                    KopyaSayısı = u.KopyaSayısı,
                    //CommentText = u.Comment != null ? u.Comment.CommentText : null,
                    //CommentStarRating = u.Comment != null ? (int?)u.Comment.StarRating : null,
                    //CommentCreatedDate = u.Comment != null ? (DateTime?)u.Comment.CreatedDate : null,
                    CreatedDate = u.CreatedDate,
                    OrderState = u.OrderState,
                    ProductPaperType = u.AgencyProduct.Product.PaperType,
                    ProductColorOption = u.AgencyProduct.Product.ColorOption,
                    ProductPrintType = u.AgencyProduct.Product.PrintType,
                    PricePerPage = u.AgencyProduct.Price,
                    CompletedCode = u.CompletedCode,
                    CopyFile = u.CopyFiles.Select(c => new
                    {
                        FileName = c.FileName,
                        FileCode = c.FileCode,
                    }),
                    UpdatedDate = u.UpdatedDate,
                }).ToListAsync();
            }
            else
            {
                throw new Exception("Siparişler alınırken bir hata ile karşılaşıldı.");
            }
            return new()
            {
                Orders = orders
            };
        }

        public async Task<UpdateOrderResponse> UpdateOrder(string? orderState,List<string>? removeCommentIds,string? completedCode,string orderCode)
        {
            var order = await _orderReadRepository.GetWhere(u => u.OrderCode == orderCode)
                .Include(u => u.Agency).Include(u => u.Customer).Include(u=>u.Comment).FirstOrDefaultAsync();
            if (order == null)
            {
                return new()
                {
                    Succeeded = false,
                    Message = "Sipariş bulunamadı."
                };
            }
            var user = await ContextUser();
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("agency"))
            {
                if (order.Agency != user)
                    return new()
                    {
                        Succeeded = false,
                        Message = "Bu sipariş size ait değil."
                    };
                if (order.OrderState == OrderState.Rejected)
                    return new()
                    {
                        Succeeded = false,
                        Message = "Sipariş halihazırda reddedilmiş durumdadır. Bu yüzden sipariş durumu değiştirilemez."
                    };
                if (!string.IsNullOrEmpty(orderState))
                {
                    if (Enum.TryParse<OrderState>(orderState, true, out var state))
                    {
                        if (state == OrderState.Completed)
                        {
                            if (string.IsNullOrEmpty(completedCode))
                                throw new Exception("Siparişi tamamlamak için doğrulama kodunu girmeniz gerekmektedir.");
                            if(order.OrderState!=OrderState.Finished)
                                throw new Exception("Doğrulama kodunu girmek için önce siparişin bitmiş olması gerekmektedir.");
                            if (order.CompletedCode != completedCode)
                                throw new Exception("Doğrulama kodunuz doğru değil lütfen kontrol edip tekrar deneyiniz.");
                        }
                        if (state == OrderState.Finished)
                            order.CompletedCode = await GenerateOrderCompletedCode();
                        await _mailService.SendOrderUpdatedMailAsync(order.Customer.Email, order.Customer.UserName, order.OrderCode, order.Agency.AgencyName, order.CompletedCode, order.KopyaSayısı, order.SayfaSayısı, order.TotalPrice, state);
                        order.OrderState = state;
                        order.UpdatedDate = DateTime.UtcNow;
                        await _orderWriteRepository.SaveAsync();
                        await _orderHubService.OrderUpdatedMessage(order.Agency.Id, order.Customer.Id, $"{order.OrderCode} kodlu sipariş güncellenmiştir.");
                        return new()
                        {
                            Succeeded = true,
                            Message = "Sipariş durumu başarıyla güncellenmiştir."
                        };
                    }
                    else
                    {
                        return new()
                        {
                            Succeeded = false,
                            Message = "Sipariş durumu doğru bir şekilde gönderilmedi."
                        };
                    }
                }
            }

            else if (userRoles.Contains("admin"))
            {
                if (!string.IsNullOrEmpty(orderState))
                {
                    if (Enum.TryParse<OrderState>(orderState, true, out var state))
                    {
                        order.OrderState = state;
                        order.UpdatedDate = DateTime.UtcNow;
                        if (state == OrderState.Finished)
                            order.CompletedCode = await GenerateOrderCompletedCode();
                        await _orderWriteRepository.SaveAsync();
                    }
                    else
                    {
                        return new()
                        {
                            Succeeded = false,
                            Message = "Sipariş durumu doğru bir şekilde gönderilmedi."
                        };
                    }
                }
                if(removeCommentIds != null && removeCommentIds.Any()&& order.Comment!=null)
                {
                    var removeComments = new List<Domain.Entities.Order.Comment>();
                    removeComments = await _commentReadRepository.GetWhere(c=>removeCommentIds.Contains(c.Id.ToString())).ToListAsync();
                    _commentWriteRepository.RemoveRange(removeComments);
                    await _commentWriteRepository.SaveAsync();
                    await _userService.UpdateStarRating(order.Agency.Id.ToString());
                    await _orderHubService.OrderUpdatedMessage(order.Agency.Id, order.Customer.Id, $"{order.OrderCode} kodlu sipariş güncellenmiştir.");
                    return new()
                    {
                        Succeeded = true,
                        Message = "Sipariş başarıyla güncellenmiştir."
                    };
                }
            }
            return new()
            {
                Succeeded = false,
                Message = "Siparişi güncellenirken bir hata ile karşılaşıldı."
            };
        }

        public async Task<GetSingleOrderResponse> GetSingleOrder(string orderCode)
        {
            var user = await ContextUser();
            var userRoles = await _userManager.GetRolesAsync(user);
            var order= await _orderReadRepository.GetSingleAsync(u=>u.OrderCode==orderCode);
            if (order == null)
                throw new Exception("Sipariş bulunamadı.");
            var file = await _fileReadRepository.GetWhere(u => u.OrderId == order.Id).ToListAsync();
            if(file == null)
                throw new Exception("Siparişin dosyası bulunamadı.");
            object selectedOrder;
            if (userRoles.Contains("admin"))
            {
                selectedOrder = await _orderReadRepository.GetWhere(u => u.OrderCode == orderCode).Include(u => u.Agency).Include(u => u.Customer)
               .Select(u => new
               {
                   OrderId=u.Id,
                   OrderCode = u.OrderCode,
                   AgencyName = u.Agency.AgencyName,
                   CustomerName = u.Customer.UserName,
                   TotalPrice = u.TotalPrice,
                   PricePerPage = u.AgencyProduct.Price,
                   TotalPage = u.SayfaSayısı,
                   PrintType = u.AgencyProduct.Product.PrintType,
                   PaperType = u.AgencyProduct.Product.PaperType,
                   ColorOption = u.AgencyProduct.Product.ColorOption,
                   KopyaSayısı = u.KopyaSayısı,
                   CreatedDate = u.CreatedDate,
                   OrderState = u.OrderState,
                   CopyFiles = u.CopyFiles.Select(c => new
                   {
                       FileName = c.FileName,
                       FileCode = c.FileCode,
                       FilePath=c.FilePath
                   }),
                   UpdatedDate=u.UpdatedDate,
                   CompletedCode = u.CompletedCode
               }).FirstOrDefaultAsync();
            }
            else if (userRoles.Contains("customer"))
            {
                selectedOrder = await _orderReadRepository.GetWhere(u => u.OrderCode == orderCode).Include(u => u.Agency).Include(u => u.Customer)
               .Select(u => new
               {
                   OrderCode = u.OrderCode,
                   AgencyName = u.Agency.AgencyName,
                   CustomerName = u.Customer.UserName,
                   TotalPrice = u.TotalPrice,
                   PricePerPage = u.AgencyProduct.Price,
                   TotalPage = u.SayfaSayısı,
                   PrintType = u.AgencyProduct.Product.PrintType,
                   PaperType = u.AgencyProduct.Product.PaperType,
                   ColorOption = u.AgencyProduct.Product.ColorOption,
                   KopyaSayısı = u.KopyaSayısı,
                   CreatedDate = u.CreatedDate,
                   OrderState = u.OrderState,
                   CopyFiles = u.CopyFiles.Select(c => new
                   {
                       FileName = c.FileName,
                       FileCode=c.FileCode
                   }),
                   CompletedCode = u.CompletedCode,
                   UpdatedDate = u.UpdatedDate,
               }).FirstOrDefaultAsync();
            }
            else if (userRoles.Contains("agency"))
            {
                selectedOrder = await _orderReadRepository.GetWhere(u => u.OrderCode == orderCode).Include(u => u.Agency).Include(u => u.Customer)
               .Select(u => new
               {
                   OrderCode = u.OrderCode,
                   AgencyName = u.Agency.AgencyName,
                   CustomerName = u.Customer.UserName,
                   TotalPrice = u.TotalPrice,
                   PricePerPage = u.AgencyProduct.Price,
                   TotalPage = u.SayfaSayısı,
                   PrintType = u.AgencyProduct.Product.PrintType,
                   PaperType = u.AgencyProduct.Product.PaperType,
                   ColorOption = u.AgencyProduct.Product.ColorOption,
                   KopyaSayısı = u.KopyaSayısı,
                   CreatedDate = u.CreatedDate,
                   OrderState = u.OrderState,
                   CopyFiles = u.CopyFiles.Select(c => new
                   {
                       FileName = c.FileName,
                       FileCode = c.FileCode
                   }),
                   UpdatedDate = u.UpdatedDate,
               }).FirstOrDefaultAsync();
            }
            else
                throw new Exception("Sipariş alınırken bir hata ile karşılaşıldı");
            if(selectedOrder == null)
                throw new Exception("Sipariş alınırken bir hata ile karşılaşıldı");
            return new()
            {
                Order = selectedOrder
            };
            
        }

        public async Task<GetAgencyAnalyticsResponse> GetAgencyAnalytics(string agencyId, string startDate, string endDate, string groupBy = "day")
        {
            var user = await ContextUser();
            if (user == null)
                throw new Exception("Analizler alınırken bir hata ile karşılaşıldı.");
            var userRoles = await _userManager.GetRolesAsync(user);
            Agency agency;
            List<Order> agencyOrders = new List<Order>();
            DateTime parsedStartDate, parsedEndDate;
            try
            {
                parsedStartDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                parsedEndDate = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new Exception("Tarih formatı doğru bir şekilde gönderilmedi.");
            }
            parsedStartDate = DateTime.SpecifyKind(parsedStartDate, DateTimeKind.Utc);
            parsedEndDate = DateTime.SpecifyKind(parsedEndDate, DateTimeKind.Utc);
            if (userRoles.Contains("admin"))
            {
                agency = await _userManager.Users.OfType<Agency>().Where(u => u.IsConfirmedAgency).FirstOrDefaultAsync(u => u.Id == agencyId);
                agencyOrders = await _orderReadRepository
                    .GetWhere(u => u.Agency.Id == agencyId && u.CreatedDate >= parsedStartDate && u.CreatedDate <= parsedEndDate)
                    .Include(u => u.Agency).ToListAsync();
            }
            else
            {
                agency = user as Agency;
                if(!agency.IsConfirmedAgency)
                    throw new Exception("Böyle bir onaylı firma bulunamadı.");
                agencyOrders = await _orderReadRepository
                .GetWhere(u => u.Agency.Id == agency.Id && u.CreatedDate >= parsedStartDate && u.CreatedDate <= parsedEndDate)
                .Include(u => u.Agency).ToListAsync();
            }
            if (agency==null)
                throw new Exception("Böyle bir onaylı firma bulunamadı.");
            if (agencyOrders.Any())
            {
                var analytics = groupBy.ToLower() switch
                {
                    "month" => agencyOrders
                        .GroupBy(o => new { o.CreatedDate.Year, o.CreatedDate.Month })
                        .Select(g => new
                        {
                            Period = $"{g.Key.Month:D2}.{g.Key.Year}",
                            TotalPrice = g.Sum(o => o.TotalPrice),
                            TotalPageCount = g.Sum(o => o.SayfaSayısı),
                            TotalCompletedOrder = g.Count(o => o.OrderState == OrderState.Completed)
                        }),
                    "year" => agencyOrders
                        .GroupBy(o => o.CreatedDate.Year)
                        .Select(g => new
                        {
                            Period = $"{g.Key}",
                            TotalPrice = g.Sum(o => o.TotalPrice),
                            TotalPageCount = g.Sum(o => o.SayfaSayısı),
                            TotalCompletedOrder = g.Count(o => o.OrderState == OrderState.Completed)
                        }),
                    _ => agencyOrders
                        .GroupBy(o => o.CreatedDate.Date)
                        .Select(g => new
                        {
                            Period = $"{g.Key:dd.MM.yyyy}",
                            TotalPrice = g.Sum(o => o.TotalPrice),
                            TotalPageCount = g.Sum(o => o.SayfaSayısı),
                            TotalCompletedOrder = g.Count(o => o.OrderState == OrderState.Completed)
                        })
                };
                return new()
                {
                    AgencyAnalytics = analytics
                };
            }
            else
                throw new Exception("Bu firmanın bir siparişi bulunmamaktadır.");
        }

        // New method to handle order creation from webhook after successful payment
        public async Task<CreateOrderResponse> CreateOrderFromWebhookAsync(CreateOrderFromWebhookDto createOrderDto)
        {
            // 1. Retrieve Entities
            var agency = await _userManager.FindByIdAsync(createOrderDto.AgencyId) as Agency;
            var customer = await _userManager.FindByIdAsync(createOrderDto.CustomerId) as Customer; // Assuming Customer inherits from AppUser
            var agencyProduct = await _agencyProductReadRepository.GetByIdAsync(createOrderDto.AgencyProductId);

            if (agency == null)
                return new() { Succeeded = false, Message = "Webhook: Firma bulunamadı." };
            if (customer == null)
                return new() { Succeeded = false, Message = "Webhook: Müşteri bulunamadı." };
            if (agencyProduct == null)
                return new() { Succeeded = false, Message = "Webhook: Ürün bulunamadı." };

            // 2. Prepare Paths
            string tempDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp_files", createOrderDto.TempDirectoryId);
            string finalFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");

            if (!Directory.Exists(finalFilesPath))
                Directory.CreateDirectory(finalFilesPath);

            if (!Directory.Exists(tempDirectoryPath))
            {
                // This shouldn't happen if CheckoutController worked, but handle it.
                return new() { Succeeded = false, Message = "Webhook: Geçici dosya dizini bulunamadı." };
            }

            // 3. Process Files (Move from Temp to Final)
            var orderId = Guid.NewGuid();
            List<CopyFile> copyFiles = new List<CopyFile>();

            try
            {
                foreach (var fileInfo in createOrderDto.FileInfos)
                {
                    string tempFilePath = Path.Combine(tempDirectoryPath, fileInfo.TempName);
                    if (!System.IO.File.Exists(tempFilePath))
                    {
                        // Log this issue, maybe skip file or fail order? For now, fail.
                        // Consider cleaning up already moved files if failing here.
                        Directory.Delete(tempDirectoryPath, true); // Attempt cleanup
                        return new() { Succeeded = false, Message = $"Webhook: Geçici dosya bulunamadı: {fileInfo.TempName}" };
                    }

                    // Use original extension, SeoHelper for name, and pre-generated code
                    string originalExtension = Path.GetExtension(fileInfo.TempName); // Get extension from temp name
                    string finalFileName = SeoHelper(Path.GetFileNameWithoutExtension(fileInfo.OriginalName)) + "-" + fileInfo.FileCode + originalExtension;
                    string finalFilePath = Path.Combine(finalFilesPath, finalFileName);

                    // Move the file
                    System.IO.File.Move(tempFilePath, finalFilePath);

                    copyFiles.Add(new CopyFile
                    {
                        FilePath = finalFilePath, // Store the final path
                        FileName = finalFileName,
                        OrderId = orderId,
                        FileCode = fileInfo.FileCode,
                        // PageCount is already known via createOrderDto.ToplamSayfaSayisi for the whole order
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Attempt to clean up the temporary directory
                try { Directory.Delete(tempDirectoryPath, true); } catch { /* Ignore cleanup error */ }
                return new() { Succeeded = false, Message = $"Webhook: Dosya taşıma hatası: {ex.Message}" };
            }


            // 4. Create Order Entity
            var orderCode = await GenerateOrderCode();
            var order = new Order
            {
                Id = orderId, // Use the generated Guid
                Agency = agency,
                Customer = customer,
                OrderCode = orderCode,
                KopyaSayısı = createOrderDto.KopyaSayisi,
                TotalPrice = createOrderDto.TotalPrice, // Use price from metadata
                OrderState = OrderState.Pending, // Initial state after payment
                AgencyProduct = agencyProduct,
                SayfaSayısı = createOrderDto.ToplamSayfaSayisi, // Use total page count from metadata
                CopyFiles = copyFiles // Add the processed files
            };

            // 5. Save Order
            var response = await _orderWriteRepository.AddAsync(order);
            if (response)
            {
                await _orderWriteRepository.SaveAsync();

                // 6. Post-Creation Actions (Notifications, Cleanup)
                try
                {
                    // Send notifications
                    await _mailService.SendOrderCreatedForAgencyMailAsync(agency.Email, customer.UserName, orderCode, agency.AgencyName, order.KopyaSayısı, order.SayfaSayısı, order.TotalPrice);
                    await _mailService.SendOrderCreatedForUserMailAsync(customer.Email, customer.UserName, orderCode, agency.AgencyName, order.KopyaSayısı, order.SayfaSayısı, order.TotalPrice);
                    await _orderHubService.OrderAddedMessage(agency.Id, customer.Id, "Siparişiniz başarıyla alındı ve firma onayı bekleniyor.");

                    // Delete temporary directory AFTER successful order save and notifications
                    Directory.Delete(tempDirectoryPath, true); // true for recursive delete
                }
                catch (Exception postActionEx)
                {
                    // Log this error, but the order is already created.
                    // Maybe add a flag to the order indicating post-processing issues?
                    Console.WriteLine($"Webhook Post-Action Error (Order {orderCode}): {postActionEx.Message}");
                }

                return new()
                {
                    Message = "Sipariş başarıyla oluşturuldu.",
                    Succeeded = true,
                };
            }
            else
            {
                // If AddAsync fails, attempt cleanup
                try { Directory.Delete(tempDirectoryPath, true); } catch { /* Ignore cleanup error */ }
                return new()
                {
                    Succeeded = false,
                    Message = "Webhook: Sipariş veritabanına eklenirken hata oluştu."
                };
            }
        }

        public async Task<SucceededMessageResponse> CancelOrderAsync(string orderCode)
        {
            var user=await ContextUser();
            var order = await _orderReadRepository.GetWhere(u => u.OrderCode == orderCode && u.Customer.Id == user.Id).FirstOrDefaultAsync();
            if (order == null)
            {
                return new()
                {
                    Message = "Sipariş bulunamadı.",
                    Succeeded = false,
                };
            }
            if (order.OrderState != OrderState.Pending)
            {
                return new()
                {
                    Message = "Siparişi iptal etmek için siparişin bekleme durumunda olması gerekir.",
                    Succeeded = false,
                };
            }
            await _orderWriteRepository.RemoveAsync(order.Id.ToString());
            await _orderWriteRepository.SaveAsync();
            return new()
            {
                Message = "Sipariş iptal edildi.",
                Succeeded = true,
            };
        }

        public async Task<SucceededMessageResponse> CreateComment(CreateComment? createComment)
        {
            var user= await ContextUser();
            var order = await _orderReadRepository.GetWhere(u => u.OrderCode == createComment.OrderCode)
                .Include(u => u.Agency).Include(u => u.Customer).Include(u => u.Comment).FirstOrDefaultAsync();
            if (order.Customer != user)
                return new()
                {
                    Succeeded = false,
                    Message = "Bu sipariş size ait değil."
                };
            if (createComment.StarRating >= 1)
            {
                if (order.OrderState == OrderState.Completed)
                {
                    await _commentWriteRepository.AddAsync(new()
                    {
                        StarRating = createComment.StarRating,
                        CommentText = createComment.CommentText,
                        Agency = order.Agency,
                        Customer = order.Customer,
                        Order = order,
                        OrderId = order.Id,
                    });
                    await _commentWriteRepository.SaveAsync();
                    await _userService.UpdateStarRating(order.Agency.Id.ToString());
                    await _orderHubService.OrderUpdatedMessage(order.Agency.Id, order.Customer.Id, $"{order.OrderCode} kodlu sipariş güncellenmiştir.");
                    return new()
                    {
                        Succeeded = true,
                        Message = "Sipariş değerlendirmesi başarıyla yapılmıştır."
                    };
                }
                else
                {
                    return new()
                    {
                        Succeeded = false,
                        Message = "Siparişi değerlendirebilmek için sipariş durumunun bitmiş olması gerekmektedir."
                    };
                }
            }
            return new()
            {
                Succeeded = false,
                Message = "Yıldız değerlendirmesi 0dan büyük olmalıdır."
            };
        }

        public async Task<GetOrderProductAnalysis> GetOrderProductAnalysis(DateTime startDate, DateTime endDate,string? paperType, string? colorOption, string? printType)
        {
            var customer = await ContextUser();
            var ordersQuery = _orderReadRepository.GetWhere(u => u.Agency.Id == customer.Id)
                .Where(u=>u.CreatedDate.Date >= startDate.Date && u.CreatedDate.Date <= endDate.Date)
                .Include(u => u.Agency).Include(u=>u.AgencyProduct).ThenInclude(u=> u.Product)
                .AsQueryable();

            PrintTypes? parsedPrintType = null; // Nullable Enum

            if (Enum.TryParse<PrintTypes>(printType, true, out var result))
            {
                parsedPrintType = result;
            }
            ColorOptions? parsedColorOption= null; // Nullable Enum

            if (Enum.TryParse<ColorOptions>(colorOption, true, out var colorOptionResult))
            {
                parsedColorOption = colorOptionResult;
            }

            if (!string.IsNullOrEmpty(paperType))
                ordersQuery = ordersQuery.Where(u => u.AgencyProduct.Product.PaperType == paperType);
            if (parsedColorOption!=null)
                ordersQuery = ordersQuery.Where(u => u.AgencyProduct.Product.ColorOption == parsedColorOption);
            if (parsedPrintType != null)
                ordersQuery = ordersQuery.Where(u => u.AgencyProduct.Product.PrintType==parsedPrintType );

            var orders = await ordersQuery.ToListAsync();
            var groupedResult = await ordersQuery
                        .GroupBy(u => new
                        {
                            u.AgencyProduct.Id,
                            u.AgencyProduct.Product.PaperType,
                            u.AgencyProduct.Product.ColorOption,
                            u.AgencyProduct.Product.PrintType
                        })
                        .Select( g => new GetOrderProductAnalysisElement
                        {
                            PaperType = g.Key.PaperType,
                            ColorOption = g.Key.ColorOption.ToString(),
                            PrintType = g.Key.PrintType.ToString(),
                            Count = g.Count(),
                        })
                        .ToListAsync();
            return new()
            {
                GetOrderProductAnalysisElements=groupedResult
            };
        }

        public async Task<List<GetAgencyCommentAnalysis>> GetAgencyCommentAnalysis(DateTime startDate, DateTime endDate,string? groupBy)
        {
            var ageny = await ContextUser();
            var agencyCommentsQuery= _commentReadRepository.GetWhere(u=> u.Agency.Id==ageny.Id)
                .Where(u => u.CreatedDate.Date >= startDate.Date && u.CreatedDate.Date <= endDate.Date)
                .Include(u => u.Agency).Include(u=>u.Customer)
                .AsQueryable();
            var groupedComments = groupBy.ToLower() switch
            {
                "month" => agencyCommentsQuery
                    .GroupBy(o => new { o.CreatedDate.Year, o.CreatedDate.Month })
                    .Select(u => new GetAgencyCommentAnalysis
                    {
                        AverageStar = (float)u.Average(u => u.StarRating),
                        Count = u.Count(),
                        Period = new DateTime(u.Key.Year, u.Key.Month, 1),
                        TotalUserCount= u.Select(u=>u.Customer).Distinct().Count()
                    }),
                "year" => agencyCommentsQuery
                    .GroupBy(o => o.CreatedDate.Year)
                    .Select(u => new GetAgencyCommentAnalysis
                    {
                        AverageStar = (float)u.Average(u => u.StarRating),
                        Count = u.Count(),
                        Period = new DateTime(u.Key, 1, 1),
                        TotalUserCount = u.Select(u => u.Customer).Distinct().Count()
                    }),
                _ => agencyCommentsQuery
                    .GroupBy(o => o.CreatedDate.Date)
                    .Select(u => new GetAgencyCommentAnalysis
                    {
                        AverageStar = (float)u.Average(u => u.StarRating),
                        Count = u.Count(),
                        Period = u.Key,
                        TotalUserCount = u.Select(u => u.Customer).Distinct().Count()
                    })
            };
            return await groupedComments.ToListAsync();
        }
    }
}
