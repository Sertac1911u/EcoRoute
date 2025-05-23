namespace EcoRoute.RouteOptimization.Entities
{
    public class RouteTask
    {
        public Guid Id { get; set; }
        public string RouteName { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public List<RouteStep> Steps { get; set; } = new();
        public WasteType WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public RouteStatus Status { get; set; } = RouteStatus.Scheduled;
        public string Notes { get; set; }
        public string? OverviewPolyline { get; set; }
        public DateTime CreatedAt { get; set; }
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }

    }
    public enum RouteStatus
    {
        Scheduled,
        Active,
        Completed
    }
    public enum WasteType
    {
        Cop = 1,           // Çöp
        GeriDonusum = 2,   // Geri Dönüşüm
        Organik = 3,       // Organik Atık
        Cam = 4,           // Cam
        Metal = 5,         // Metal
        Elektronik = 6,    // Elektronik Atık
        Tehlikeli = 7      // Tehlikeli Atık
    }

    public enum OptimizationType
    {
        EnKisaMesafe,
        DolulukOncelikli
    }
}
