using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BicycleStore.Models;
using System.Security.Claims;

namespace BicycleStore.Controllers
{
    public class rentalsController : Controller
    {
        private readonly bicycleStoreContext _context;
        public rentalsController(bicycleStoreContext context)
        {
            _context = context;
        }

        // GET: rentals
        public async Task<IActionResult> Index()
        {
            var bicycleStoreContext = _context.rentals.Include(r => r.Bicycle).Include(r => r.CreatedByAdminNavigation).Include(r => r.CreatedByEmployeeNavigation);
            return View(await bicycleStoreContext.ToListAsync());
        }

        // GET: rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.rentals
                .Include(r => r.Bicycle)
                .Include(r => r.CreatedByAdminNavigation)
                .Include(r => r.CreatedByEmployeeNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: rentals/Create
        public IActionResult Create()
        {
            var availableBicycles = _context.bicycles.Where(b => b.Status == 1).OrderBy(b => b.Type);
            ViewData["BicycleId"] = new SelectList(availableBicycles, "id", "Type");

            //ViewData["CreatedByAdmin"] = new SelectList(_context.admins, "id", "id");
            //ViewData["CreatedByEmployee"] = new SelectList(_context.employees, "id", "id");
            return View();
        }

        // POST: rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,MatricNo,PhoneNo,RentalStartDay,RentalEndDay,BicycleId, CreatedByAdmin, CreatedByEmployee")] rental rental)
        {
            if (ModelState.IsValid)
            {
                var selectedBicycle = await _context.bicycles.FirstOrDefaultAsync(b => b.id == rental.BicycleId);
                if (selectedBicycle != null)
                {
                    selectedBicycle.Status = 0; // Update the status of the selected bicycle
                    _context.Update(selectedBicycle);
                }

                // calculate the rental fee using rental end date and rental start date
                TimeSpan rentalDuration = rental.RentalEndDay - rental.RentalStartDay;
                int rentalDays = rentalDuration.Days + 1;

                rental.RentalFee = rentalDays * 10.00m;

                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BicycleId"] = new SelectList(_context.bicycles, "id", "id", rental.BicycleId);
            return View(rental);
        }

        // GET: rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            var selectedBicycle = await _context.bicycles.FirstOrDefaultAsync(b => b.id == rental.BicycleId);
            var availableBicycles = _context.bicycles.Where(b => b.Status == 1 || b.id == rental.BicycleId).OrderBy(b => b.Type);

            ViewData["BicycleId"] = new SelectList(availableBicycles, "id", "Type", rental.BicycleId);

            ViewData["CreatedByAdmin"] = new SelectList(_context.admins, "id", "id", rental.CreatedByAdmin);
            ViewData["CreatedByEmployee"] = new SelectList(_context.employees, "id", "id", rental.CreatedByEmployee);
            return View(rental);
        }

        // POST: rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,MatricNo,PhoneNo,RentalStartDay,RentalEndDay,BicycleId, CreatedByAdmin, CreatedByEmployee")] rental rental)
        {
            if (id != rental.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var selectedBicycle = await _context.bicycles.FirstOrDefaultAsync(b => b.id == rental.BicycleId);
                if (selectedBicycle != null)
                {
                    // Check if the bicycle has changed
                    var originalRental = await _context.rentals.AsNoTracking().FirstOrDefaultAsync(r => r.id == rental.id);
                    if (originalRental.BicycleId != rental.BicycleId)
                    {
                        var previousBicycle = await _context.bicycles.FirstOrDefaultAsync(b => b.id == originalRental.BicycleId);
                        if (previousBicycle != null)
                        {
                            previousBicycle.Status = 1; // Set the status of the previous bicycle to available
                            _context.Update(previousBicycle);
                        }

                        selectedBicycle.Status = 0; // Update the status of the selected bicycle
                        _context.Update(selectedBicycle);
                    }
                }

                // calculate the rental fee using rental end date and rental start date
                TimeSpan rentalDuration = rental.RentalEndDay - rental.RentalStartDay;
                int rentalDays = rentalDuration.Days + 1;

                rental.RentalFee = rentalDays * 10.00m;

                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rentalExists(rental.id))
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
            ViewData["BicycleId"] = new SelectList(_context.bicycles, "id", "id", rental.BicycleId);
            //ViewData["CreatedByAdmin"] = new SelectList(_context.admins, "id", "id", rental.CreatedByAdmin);
            //ViewData["CreatedByEmployee"] = new SelectList(_context.employees, "id", "id", rental.CreatedByEmployee);
            return View(rental);
        }

        // GET: rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.rentals
                .Include(r => r.Bicycle)
                .Include(r => r.CreatedByAdminNavigation)
                .Include(r => r.CreatedByEmployeeNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.rentals == null)
            {
                return Problem("Entity set 'bicycleStoreContext.rentals'  is null.");
            }
            var rental = await _context.rentals.FindAsync(id);
            if (rental != null)
            {
                var selectedBicycle = await _context.bicycles.FirstOrDefaultAsync(b => b.id == rental.BicycleId);
                if (selectedBicycle != null)
                {
                    // Check if the bicycle has changed
                    var originalRental = await _context.rentals.AsNoTracking().FirstOrDefaultAsync(r => r.id == rental.id);
                    if (originalRental.BicycleId != rental.BicycleId)
                    {
                        var previousBicycle = await _context.bicycles.FirstOrDefaultAsync(b => b.id == originalRental.BicycleId);

                        if (previousBicycle != null)
                        {
                            previousBicycle.Status = 1; // Set the status of the previous bicycle to available
                            _context.Update(previousBicycle);
                        }
                    }
                    selectedBicycle.Status = 1; // Update the status of the selected bicycle
                    _context.Update(selectedBicycle);
                }
                _context.rentals.Remove(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool rentalExists(int id)
        {
          return (_context.rentals?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
