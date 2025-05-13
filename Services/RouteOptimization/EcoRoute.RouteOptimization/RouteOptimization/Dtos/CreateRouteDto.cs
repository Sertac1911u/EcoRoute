namespace EcoRoute.RouteOptimization.Dtos
{
    public class CreateRouteDto
    {
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public string WasteType { get; set; }
        public string OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public List<Guid> WasteBinIds { get; set; }
        public string Notes { get; set; }
    }
}
