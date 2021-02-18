using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Authorize]
    [Route("api/files")]
    [ApiController]
    public class FilesController : Controller
    {
        private ActionResult<string> UploadAnyFile(string folder, IFormFile file, string fileName)
        {
            string folderName = Path.Combine("Resources", folder);
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string fullPath = Path.Combine(folderPath, fileName);
            string applicationUrl = @"https://localhost:5001/";
            string databasePath = Path.Combine(applicationUrl, folderName, fileName);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return Ok(new { databasePath });
        }

        [HttpPost("image")]
        [DisableRequestSizeLimit]
        public ActionResult<string> UploadImage()
        {
            IFormFile file = Request.Form.Files[0];

            if (file.Length <= 0) return BadRequest();
            
            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            return UploadAnyFile("Images", file, fileName);
        }

        [HttpPost("file")]
        [DisableRequestSizeLimit]
        public ActionResult<string> UploadFile()
        {
            IFormFile file = Request.Form.Files[0];
            
            if (file.Length <= 0) return BadRequest();
            
            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            return UploadAnyFile("Files", file, fileName);
        }
    }
}
