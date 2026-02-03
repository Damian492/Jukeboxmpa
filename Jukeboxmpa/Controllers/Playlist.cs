using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jukeboxmpa.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public PlaylistController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Playlist/My
        public async Task<IActionResult> My()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("All");

            var userId = _userManager.GetUserId(User);

            var playlists = await _db.Playlists
                .Include(p => p.Songs)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return View(playlists);
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

            _db.Playlists.Add(playlist);
            await _db.SaveChangesAsync();

            return RedirectToAction("My");
        }
    }
}
