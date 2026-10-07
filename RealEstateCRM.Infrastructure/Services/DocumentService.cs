using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RealEstateCRM.API.Services
{
    public class DocumentService
    {
        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Set destination path inside static wwwroot folder
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "properties");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique filename to prevent overwriting existing files
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return relative URL path for DB storage
            return $"/images/properties/{uniqueFileName}";
        }
    }
}