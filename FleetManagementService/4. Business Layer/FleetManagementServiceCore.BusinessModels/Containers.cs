namespace FleetManagementServiceCore.BusinessModels
{
    public class Containers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? VesselId { get; set; } // Foreign Key to Vessels
        public Vessel? Vessel { get; set; } // Vessel Object
    }
}
