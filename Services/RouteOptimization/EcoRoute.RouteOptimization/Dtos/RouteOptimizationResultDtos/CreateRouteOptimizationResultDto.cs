namespace EcoRoute.RouteOptimization.Dtos.RouteOptimizationResultDtos
{
    public class CreateRouteOptimizationResultDto
    {
        public Guid RouteId { get; set; }
        public string OptimizedPath { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan EstimatedTravelTime { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
