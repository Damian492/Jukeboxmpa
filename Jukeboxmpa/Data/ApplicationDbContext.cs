using Jukeboxmpa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // <-- NEW USING
using Microsoft.AspNetCore.Identity; // Needed if you want to use IdentityUser directly

namespace Jukeboxmpa.Data
{
    // The class now inherits from IdentityDbContext<IdentityUser>
    // This tells EF Core to include all the standard Identity tables (AspNetUsers, AspNetRoles, etc.)
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Songs DbSet maps to the Songs table in the database.
        public DbSet<Song> Songs { get; set; }
    }
}