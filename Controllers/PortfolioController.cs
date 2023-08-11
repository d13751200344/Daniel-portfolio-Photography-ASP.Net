using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography.Data;

namespace Photography.Controllers
{
    public class PortfolioController : Controller
    {
        private ApplicationDbContext _context;

        public PortfolioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var galleries = await _context.Gallery
                .OrderBy(gallery => gallery.Id)
                .ToListAsync();

            return View(galleries);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var galleryWithPhotos = await _context.Gallery
                .Include(gallery => gallery.Photos)
                .FirstOrDefaultAsync(gallery => gallery.Id == id);

            return View(galleryWithPhotos);
        }
    }
}
