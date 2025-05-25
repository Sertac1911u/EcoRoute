namespace EcoRoute.Reports.Dtos
{
    public class RoutePerformanceReportDto
    {
        public Guid RouteId { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public string? DriverName { get; set; }

        // Eski DTO alanları
        public double DistanceKm { get; set; }          // total km
        public TimeSpan Duration { get; set; }          // gerçek süre
        public double EfficiencyScore { get; set; }          // %100 = plan/gerçek mükemmel
        public DateTime? CompletedAt { get; set; }

        // Yeni DTO alanları
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }
        public int StepCount { get; set; }
        public int CompletedStepCount { get; set; }
    }
}
