namespace EcoRoute.RouteOptimization.Dtos
{
    public class RouteLiveStatusDto
    {
        public int TotalSteps { get; set; }
        public int CompletedSteps { get; set; }
        public double ProgressPercentage { get; set; }
        public string CurrentAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int EstimatedTimeRemainingMin { get; set; }
        public int ElapsedTimeMin { get; set; }
    }
}
