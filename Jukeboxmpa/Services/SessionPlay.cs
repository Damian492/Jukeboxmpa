using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace Jukeboxmpa.Services
{
    public class SessionPlaylistService
    {
        private const string SessionKey = "VisitorPlaylist";
        private readonly ISession _session;

        public SessionPlaylistService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public List<Song> GetPlaylist()
        {
            var json = _session.GetString(SessionKey);
            return json == null ? new List<Song>() : JsonSerializer.Deserialize<List<Song>>(json);
        }

        public void AddSong(Song song)
        {
            var playlist = GetPlaylist();
            playlist.Add(song);
            _session.SetString(SessionKey, JsonSerializer.Serialize(playlist));
        }

        public void ClearPlaylist()
        {
            _session.Remove(SessionKey);
        }
    }
}
