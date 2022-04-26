using HSE_Clubs.Models;
using Microsoft.EntityFrameworkCore;


namespace HSE_Clubs.DAL
{
    public class ClubsContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("User ID=postgres;Password=mysecretpassword;Host=localhost;Port=5432;Database=postgres;");
        
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Club> Clubs { get; set; }
    }
}