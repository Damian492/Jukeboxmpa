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


        public async Task<IActionResult> All()
        {
            var playlists = await _db.Playlists
                .Include(p => p.User)
                .Include(p => p.Songs)
                .Where(p => p.IsPublic)
                .ToListAsync();
            return View(playlists);
        }


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


        public IActionResult Create()
        {
            return View();
        }


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

        public async Task<IActionResult> Details(int id)
        {
            var playlist = await _db.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            // Only show private playlists to the owner
            if (!playlist.IsPublic && (!User.Identity.IsAuthenticated || playlist.UserId != _userManager.GetUserId(User)))
                return Forbid();

            // Provide all songs for the add-song dropdown
            ViewBag.AllSongs = await _db.Songs.ToListAsync();

            return View(playlist);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSong(int playlistId, int songId)
        {
            var playlist = await _db.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
                return NotFound();

            // Only allow if public or owner
            var isOwner = User.Identity.IsAuthenticated && playlist.UserId == _userManager.GetUserId(User);
            if (!playlist.IsPublic && !isOwner)
                return Forbid();

            var song = await _db.Songs.FindAsync(songId);
            if (song != null && !playlist.Songs.Any(s => s.ID == songId))
            {
                playlist.Songs.Add(song);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = playlistId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveToMyList(int playlistId)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = _userManager.GetUserId(User);
            var playlist = await _db.Playlists.Include(p => p.Songs).FirstOrDefaultAsync(p => p.Id == playlistId && p.IsPublic);
            if (playlist == null)
                return NotFound();

            var newPlaylist = new Playlist
            {
                Name = playlist.Name,
                UserId = userId,
                IsPublic = false,
                Songs = new List<Song>(playlist.Songs)
            };
            _db.Playlists.Add(newPlaylist);
            await _db.SaveChangesAsync();

            return RedirectToAction("My");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var playlist = await _db.Playlists.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (playlist != null)
            {
                _db.Playlists.Remove(playlist);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("My");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSong(int playlistId, int songId)
        {
            var playlist = await _db.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            var userId = _userManager.GetUserId(User);
            var isOwner = playlist != null && playlist.UserId == userId;

            if (playlist == null || (!playlist.IsPublic && !isOwner))
                return Forbid();

            var song = playlist.Songs.FirstOrDefault(s => s.ID == songId);
            if (song != null)
            {
                playlist.Songs.Remove(song);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = playlistId });
        }
    }
}
