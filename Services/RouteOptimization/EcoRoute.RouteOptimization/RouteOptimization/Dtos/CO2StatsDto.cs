namespace EcoRoute.RouteOptimization.Dtos
{
    public class DailyCO2Stat
    {
        public DateTime Date { get; set; }
        public double CO2Kg { get; set; }
        public int RouteCount { get; set; }
    }

    public class CO2StatsDto
    {
        public double TotalCO2Kg { get; set; }
        public int TotalRoutes { get; set; }
        public double TotalDistanceKm { get; set; }
        public double AverageCO2PerRouteKg { get; set; }
        public List<DailyCO2Stat> DailyStats { get; set; }
    }
}
