using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class CreateRouteDto
    {
        public string RouteName { get; set; }
        public string VehicleId { get; set; } = string.Empty; // Changed from Guid to string
        public string DriverId { get; set; } = string.Empty; // Changed from Guid to string
        public OptimizationType OptimizationType { get; set; }
        public WasteType WasteType { get; set; }
        public DateTime ScheduledStart { get; set; } = DateTime.Now;
        public double StartLatitude { get; set; } = 41.181;  // Çorlu
        public double StartLongitude { get; set; } = 27.82;

        public double EndLatitude { get; set; } = 41.181;
        public double EndLongitude { get; set; } = 27.82;
        public string Notes { get; set; } = string.Empty;
        public List<Guid> WasteBinIds { get; set; } = new();
        public bool AutoOptimize { get; set; } = true;
    }

}
