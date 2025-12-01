using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jukeboxmpa.Controllers
{
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. READ (Overzicht):
        // GET: /Song/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Songs.ToListAsync());
        }

        // 2. CREATE (GET):
        // GET: /Song/Create
        public IActionResult Create()
        {
            return View();
        }

        // 3. CREATE (POST):
        // POST: /Song/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Artist,Album,FilePath")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(song);
        }

        // 4. EDIT (GET):
        // GET: /Song/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var song = await _context.Songs.FindAsync(id.Value);
            if (song == null)
                return NotFound();

            return View(song);
        }

        // 5. EDIT (POST):
        // POST: /Song/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Artist,Album,FilePath")] Song song)
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

        // 6. DELETE (GET):
        // GET: /Song/Delete
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

        // 7. DELETE (POST):
        // POST: /Song/Delete
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