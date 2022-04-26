using System;
using System.Collections.Generic;
using System.Linq;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController
    {
        private ClubsContext db = new ClubsContext();

        [HttpPost("login")]
        public IActionResult Login([FromForm] string name, [FromForm] string email, [FromForm(Name = "unique_name")] string uniqueName)
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
        public IActionResult Update([FromForm] string name, [FromForm] string email, [FromForm(Name = "unique_name")] string uniqueName, [FromForm] string vk, string telegram)
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
    }
}