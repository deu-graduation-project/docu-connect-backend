using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.Repositories.FileRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FotokopiRandevuAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        readonly IFileService _fileService;
        readonly IFileReadRepository _fileReadRepository;
        public FileController(IFileService fileService, IFileReadRepository fileReadRepository)
        {
            _fileService = fileService;
            _fileReadRepository = fileReadRepository;
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileCode)
        {
            var succeeded=await _fileService.DownloadFileCheck(fileCode);
            if (succeeded)
            {
                var file= await _fileReadRepository.GetSingleAsync(u=>u.FileCode == fileCode);
                if (file == null)
                    throw new Exception("Dosya bulunamadı.");
                var memory = new MemoryStream();
                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read))
                {
                    stream.CopyTo(memory);
                }

                memory.Position = 0;

                var fileName = Path.GetFileName(file.FilePath);
                return File(memory, "application/pdf", fileName);
            }
            else
            {
                throw new Exception("Dosya indirilirken bir hata ile karşılaşıldı.");
            }
            
        }
    }
}
