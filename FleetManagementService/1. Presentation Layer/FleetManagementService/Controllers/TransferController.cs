using FleetManagementServiceCore.Infrastructure;
using FleetManagementServiceCore.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class TransferController : Controller
{
    private readonly TransferService _transferService;
    private readonly FleetManagementServiceCoreDbContext _context;

    public TransferController(TransferService transferService, FleetManagementServiceCoreDbContext context)
    {
        _transferService = transferService;
        _context = context;
    }

    //// GET: Transfer/Index
    //public IActionResult Index()
    //{
    //    return View();
    //}

    // GET: Transfer/TransferContainer
    public async Task<IActionResult> Index()
    {
        // Fetch vessels and containers for dropdowns
        var vessels = await _transferService.GetAllVesselsAsync();
        var containers = await _transferService.GetAllContainersAsync();

        // assign viewbags
        ViewBag.Vessels = vessels;
        ViewBag.Containers = containers;

        return View();
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> TransferContainer(int sourceVesselId, int destinationVesselId, List<int> containerIds)
    {
        try
        {
            // call transferService function to perfom load transfer (could be 1, could be multiple) and do the right validations before the containers are transferred
            await _transferService.TransferMultipleContainers(sourceVesselId, destinationVesselId, containerIds);
            TempData["Message"] = "Containers transferred successfully!";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetContainersByVesselId(int vesselId)
    {
        // retrieve containers based on source vessel selection to display containers of the ship so the user can select which ones to transfer
        var containers = await _context.Containers
            .Where(c => c.VesselId == vesselId)
            .Select(c => new
            {
                id = c.Id,
                name = c.Name,
                vesselName = c.Vessel.Name
            })
            .ToListAsync();

        return Json(containers);
    }
}