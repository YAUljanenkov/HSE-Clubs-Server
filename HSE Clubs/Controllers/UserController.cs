using System;
using System.Collections.Generic;
using System.Linq;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : SavableController
    {
        private ClubsContext db = new ClubsContext();

        public UserController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromForm] string name, [FromForm] string email,
            [FromForm(Name = "unique_name")] string uniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                user.Name = name;
                user.Email = email;
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                user = new User { Name = name, UniqueName = uniqueName, Email = email };
                db.Users.Add(user);
                db.SaveChanges();
            }

            return new OkResult();
        }

        [HttpPost("update")]
        public IActionResult Update([FromForm] string name, [FromForm] string email,
            [FromForm(Name = "unique_name")] string uniqueName, [FromForm] string vk, [FromForm] string telegram)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                user.Name = name ?? user.Name;
                user.Email = email ?? user.Email;
                user.Telegram = telegram ?? user.Telegram;
                user.Vk = vk ?? user.Vk;
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult("User not found") { StatusCode = 404 };
            }

            return new OkResult();
        }

        [HttpGet("get")]
        public IActionResult Get([FromQuery(Name = "unique_name")] string uniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                return new OkObjectResult(user);
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult("User not found") { StatusCode = 404 };
            }
        }

        [HttpGet("get_clubs")]
        public IActionResult GetClubs([FromQuery(Name = "unique_name")] string uniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                return new OkObjectResult(user.Clubs ?? new List<Club>());
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
                db.SaveChanges();
                return new OkResult();
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult("User not found") { StatusCode = 404 };
            }
        }

        // [HttpGet("{uniqueName}/get_image")]
        // public IActionResult GetImage(string unique_name)
        // {
        //     
        // }
    }
}