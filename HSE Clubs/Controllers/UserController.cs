using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    /// <summary>
    /// Provides methods to work with User.
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : SavableController
    {
        private ClubsContext db = new ClubsContext();

        public UserController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
        }

        /// <summary>
        /// Allows user to log in or regiser a new user.
        /// </summary>
        /// <param name="name">Full name of the user.</param>
        /// <param name="email">User's corporative email.</param>
        /// <param name="uniqueName">User's unique name. Same field from ID token.</param>
        /// <returns>OK if user logged in or created.</returns>
        [HttpPost("login")]
        [Produces("application/json")]
        public IActionResult Login([FromForm] string name, [FromForm] string email, [FromForm] string uniqueName)
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

            Console.WriteLine($"Got user {user.Name} {user.Email}");
            return new OkResult();
        }

        /// <summary>
        /// Allows to update information about the user.
        /// </summary>
        /// <param name="uniqueName">User's unique name is required to find user in database.</param>
        /// <param name="name">Full name of the user.</param>
        /// <param name="email">User's email.</param>
        /// <param name="vk">Link to user's VK in format "https://vk.com/userId"</param>
        /// <param name="telegram">Link to user's Telegram in format "https://t.me/userId"</param>
        /// <param name="isShowContacts">User's contacts.</param>
        /// <returns>Ok if info is updated and Error if user not found.</returns>
        [HttpPost("{uniqueName}/update")]
        [Produces("application/json")]
        public IActionResult Update(string uniqueName, [FromForm] string name, [FromForm] string email,
            [FromForm] string vk, [FromForm] string telegram, [FromForm] string isShowContacts)
        {
            Console.WriteLine($"Got request from {uniqueName}");
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                user.Name = name ?? user.Name;
                user.Email = email ?? user.Email;
                user.Telegram = telegram ?? user.Telegram;
                user.Vk = vk ?? user.Vk;
                Console.WriteLine(isShowContacts);
                user.IsShowContacts = isShowContacts == "true";
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult("User not found") { StatusCode = 404 };
            }

            return new OkResult();
        }

        /// <summary>
        /// Returns full info about the user.
        /// </summary>
        /// <param name="uniqueName">User's unique name is required to find user in database.</param>
        /// <returns>All fields about the user. </returns>
        /// <example>
        /// <code>
        /// {
        ///   "id": 543,
        ///   "uniqueName": "EDU//uniqueName",
        ///   "name": "Full User's Name",
        ///   "email": "email@edu.hse.ru",
        ///   "vk": "https://vk.com/userId",
        ///   "telegram": "https://t.me/userId",
        ///   "photoPath": "avatar.jpg",
        ///   "clubs": []
        /// }
        /// </code>
        /// </example>
        [HttpGet("{uniqueName}/get")]
        [Produces("application/json")]
        public IActionResult Get(string uniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                return new OkObjectResult(user);
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult(new { error = "User not found" }) { StatusCode = 404 };
            }
        }


        /// <summary>
        /// Returns a list of clubs where user is participated.
        /// </summary>
        /// <param name="uniqueName">User's unique name is required to find user in database.</param>
        /// <returns>A list of clubs in a format described below.</returns>
        /// <example>
        /// <code>
        /// {
        ///   "clubs": []
        /// }
        /// </code>
        /// </example>
        [HttpGet("{uniqueName}/get_clubs")]
        [Produces("application/json")]
        public IActionResult GetClubs(string uniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                return new OkObjectResult(new { clubs = user.Clubs ?? new List<Club>() });
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult(new { error = "User not found" }) { StatusCode = 404 };
            }
        }

        /// <summary>
        /// Allows to set an avatar to a user.
        /// </summary>
        /// <param name="uniqueName">User's unique name is required to find user in database.</param>
        /// <param name="image">An image to use as an avatar.</param>
        /// <returns>Ok if image is set, 404 if user not found.</returns>
        [HttpPost("{uniqueName}/set_image")]
        [Produces("application/json")]
        public IActionResult SetImage(string uniqueName, [FromForm] IFormFile image)
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
                return new ObjectResult(new { error = "User not found" }) { StatusCode = 404 };
            }
        }

        /// <summary>
        /// Allows to get an avatar name to upload a photo.
        /// </summary>
        /// <param name="uniqueName">User's unique name is required to find user in database.</param>
        /// <returns>A photo name on the server.</returns>
        [HttpGet("{uniqueName}/get_image")]
        [Produces("application/json")]
        public IActionResult GetImage(string uniqueName)
        {
            User user;
            try
            {
                user = db.Users.First(x => x.UniqueName == uniqueName);
                return new OkObjectResult(new { photo = user.PhotoPath });
            }
            catch (InvalidOperationException)
            {
                return new ObjectResult(new { error = "User not found" }) { StatusCode = 404 };
            }
        }
    }
}