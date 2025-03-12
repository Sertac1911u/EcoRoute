using EcoRoute.RouteOptimization.Dtos.WaypointDtos;

namespace EcoRoute.RouteOptimization.Dtos.RouteDtos
{
    public class CreateRouteDto
    {
        public Guid? AssignedDriverId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public double TotalDistance { get; set; }
        public double EstimatedFuelConsumption { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
