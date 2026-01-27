namespace Jukeboxmpa.Models
{
    public class Song
    {
        public int ID { get; set; } // unieke ID voor het iedje
        public string Title { get; set; } // naam van het liedje
        public string Artist { get; set; } // artiest van het liedje
        public string Album { get; set; } // naam album
        public string FilePath { get; set; } // pad naar het audiobestand
    }
}
