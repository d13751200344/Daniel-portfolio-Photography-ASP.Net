using Microsoft.AspNetCore.Mvc;
using Photography.Models;

namespace Photography.Controllers
{
    public class GalleryController : Controller
    {
        /*
        private List<Gallery> Gallery;
        public GalleryController()
        {
            Gallery = new List<Gallery>
            {
                new Gallery{ Id = 1, GalleryName = "Product", Description = "Product images."},
                new Gallery{ Id = 2, GalleryName = "Food", Description = "Food pictures."},
                new Gallery{ Id = 3, GalleryName = "Property", Description = "Design and houses."},
            };
        }
        */

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Food()
        {
            return View();
        }

        public IActionResult Wedding()
        {
            return View();
        }

        public IActionResult Property()
        {
            return View();
        }

    }
}
