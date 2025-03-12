namespace EcoRoute.RouteOptimization.Entities
{
    public class MyRoute
    {
        public Guid MyRouteId { get; set; }
        public Guid? AssignedDriverId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public double TotalDistance { get; set; }
        public double EstimatedFuelConsumption { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public ICollection<Waypoint> Waypoints { get; set; }

    }
}
