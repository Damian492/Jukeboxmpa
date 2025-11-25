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


    }
}