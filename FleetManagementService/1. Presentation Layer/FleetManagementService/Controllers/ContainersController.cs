using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementServiceCore.BusinessModels;
using FleetManagementServiceCore.Infrastructure;
using System.ComponentModel;

namespace FleetManagementServiceCore.Controllers
{
    public class ContainersController : Controller
    {
        private readonly FleetManagementServiceCoreDbContext _context;

        public ContainersController(FleetManagementServiceCoreDbContext context)
        {
            _context = context;
        }

        // GET: Containers
        public async Task<IActionResult> Index()
        {
            var fleetManagementServiceCoreDbContext = _context.Containers.Include(c => c.Vessel);
            return View(await fleetManagementServiceCoreDbContext.ToListAsync());
        }

        // GET: Containers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // check if container exists then display details
            var containers = await _context.Containers
                .Include(c => c.Vessel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (containers == null)
            {
                return NotFound();
            }

            return View(containers);
        }

        // GET: Containers/Create
        public IActionResult Create()
        {
            ViewData["VesselId"] = new SelectList(_context.Vessels, "Id", "Name");
            return View();
        }

        // POST: Containers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,VesselId")] Containers containers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Find vessel
                    var vessel = await _context.Vessels.FindAsync(containers.VesselId);
                    // Perform validation to check capacity of vessel before loading container
                    if (vessel == null || vessel.CurrentLoad >= vessel.Capacity)
                    {
                        throw new InvalidOperationException("Vessel not found or capacity exceeded.");
                    }
                    else
                    {
                        //Perform action and save state of DB then redirect
                        _context.Add(containers);
                        vessel.CurrentLoad++;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                // set VesselId viewData
                ViewData["VesselId"] = new SelectList(_context.Vessels, "Id", "Name", containers.VesselId);
                return View(containers);
            }
            catch (InvalidOperationException ex)
            {
                // display error in case an exception is thrown
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Create));
            }

        }

        // GET: Containers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // check if container exists then display edit page
            var containers = await _context.Containers.FindAsync(id);
            if (containers == null)
            {
                return NotFound();
            }
            ViewData["VesselId"] = new SelectList(_context.Vessels, "Id", "Name", containers.VesselId);
            return View(containers);
        }

        // POST: Containers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,VesselId")] Containers containers)
        {
            if (id != containers.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // retrieve original container and new vessel info
                        var ogContainer = _context.Containers.AsNoTracking().FirstOrDefault(e=>e.Id == containers.Id);
                        var newVessel = await _context.Vessels.FindAsync(containers.VesselId);
                        
                        // change current load between old and new vessel if vesselid of record and new vesselid does not match
                        if (ogContainer != null && ogContainer.VesselId != containers.VesselId)
                        {
                            if (newVessel != null && newVessel.CurrentLoad >= newVessel.Capacity)
                                throw new InvalidOperationException("Vessel not found or capacity exceeded.");
                            else
                                newVessel.CurrentLoad++;

                            var oldVessel = await _context.Vessels.FindAsync(ogContainer.VesselId);

                            if (oldVessel != null)
                                oldVessel.CurrentLoad--;
                        }

                        // perform update
                        _context.Update(containers);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ContainersExists(containers.Id))
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
                ViewData["VesselId"] = new SelectList(_context.Vessels, "Id", "Name", containers.VesselId);
                return View(containers);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: Containers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // check if object exists in db then show details in delete page
            var containers = await _context.Containers
                .Include(c => c.Vessel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (containers == null)
            {
                return NotFound();
            }

            return View(containers);
        }

        // POST: Containers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // retreive container and vessel, deduct current load from vessel and delete container
            var containers = await _context.Containers.FindAsync(id);
            if (containers != null)
            {
                var vessel = await _context.Vessels.FindAsync(containers.VesselId);

                if (vessel.CurrentLoad > 0)
                    vessel.CurrentLoad--;

                _context.Containers.Remove(containers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContainersExists(int id)
        {
            // check if container exists
            return _context.Containers.Any(e => e.Id == id);
        }
    }
}
