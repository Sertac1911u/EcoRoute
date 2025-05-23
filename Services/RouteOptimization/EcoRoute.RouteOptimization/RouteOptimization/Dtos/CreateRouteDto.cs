using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Dtos
{
    public class CreateRouteDto
    {
        public string RouteName { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public WasteType WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public List<Guid> WasteBinIds { get; set; }
        public string Notes { get; set; }
    }
}
