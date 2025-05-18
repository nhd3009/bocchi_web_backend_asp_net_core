using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file);
        void DeleteImage(string imageUrl);
    }
}