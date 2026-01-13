using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Jukeboxmpa.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Mijn Playlist";

        // Links the playlist to a specific registered Gebruiker
        public string UserId { get; set; } // Owner
        public IdentityUser User { get; set; }

        // If true, other users can find/view this playlist
        public bool IsPublic { get; set; } = false;

        // The list of songs in this playlist
        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
