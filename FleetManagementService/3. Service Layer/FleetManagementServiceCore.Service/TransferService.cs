using FleetManagementServiceCore.BusinessModels;
using FleetManagementServiceCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagementServiceCore.Service
{
    public class TransferService
    {
        private readonly FleetManagementServiceCoreDbContext _context;

        public TransferService(FleetManagementServiceCoreDbContext context)
        {
            _context = context;
        }

        // Allow User to Transfer multiple containers service
        // Validations for Invalid source/destination, no same vessel transfers, no containers found to transfer and destination exceeded
        public async Task TransferMultipleContainers(int sourceVesselId, int destinationVesselId, List<int> containerIds)
        {
            var sourceVessel = await _context.Vessels
                .Include(v => v.Containers)
                .FirstOrDefaultAsync(v => v.Id == sourceVesselId);

            var destinationVessel = await _context.Vessels
                .Include(v => v.Containers)
                .FirstOrDefaultAsync(v => v.Id == destinationVesselId);

            if (sourceVessel == null || destinationVessel == null)
                throw new InvalidOperationException("Invalid source or destination vessel.");

            if (sourceVessel.Id == destinationVessel.Id)
                throw new InvalidOperationException("Cannot transfer load to the same vessel.");

            var containers = await _context.Containers
                .Where(c => containerIds.Contains(c.Id) && c.VesselId == sourceVesselId)
                .ToListAsync();

            if (containers.Count == 0)
                throw new InvalidOperationException("No valid containers found for transfer.");

            if (destinationVessel.CurrentLoad + containers.Count > destinationVessel.Capacity)
                throw new InvalidOperationException("Destination vessel capacity exceeded.");

            // Transfer containers
            foreach (var container in containers)
            {
                sourceVessel.Containers.Remove(container);
                sourceVessel.CurrentLoad--;
                destinationVessel.Containers.Add(container);
                destinationVessel.CurrentLoad++;
                container.VesselId = destinationVesselId;
            }

            await _context.SaveChangesAsync();
        }

        // Fetch all vessels
        public async Task<List<Vessel>> GetAllVesselsAsync()
        {
            return await _context.Vessels.ToListAsync();
        }

        // Fetch all containers
        public async Task<List<Containers>> GetAllContainersAsync()
        {
            return await _context.Containers.ToListAsync();
        }
    }
}
