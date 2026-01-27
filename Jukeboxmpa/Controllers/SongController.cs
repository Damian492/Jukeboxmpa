using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using NAudio.Wave; // Add this at the top

namespace Jukeboxmpa.Controllers
{
    // Controller that handles CRUD operations for Song entities.
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SongController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // READ (Overview) with optional genre filter
        // GET: /Song/Index?genre=Rock
        public async Task<IActionResult> Index(string? genre, string? search)
        {
            // Build a distinct list of genres for the filter dropdown.
            var genresQuery = _context.Songs
                .Select(s => s.Genre)
                .Where(g => g != null)
                .Distinct()
                .OrderBy(g => g);

            ViewBag.Genres = await genresQuery.ToListAsync();

            // Query songs and apply optional filter.
            var songs = _context.Songs.AsQueryable();
            if (!string.IsNullOrEmpty(genre))
            {
                songs = songs.Where(s => s.Genre == genre);
            }

            // Provide public playlists for the sidebar (with optional search)
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

        // Details (GET)
        // GET: /Song/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            // Only show private playlists to the owner
            if (!playlist.IsPublic && (!User.Identity.IsAuthenticated || playlist.UserId != _userManager.GetUserId(User)))
                return Forbid();

            // Provide all songs for the add-song dropdown
            ViewBag.AllSongs = await _context.Songs.ToListAsync();

            return View(playlist);
        }

        // CREATE (GET)
        // GET: /Song/Create
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        // POST: /Song/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Artist,Album,Genre,Credits")] Song song, IFormFile mp3File)
        {
            if (mp3File == null || mp3File.Length == 0)
            {
                ModelState.AddModelError("FilePath", "MP3 bestand is verplicht.");
                return View(song);
            }
            
            // Save the file to wwwroot/music
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "music");
            Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid() + Path.GetExtension(mp3File.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await mp3File.CopyToAsync(stream);
            }

            // Extract duration using NAudio
            using (var reader = new Mp3FileReader(filePath))
            {
                song.Duration = (int)reader.TotalTime.TotalSeconds;
            }

            song.FilePath = "/music/" + fileName;

            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(song);
        }

        // EDIT (GET)
        // GET: /Song/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var song = await _context.Songs.FindAsync(id.Value);
            if (song == null)
                return NotFound();

            return View(song);
        }

        // EDIT (POST)
        // POST: /Song/Edit/{id}
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

        // DELETE (GET)
        // GET: /Song/Delete/{id}
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

        // DELETE (POST)
        // POST: /Song/Delete/{id}
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