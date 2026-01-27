namespace Jukeboxmpa.Models
{
    // Simple domain model representing a Song.
    // The properties correspond to table columns in the database.
    public class Song
    {
        public int ID { get; set; } // unique identifier (primary key)
        public string? Title { get; set; } // song title
        public string? Artist { get; set; } // artist name
        public string? Album { get; set; } // album name
        public string? FilePath { get; set; } // path/URL to the audio file
        public string? Genre { get; set; } // genre of the song

        // New: free-form credits text (e.g. "Performed by ... / Written by ...")
        public string? Credits { get; set; }

        public int? PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        // Add this property:
        public int? Duration { get; set; } // Duration in seconds
    }
}
