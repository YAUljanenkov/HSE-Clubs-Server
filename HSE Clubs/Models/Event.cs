using System;

namespace HSE_Clubs.Models
{
    public class Event
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Place { get; set; }
        public bool IsOnline { get; set; }
        public Club Owner { get; set; }
    }
}