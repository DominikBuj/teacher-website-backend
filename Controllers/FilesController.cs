using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Authorize]
    [Route("api/files")]
    [ApiController]
    public class FilesController : Controller
    {
        private async Task<ActionResult> UploadFile(IFormFile file, string uploadFolderName)
        {
            if (file.Length <= 0) return BadRequest();

            try
            {
                string? fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
                if (String.IsNullOrEmpty(fileName)) return BadRequest();

                try
                {
                    string folderName = Path.Combine("Resources", uploadFolderName);
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    string fullPath = Path.Combine(folderPath, fileName);
                    string applicationUrl = @"https://localhost:5001/";
                    string databasePath = Path.Combine(applicationUrl, folderName, fileName);

                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    return Ok(new { databasePath });
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to upload the file");
                    Console.WriteLine(e.Message);
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect file name");
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        [HttpPost("image")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> UploadImage()
        {
            IFormCollection formCollection = await Request.ReadFormAsync();
            IFormFile file = formCollection.Files[0];

            return await UploadFile(file, "Images");
        }

        [HttpPost("file")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> UploadFile()
        {
            IFormCollection formCollection = await Request.ReadFormAsync();
            IFormFile file = formCollection.Files[0];

            return await UploadFile(file, "Files");
        }
    }
}
