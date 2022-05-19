using System;
using System.Linq;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSE_Clubs.Controllers
{
    [Route("api/club")]
    public class ClubController : SavableController
    {
        public ClubController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
        }

        private ClubsContext db = new ClubsContext();

        /// <summary>
        /// Creates new club.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="administratorUniqueName"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult Create([FromForm] string name, [FromForm] string description,
            [FromForm(Name = "unique_name")] string administratorUniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == administratorUniqueName);
                if (db.Clubs.Any(x => x.Name == name))
                {
                    return new ConflictObjectResult("Club with this name already exist");
                }

                var club = new Club { Name = name, Description = description, Administrator = user };
                user.Clubs.Add(club);
                db.Clubs.Add(club);
                db.SaveChanges();
                club = db.Clubs.First(x => x.Name == name);
                return new OkObjectResult(new { id = club.ID });
            }
            catch (InvalidOperationException)
            {
                return new NotFoundObjectResult("User not found");
            }
        }

        [HttpPost("{id}/update")]
        public IActionResult Update([FromForm] string name, [FromForm] string description, string id)
        {
            if (!int.TryParse(id, out int number))
            {
                return new BadRequestObjectResult("Wrong ID format.");
            }

            try
            {
                var club = db.Clubs.First(x => x.ID == number);
                club.Name = name ?? club.Name;
                club.Description = description;
                Console.WriteLine(club.Administrator?.Name);
                db.SaveChanges();
                return new OkResult();
            }
            catch (InvalidOperationException)
            {
                return new NotFoundObjectResult("Club not found");
            }
        }

        [HttpPost("{id}/set_image")]
        public IActionResult SetImage([FromForm] IFormFile image, string id)
        {
            string fileName = UploadedFile(image);
            Club club;
            try
            {
                if (int.TryParse(id, out int number))
                {
                    club = db.Clubs.First(x => x.ID == number);
                    club.AvatarPath = fileName;
                    db.SaveChanges();
                    return new OkResult();
                }

                return new BadRequestObjectResult("Wrong ID format.");
            }
            catch (InvalidOperationException)
            {
                return new NotFoundObjectResult("Club not found");
            }
        }

        [HttpGet("get_all_clubs")]
        public IActionResult GetAllClubs(string query)
        {
            query = query?.Trim().ToLower();
            return string.IsNullOrEmpty(query)
                ? new OkObjectResult(db.Clubs.Include(c => c.Administrator).Include(c => c.Events))
                : new OkObjectResult(db.Clubs.Include(c=>c.Administrator).Where(x =>
                    x.Name.ToLower().Contains(query) || x.Description.ToLower().Contains(query)));
        }

        [HttpGet("get/{id:int}")]
        public IActionResult GetClub(int id)
        {
            
            var club = db.Clubs.Include(c => c.Administrator).Include(c => c.Events)
                .FirstOrDefault(c => c.ID == id);
            Console.WriteLine($"{club?.Name} {club?.Description}");
            return new OkObjectResult(club);
        }
    }
}
