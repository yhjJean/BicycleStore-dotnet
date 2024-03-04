using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BicycleStore.Models;

namespace BicycleStore.Controllers
{
    public class bicyclesController : Controller
    {
        private readonly bicycleStoreContext _context;

        public bicyclesController(bicycleStoreContext context)
        {
            _context = context;
        }

        // GET: bicycles
        public async Task<IActionResult> Index()
        {
              return _context.bicycles != null ? 
                          View(await _context.bicycles.ToListAsync()) :
                          Problem("Entity set 'bicycleStoreContext.bicycles'  is null.");
        }

        // GET: bicycles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.bicycles == null)
            {
                return NotFound();
            }

            var bicycle = await _context.bicycles
                .FirstOrDefaultAsync(m => m.id == id);
            if (bicycle == null)
            {
                return NotFound();
            }

            return View(bicycle);
        }

        // GET: bicycles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: bicycles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Type,Status")] bicycle bicycle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bicycle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bicycle);
        }

        // GET: bicycles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.bicycles == null)
            {
                return NotFound();
            }

            var bicycle = await _context.bicycles.FindAsync(id);
            if (bicycle == null)
            {
                return NotFound();
            }
            return View(bicycle);
        }

        // POST: bicycles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Type,Status")] bicycle bicycle)
        {
            if (id != bicycle.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bicycle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bicycleExists(bicycle.id))
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
            return View(bicycle);
        }

        // GET: bicycles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bicycles == null)
            {
                return NotFound();
            }

            var bicycle = await _context.bicycles
                .FirstOrDefaultAsync(m => m.id == id);
            if (bicycle == null)
            {
                return NotFound();
            }

            return View(bicycle);
        }

        // POST: bicycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.bicycles == null)
            {
                return Problem("Entity set 'bicycleStoreContext.bicycles'  is null.");
            }
            var bicycle = await _context.bicycles.FindAsync(id);
            if (bicycle != null)
            {
                _context.bicycles.Remove(bicycle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool bicycleExists(int id)
        {
          return (_context.bicycles?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
