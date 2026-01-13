using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Jukeboxmpa.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jukeboxmpa.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SessionPlaylistService _sessionPlaylist;
        private readonly UserManager<IdentityUser> _userManager;

        public PlaylistController(ApplicationDbContext db, SessionPlaylistService sessionPlaylist, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _sessionPlaylist = sessionPlaylist;
            _userManager = userManager;
        }

        // GET: /Playlist/My
        public async Task<IActionResult> My()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var playlists = await _db.Playlists
                    .Include(p => p.Songs)
                    .Where(p => p.UserId == userId)
                    .ToListAsync();
                return View(playlists);
            }
            else
            {
                var playlist = _sessionPlaylist.GetPlaylist();
                return View(playlist);
            }
        }

        // GET: /Playlist/All
        public async Task<IActionResult> All()
        {
            var playlists = await _db.Playlists
                .Include(p => p.User)
                .Include(p => p.Songs)
                .Where(p => p.IsPublic)
                .ToListAsync();
            return View(playlists);
        }

        // POST: /Playlist/Save
        [HttpPost]
        public async Task<IActionResult> Save(string name, bool isPublic)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = _userManager.GetUserId(User);
            var sessionSongs = _sessionPlaylist.GetPlaylist();

            var playlist = new Playlist
            {
                Name = name,
                UserId = userId,
                IsPublic = isPublic,
                Songs = sessionSongs
            };

            _db.Playlists.Add(playlist);
            await _db.SaveChangesAsync();
            _sessionPlaylist.ClearPlaylist();

            return RedirectToAction("My");
        }

        // GET: /Playlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Playlist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsPublic")] Playlist playlist)
        {
            if (User.Identity.IsAuthenticated)
            {
                playlist.UserId = _userManager.GetUserId(User);
                playlist.User = await _userManager.GetUserAsync(User);
            }
            else
            {
                playlist.UserId = null;
                playlist.User = null;
            }

            _db.Playlists.Add(playlist);
            await _db.SaveChangesAsync();
            return RedirectToAction("My");
        }
    }
}
