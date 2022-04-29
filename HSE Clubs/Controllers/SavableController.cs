using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    public class SavableController : Controller
    {
        protected readonly IWebHostEnvironment _webHostEnvironment;

        public SavableController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        internal string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var directory = new DirectoryInfo(path);

                if (directory.Exists == false)
                {
                    directory.Create();
                }

                uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
                string filePath = Path.Combine(path, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}