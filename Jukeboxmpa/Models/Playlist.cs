using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Jukeboxmpa.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Mijn Playlist";

        // Links the playlist to a specific registered Gebruiker
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        // if true other users can find or view this playlist
        public bool IsPublic { get; set; } = false;

        // the list of songs in this playlist
        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
