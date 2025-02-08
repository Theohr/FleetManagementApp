using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetManagementServiceCore.Infrastructure;
using FleetManagementServiceCore.Models;

namespace FleetManagementServiceCore.Controllers
{
    public class FleetsController : Controller
    {
        private readonly FleetManagementServiceCoreDbContext _context;

        public FleetsController(FleetManagementServiceCoreDbContext context)
        {
            _context = context;
        }

        // GET: Fleets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fleets.ToListAsync());
        }

        // GET: Fleets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fleet = await _context.Fleets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fleet == null)
            {
                return NotFound();
            }

            return View(fleet);
        }

        // GET: Fleets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fleets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Fleet fleet)
        {
            // check if fleet is valid then perform create
            if (ModelState.IsValid)
            {
                _context.Add(fleet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fleet);
        }

        // GET: Fleets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fleet = await _context.Fleets.FindAsync(id);
            if (fleet == null)
            {
                return NotFound();
            }
            return View(fleet);
        }

        // POST: Fleets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Fleet fleet)
        {
            if (id != fleet.Id)
            {
                return NotFound();
            }

            // perform update of current fleet
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fleet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FleetExists(fleet.Id))
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
            return View(fleet);
        }

        // GET: Fleets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fleet = await _context.Fleets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fleet == null)
            {
                return NotFound();
            }

            return View(fleet);
        }

        // POST: Fleets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vessels = _context.Vessels.Where(e=>e.FleetId == id);

            try
            {
                // check if fleet has any vessels assigned to it, if yes do not perform delete
                if (vessels != null && vessels.Count() > 0)
                {
                    throw new InvalidOperationException("Cannot delete a fleet that has vessels assigned to it.");
                }
                else
                {
                    var fleet = await _context.Fleets.FindAsync(id);

                    if (fleet != null)
                    {
                        _context.Fleets.Remove(fleet);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Delete));
            }
        }

        private bool FleetExists(int id)
        {
            // check if fleet exists
            return _context.Fleets.Any(e => e.Id == id);
        }
    }
}
