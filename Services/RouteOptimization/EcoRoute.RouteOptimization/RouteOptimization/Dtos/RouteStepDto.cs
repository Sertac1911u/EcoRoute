namespace EcoRoute.RouteOptimization.Dtos
{
    public class RouteStepDto
    {
        public Guid Id { get; set; }
        public Guid RouteTaskId { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }

        public Guid WasteBinId { get; set; }

    }
}
