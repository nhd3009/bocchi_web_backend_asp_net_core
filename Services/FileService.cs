using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.Services
{
    public class FileService : IFileService
    {
        private readonly string _uploadDirectory;

        public FileService(IWebHostEnvironment environment)
        {
            var rootPath = environment.WebRootPath ?? environment.ContentRootPath;
            _uploadDirectory = Path.Combine(rootPath, "uploads", "products");

            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                throw new ArgumentException("Invalid file type. Only JPEG, PNG and GIF are allowed.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/products/{fileName}";
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_uploadDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}