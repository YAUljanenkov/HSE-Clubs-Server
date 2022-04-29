using System;
using System.Linq;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    [Route("api/club")]
    public class ClubController : SavableController
    {
        public ClubController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
        }

        private ClubsContext db = new ClubsContext();
        
        [HttpPost("create")]
        public IActionResult Create([FromForm] string name, [FromForm] string description,
            [FromForm(Name = "unique_name")] string administratorUniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == administratorUniqueName);
                var club = new Club { Name = name, Description = description, Administrator = user};
                db.Clubs.Add(club);
                db.SaveChanges();
                return new OkResult();
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult("User not found") { StatusCode = 404 };
            }
        }

        [HttpPost("{uniqueName}/set_image")]
        public IActionResult SetImage([FromForm] IFormFile image, string uniqueName)
        {
            string fileName = UploadedFile(image);
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                user.PhotoPath = fileName;
                return new OkResult();
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult("User not found") { StatusCode = 404 };
            }
        }
    }
}