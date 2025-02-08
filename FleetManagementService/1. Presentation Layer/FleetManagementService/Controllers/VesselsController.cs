using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementServiceCore.BusinessModels;
using FleetManagementServiceCore.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace FleetManagementServiceCore.Controllers
{
    public class VesselsController : Controller
    {
        private readonly FleetManagementServiceCoreDbContext _context;

        public VesselsController(FleetManagementServiceCoreDbContext context)
        {
            _context = context;
        }

        // GET: Vessels
        public async Task<IActionResult> Index()
        {
            var fleetManagementServiceCoreDbContext = _context.Vessels.Include(v => v.Fleet);
            return View(await fleetManagementServiceCoreDbContext.ToListAsync());
        }

        // GET: Vessels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessels
                .Include(v => v.Fleet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }

            return View(vessel);
        }

        // GET: Vessels/Create
        public IActionResult Create()
        {
            ViewData["FleetId"] = new SelectList(_context.Fleets, "Id", "Name");
            return View();
        }

        // POST: Vessels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity,CurrentLoad,FleetId")] Vessel vessel)
        {
            if (ModelState.IsValid)
            {
                // compare load to capacity before performing save to ensure the load is lower than the capacity
                // (check might not needed since the current load field is disabled and increments/decreases whenever a container is assigned to the vessel)
                if (vessel.CurrentLoad > vessel.Capacity)
                {
                    throw new InvalidOperationException("Current Load cannot be bigger than the Capacity.");
                }
                else
                {
                    _context.Add(vessel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["FleetId"] = new SelectList(_context.Fleets, "Id", "Name", vessel.FleetId);
            return View(vessel);
        }

        // GET: Vessels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessels.FindAsync(id);
            if (vessel == null)
            {
                return NotFound();
            }
            ViewData["FleetId"] = new SelectList(_context.Fleets, "Id", "Name", vessel.FleetId);
            return View(vessel);
        }

        // POST: Vessels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity,CurrentLoad,FleetId")] Vessel vessel)
        {
            if (id != vessel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //ToDo: Find why CurrentLoad comes as 0 instead of field value
                    //Temporary Workaround since current load is disabled
                    //***
                    var vesselDb = _context.Vessels.AsNoTracking().Where(e => e.Id == id).FirstOrDefault();
                    if (vesselDb != null)
                        vessel.CurrentLoad = vesselDb.CurrentLoad;
                    //***

                    _context.Update(vessel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VesselExists(vessel.Id))
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
            ViewData["FleetId"] = new SelectList(_context.Fleets, "Id", "Name", vessel.FleetId);
            return View(vessel);
        }

        // GET: Vessels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessels
                .Include(v => v.Fleet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }

            return View(vessel);
        }

        // POST: Vessels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vessel = await _context.Vessels.FindAsync(id);

            try 
            { 
                // check if vessel's currentload is 0 to perform delete if not then vessel cannot be deleted until all containers are offloaded
                if (vessel != null && vessel.CurrentLoad == 0)
                {
                    _context.Vessels.Remove(vessel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new InvalidOperationException("Cannot delete a vessel that has Containers loaded.");
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Delete));
            }
        }

        private bool VesselExists(int id)
        {
            return _context.Vessels.Any(e => e.Id == id);
        }
    }
}
