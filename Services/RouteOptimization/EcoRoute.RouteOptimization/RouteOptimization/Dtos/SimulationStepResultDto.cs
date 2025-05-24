namespace EcoRoute.RouteOptimization.Dtos
{
    public class SimulationStepResultDto
    {
        public Guid RouteId { get; set; }
        public Guid CompletedStepId { get; set; }
        public string CompletedAddress { get; set; } = string.Empty;
        public int CompletedStepOrder { get; set; }
        public bool IsRouteCompleted { get; set; }
        public int TotalSteps { get; set; }
        public int CompletedSteps { get; set; }
        public double ProgressPercentage { get; set; }
        public Guid? NextStepId { get; set; }
        public string? NextAddress { get; set; }
    }
}
