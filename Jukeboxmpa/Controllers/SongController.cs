using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Jukeboxmpa.Controllers
{
    // Controller that handles CRUD operations for Song entities.
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ (Overview) with optional genre filter
        // GET: /Song/Index?genre=Rock
        public async Task<IActionResult> Index(string? genre)
        {
            // Build a distinct list of genres for the filter dropdown.
            var genresQuery = _context.Songs
                .Select(s => s.Genre)
                .Where(g => g != null)
                .Distinct()
                .OrderBy(g => g);

            ViewBag.Genres = new SelectList(await genresQuery.ToListAsync());

            // Query songs and apply optional filter.
            var songs = _context.Songs.AsQueryable();
            if (!string.IsNullOrEmpty(genre))
            {
                songs = songs.Where(s => s.Genre == genre);
            }

            return View(await songs.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Title,Artist,Album,FilePath,Genre")] Song song)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Artist,Album,FilePath,Genre")] Song song)
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