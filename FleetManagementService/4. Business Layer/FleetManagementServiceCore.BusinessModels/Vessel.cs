using System.ComponentModel;

namespace FleetManagementServiceCore.BusinessModels
{
    public class Vessel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; } // Maximum number of containers it can hold
        public int CurrentLoad { get; set; } // Current number of containers loaded
        public int? FleetId { get; set; } // Foreign key to Fleet
        public Fleet? Fleet { get; set; } // Fleet Object
        public List<Containers>? Containers { get; set; } = new List<Containers>(); // Containers List (not needed for now)
    }
}
