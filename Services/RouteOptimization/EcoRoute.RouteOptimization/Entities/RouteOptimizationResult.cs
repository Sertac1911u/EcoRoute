namespace EcoRoute.RouteOptimization.Entities
{
    public class RouteOptimizationResult
    {
        public Guid RouteOptimizationResultId { get; set; }
        public Guid RouteId { get; set; }
        public string OptimizedPath { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan EstimatedTravelTime { get; set; }
        public DateTime CalculatedAt { get; set; }


        public MyRoute Route { get; set; }
    }
}
