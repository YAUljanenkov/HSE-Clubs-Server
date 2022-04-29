using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HSE_Clubs.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UniqueName { get; set; } // Same as in HSE SSO id_token
        public string Name { get; set; }
        public string Email { get; set; }
        public string Vk { get; set; } = "";
        public string Telegram { get; set; }  = "";
        public string PhotoPath { get; set; }  = "";
        public ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}
