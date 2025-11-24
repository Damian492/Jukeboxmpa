using Jukeboxmpa.Data;
using Jukeboxmpa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jukeboxmpa.Controllers
{
    // De Controller handel alle verzoeken mte song 
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor toegang met database krijgn
        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        //alle liedjes
        // GET: /Song/Index
        public async Task<IActionResult> Index()
        {
            // Haalt alle liedjesop uit de database
            // .Include() artiesten en albums tonen
            return View(await _context.Songs.ToListAsync());
        }

        // 2. CREATE (GET): song toevoegne
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