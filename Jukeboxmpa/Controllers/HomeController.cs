using System.Diagnostics;
using Jukeboxmpa.Models;
using Jukeboxmpa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jukeboxmpa.Controllers
{
    // Standard Home controller for basic site pages and error handling.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index(string search)
        {
            var playlists = _context.Playlists
                .Where(p => p.IsPublic);

            if (!string.IsNullOrEmpty(search))
                playlists = playlists.Where(p => p.Name.Contains(search));

            ViewBag.PublicPlaylists = await playlists.ToListAsync();
            return View();
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Error page action - uses ErrorViewModel to show request id for diagnostics.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
