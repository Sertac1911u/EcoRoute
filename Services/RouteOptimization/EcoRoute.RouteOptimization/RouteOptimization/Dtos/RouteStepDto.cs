namespace EcoRoute.RouteOptimization.Dtos
{
    public class RouteStepDto
    {
        public Guid Id { get; set; }
        public Guid RouteTaskId { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }

        public Guid? WasteBinId { get; set; }

        public bool IsDepotStep => !WasteBinId.HasValue;
        public bool IsWasteBinStep => WasteBinId.HasValue;
        public string StepType => Order switch
        {
            0 => "START",
            _ when IsDepotStep => "END",
            _ when IsWasteBinStep => "WASTE_BIN",
            _ => "UNKNOWN"
        };
    }
}
