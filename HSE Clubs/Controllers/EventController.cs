using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using HSE_Clubs.DAL;
using HSE_Clubs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Clubs.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventController: SavableController
    {
        private ClubsContext db = new ClubsContext();

        public EventController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        [HttpPost("create")]
        public IActionResult Create([FromForm] string name, [FromForm] string description, [FromForm] string dateTime,
            [FromForm] string room, [FromForm] int clubID)
        {
            var club = db.Clubs.FirstOrDefault(c => c.ID == clubID);
            if (club == null)
            {
                return new NotFoundResult();
            }
            
            var newEvent = new Event { Name = name, Description = description, DateTime = dateTime, Place = room, Owner = club};
            club.Events.Add(newEvent);
            db.SaveChanges();
            return new OkResult();
        }
        
        [HttpPost("get_events")]
        public IActionResult GetEvents([FromForm] int clubID)
        {
            var events = db.Events.Where(e => e.OwnerID == clubID);
            return new OkObjectResult(events);
        }
    }
}