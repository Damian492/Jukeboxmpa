using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jukeboxmpa.Data;
using Jukeboxmpa.Models;

namespace Jukeboxmpa.Controllers
{
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context) // Constructor Injectie
        {
            _context = context;
        }

        // GET: Song/Index
        public async Task<IActionResult> Index()
        {
            // Haal alle liedjes uit de 'Songs' tabel op en stuur ze naar de View
            return View(await _context.Songs.ToListAsync());
        }

        // GET: Song/Create
        public IActionResult Create()
        {
            //een nieuw song in te voeren
            return View();
        }

        // POST: Song/Create
    }
}