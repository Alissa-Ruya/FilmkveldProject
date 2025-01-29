// AppDbContext
using Microsoft.EntityFrameworkCore;
using FilmkveldAPI.Models;

namespace FilmkveldAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Filmkveld> MovieNights { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}