using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Dtos
{
    public class RouteResultDto
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public WasteType WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public RouteStatus Status { get; set; }
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }
        public string? OverviewPolyline { get; set; }
        public string? RouteName { get; set; }
        public string? Notes { get; set; }
        public List<RouteStepDto> Steps { get; set; } = new List<RouteStepDto>();

        // Computed properties
        public int TotalSteps => Steps?.Count ?? 0;
        public int CompletedSteps => Steps?.Count(s => s.IsCompleted) ?? 0;
        public int RemainingSteps => TotalSteps - CompletedSteps;
        public double ProgressPercentage => TotalSteps > 0 ? (CompletedSteps / (double)TotalSteps) * 100 : 0;
    }
}
