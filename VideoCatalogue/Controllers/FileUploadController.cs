using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HWVideoCatalogue.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
 
        private readonly string _mediaFolderPath;

        public FileUploadController(IWebHostEnvironment env)
        {
             _mediaFolderPath = Path.Combine(env.ContentRootPath, "MediaFiles");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)] // 200 MB limit
        public async Task<ActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest("No files selected for upload.");
                }
                foreach (var file in files)
                {
                    if (file.Length == 0)
                    {
                        return BadRequest("Empty file(s) cannot be uploaded.");
                    }

                    if (file.ContentType != "video/mp4")
                    {
                        return BadRequest("Only MP4 files are allowed.");
                    }

                  
                   if (file.Length > 0 && file.ContentType == "video/mp4")
                    {

                        if (!Directory.Exists(_mediaFolderPath))
                        {
                            Directory.CreateDirectory(_mediaFolderPath);
                        }
                        var filePath = Path.Combine(_mediaFolderPath, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to upload files: {ex.Message}");
            }
        }
    }
    }
