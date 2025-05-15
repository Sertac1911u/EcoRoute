namespace EcoRoute.RouteOptimization.Entities
{
    public class RouteTask
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public List<RouteStep> Steps { get; set; } = new();
        public string WasteType { get; set; }
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

    public enum OptimizationType
    {
        EnKisaMesafe,
        DolulukOncelikli
    }
}
