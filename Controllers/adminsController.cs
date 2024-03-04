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
    public class adminsController : Controller
    {
        private readonly bicycleStoreContext _context;

        public adminsController(bicycleStoreContext context)
        {
            _context = context;
        }

        // GET: admins
        public async Task<IActionResult> Index()
        {
              return _context.admins != null ? 
                          View(await _context.admins.ToListAsync()) :
                          Problem("Entity set 'bicycleStoreContext.admins'  is null.");
        }

        // GET: admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.admins == null)
            {
                return NotFound();
            }

            var admin = await _context.admins
                .FirstOrDefaultAsync(m => m.id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Password,Username")] admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.admins == null)
            {
                return NotFound();
            }

            var admin = await _context.admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Password,Username")] admin admin)
        {
            if (id != admin.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!adminExists(admin.id))
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
            return View(admin);
        }

        // GET: admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.admins == null)
            {
                return NotFound();
            }

            var admin = await _context.admins
                .FirstOrDefaultAsync(m => m.id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.admins == null)
            {
                return Problem("Entity set 'bicycleStoreContext.admins'  is null.");
            }
            var admin = await _context.admins.FindAsync(id);
            if (admin != null)
            {
                _context.admins.Remove(admin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool adminExists(int id)
        {
          return (_context.admins?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
