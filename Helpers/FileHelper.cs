using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Helpers
{
    public class FileHelper
    {
        private const string FILE_PATH = "files";

        public async Task Upload(IFormFile file, string fileName, IHostingEnvironment environment)
        {
            if (file != null && file.Length > 0)
            {
                // Rename file
                
                var filePath = Path.Combine(environment.WebRootPath, FILE_PATH, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public string GenerateFileName(string extension)
        {
            return Path.ChangeExtension(Path.GetRandomFileName(), "." + extension);
        }

        public void UploadBase64String(string base64String, string fileName, IHostingEnvironment environment)
        {
            var filePath = Path.Combine(environment.WebRootPath, FILE_PATH, fileName);
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64String));
        }
    }
}
