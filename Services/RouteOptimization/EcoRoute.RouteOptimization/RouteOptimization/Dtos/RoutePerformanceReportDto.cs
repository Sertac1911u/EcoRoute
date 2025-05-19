namespace EcoRoute.RouteOptimization.Dtos
{
    public class RoutePerformanceReportDto
    {
        public Guid RouteId { get; set; }
        public string DriverId { get; set; }
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }
        public int StepCount { get; set; }
        public int CompletedStepCount { get; set; }

    }
}
