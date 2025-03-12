namespace EcoRoute.RouteOptimization.Entities
{
    public class Waypoint
    {
        public Guid WaypointId { get; set; }
        public Guid RouteId { get; set; }
        public string Location { get; set; }
        public int Sequence { get; set; }
        public string AdditionalInfo { get; set; }
        public MyRoute Route { get; set; }
    }
}
