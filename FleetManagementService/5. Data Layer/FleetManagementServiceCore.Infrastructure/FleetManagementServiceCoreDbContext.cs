using FleetManagementServiceCore.BusinessModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace FleetManagementServiceCore.Infrastructure
{
    public class FleetManagementServiceCoreDbContext : DbContext
    {
        // DB Tables
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<Containers> Containers { get; set; }
        public DbSet<Fleet> Fleets { get; set; }

        //DB Constructor
        public FleetManagementServiceCoreDbContext(DbContextOptions<FleetManagementServiceCoreDbContext> options)
        : base(options)
            {
            }
    }
}
