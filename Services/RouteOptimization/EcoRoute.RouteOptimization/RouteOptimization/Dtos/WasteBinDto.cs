namespace EcoRoute.RouteOptimization.Dtos
{
    public class WasteBinDto
    {
        public Guid Id { get; set; } 
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? FillLevel { get; set; }
    }
}
