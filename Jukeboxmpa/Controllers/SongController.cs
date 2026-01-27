using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Jukeboxmpa.Controllers
{
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SongController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? genre, string? search)
        {
            var genresQuery = _context.Songs
                .Select(s => s.Genre)
                .Where(g => g != null)
                .Distinct()
                .OrderBy(g => g);

            ViewBag.Genres = await genresQuery.ToListAsync();

            var songs = _context.Songs.AsQueryable();
            if (!string.IsNullOrEmpty(genre))
            {
                songs = songs.Where(s => s.Genre == genre);
            }

            if (!string.IsNullOrEmpty(search))
                ViewBag.PublicPlaylists = await _context.Playlists
                    .Where(p => p.IsPublic && p.Name.Contains(search))
                    .ToListAsync();
            else
                ViewBag.PublicPlaylists = await _context.Playlists
                    .Where(p => p.IsPublic)
                    .ToListAsync();

            return View(await songs.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(m => m.ID == id);
            if (song == null)
                return NotFound();

            return View(song);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Artist,Album,FilePath,Genre,Credits")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var song = await _context.Songs.FindAsync(id.Value);
            if (song == null)
                return NotFound();

            return View(song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Artist,Album,FilePath,Genre,Credits")] Song song)
        {
            if (id != song.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Songs.AnyAsync(e => e.ID == song.ID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var song = await _context.Songs
                .FirstOrDefaultAsync(m => m.ID == id);

            if (song == null)
                return NotFound();

            return View(song);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}