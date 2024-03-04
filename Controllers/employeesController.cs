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
    public class employeesController : Controller
    {
        private readonly bicycleStoreContext _context;

        public employeesController(bicycleStoreContext context)
        {
            _context = context;
        }

        // GET: employees
        public async Task<IActionResult> Index()
        {
              return _context.employees != null ? 
                          View(await _context.employees.ToListAsync()) :
                          Problem("Entity set 'bicycleStoreContext.employees'  is null.");
        }

        // GET: employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employee = await _context.employees
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Username,Password,Name")] employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Username,Password,Name")] employee employee)
        {
            if (id != employee.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!employeeExists(employee.id))
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
            return View(employee);
        }

        // GET: employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employee = await _context.employees
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.employees == null)
            {
                return Problem("Entity set 'bicycleStoreContext.employees'  is null.");
            }
            var employee = await _context.employees.FindAsync(id);
            if (employee != null)
            {
                _context.employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool employeeExists(int id)
        {
          return (_context.employees?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
