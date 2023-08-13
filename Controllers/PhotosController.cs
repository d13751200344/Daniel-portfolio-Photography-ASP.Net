using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Photography.Data;
using Photography.Models;

namespace Photography.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Photos
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Photos
                .OrderBy(image => image.Id)
                .Include(p => p.Gallery);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Gallery)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "GalleryName");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,GalleryId,Title,Caption")] Photo photo, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                photo.Image = await UploadPhoto(Image);
                // Do this first, hover over "UploadPhoto()", AND generate method

                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "GalleryName", photo.GalleryId);
            return View(photo);
        }

        [Authorize(Roles = "Administrator")]
        private async Task<string> UploadPhoto(IFormFile Image)
        {
            // GTFO ASAP Principle
            if (Image == null) return null;

            // Get temp photo location
            var filePath = Path.GetTempFileName();

            // Create a unique file name (use Guid)
            var fileName = Guid.NewGuid() + "-" + Image.FileName;

            // Set permanent destination for the photo
            var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\images\\photos\\" + fileName;

            // Execute the file copy and move it to permanent location
            using var stream = new FileStream(uploadPath, FileMode.Create);
            await Image.CopyToAsync(stream);

            // Return the filename so we can find the photo based on it
            return fileName;
        }



        // GET: Photos/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "GalleryName", photo.GalleryId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GalleryId,Title,Caption")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPhoto = await _context.Photos.FindAsync(id);

                    // Update non-image properties from the edited photo to the existing photo
                    existingPhoto.Title = photo.Title;
                    existingPhoto.Caption = photo.Caption;
                    existingPhoto.GalleryId = photo.GalleryId;

                    _context.Update(existingPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "GalleryName", photo.GalleryId);
            return View(photo);
        }



        // GET: Photos/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Gallery)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Photos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Photos'  is null.");
            }
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
          return (_context.Photos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
