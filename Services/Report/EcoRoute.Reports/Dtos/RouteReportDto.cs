namespace EcoRoute.Reports.Dtos
{
    public class RouteReportDto
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalDistanceKm { get; set; }
        public double CO2EmissionKg { get; set; }
    }
}
