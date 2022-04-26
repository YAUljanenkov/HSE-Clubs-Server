using System;
using System.Collections.Generic;
using System.Linq;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    [Route("api/club")]
    public class ClubController
    {
        [HttpPost("create")]
        public IActionResult Create([FromForm] string name, [FromForm] string description,
            [FromForm] string AvatarPath, [FromForm(Name = "unique_name")] string unique_name)
        {
            return new OkResult();
        }
    }
}