using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class ApiCreateRouteDto
    {
        public string RouteName { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public WasteType WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public List<Guid> WasteBinIds { get; set; }
        public string Notes { get; set; }
    }
}
