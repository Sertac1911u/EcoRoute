namespace EcoRoute.RouteOptimization.Dtos.WaypointDtos
{
    public class UpdateWaypointDto
    {
        public Guid WaypointId { get; set; }
        public Guid RouteId { get; set; }
        public string Location { get; set; }
        public int Sequence { get; set; }
        public string AdditionalInfo { get; set; }
        public Route Route { get; set; }
    }
}
