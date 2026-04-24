using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly AppDbContext _context;

        private readonly BlobService _blobService;

        public VenuesController(AppDbContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchString)
        {
            var venues = _context.Venues.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(v =>
                    v.VenueName.Contains(searchString) ||
                    v.Location.Contains(searchString));
            }

            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            // Enforce image upload
            if (venue.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image upload is required");
            }

            if (ModelState.IsValid)
            {
                venue.ImageUrl = await _blobService.UploadFileAsync(venue.ImageFile);

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            // Get existing record from DB
            var existingVenue = await _context.Venues.AsNoTracking()
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (existingVenue == null)
            {
                return NotFound();
            }

            // If new image uploaded → replace
            if (venue.ImageFile != null)
            {
                venue.ImageUrl = await _blobService.UploadFileAsync(venue.ImageFile);
            }
            else
            {
                // Keep existing image (IMPORTANT)
                venue.ImageUrl = existingVenue.ImageUrl;
            }

            // Final safety check (image must exist)
            if (string.IsNullOrEmpty(venue.ImageUrl))
            {
                ModelState.AddModelError("ImageFile", "Image is required");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
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

            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);

            var hasBookings = _context.Bookings.Any(b => b.VenueId == id);

            if (hasBookings)
            {
                ViewBag.Error = "Cannot delete venue with existing bookings.";
                return View(venue); // stay on delete page
            }

            if (venue != null)
            {
                // Delete image from Azure
                if (!string.IsNullOrEmpty(venue.ImageUrl))
                {
                    await _blobService.DeleteFileAsync(venue.ImageUrl);
                }

                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }

    }
}
