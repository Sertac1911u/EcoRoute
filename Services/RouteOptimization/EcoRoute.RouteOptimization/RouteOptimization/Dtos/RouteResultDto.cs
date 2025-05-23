using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Dtos
{
    public class RouteResultDto
    {
        public Guid Id { get; set; }
        public string RouteName { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public WasteType WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public RouteStatus Status { get; set; }
        public string? OverviewPolyline { get; set; }
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }


        public List<RouteStepDto> Steps { get; set; }
    }
}
