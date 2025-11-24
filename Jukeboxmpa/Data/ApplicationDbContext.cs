using Jukeboxmpa.Models;
using Microsoft.EntityFrameworkCore;

namespace Jukebompa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; } 
    }
}