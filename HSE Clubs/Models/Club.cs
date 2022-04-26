using System.Collections.Generic;

namespace HSE_Clubs.Models
{
    public class Club
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarPath { get; set; }
        public User Administrator { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
