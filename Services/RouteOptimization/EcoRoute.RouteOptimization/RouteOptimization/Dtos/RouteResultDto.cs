namespace EcoRoute.RouteOptimization.Dtos
{
    public class RouteResultDto
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public string WasteType { get; set; }
        public string OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public string Status { get; set; }
        public string? OverviewPolyline { get; set; }

        public List<RouteStepDto> Steps { get; set; }
    }
}
