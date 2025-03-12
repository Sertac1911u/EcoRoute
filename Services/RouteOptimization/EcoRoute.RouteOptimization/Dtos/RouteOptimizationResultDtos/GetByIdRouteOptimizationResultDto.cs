namespace EcoRoute.RouteOptimization.Dtos.RouteOptimizationResultDtos
{
    public class GetByIdRouteOptimizationResultDto
    {
        public Guid ResultId { get; set; }
        public Guid RouteId { get; set; }
        public string OptimizedPath { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan EstimatedTravelTime { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
