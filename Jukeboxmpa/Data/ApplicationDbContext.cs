using Jukeboxmpa.Models;
using Microsoft.EntityFrameworkCore;

namespace Jukeboxmpa.Data
{
    // EF Core DbContext for the application.
    // Configure the context in Program.cs to use SQLite.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Songs DbSet maps to the Songs table in the database.
        public DbSet<Song> Songs { get; set; } 
    }
}