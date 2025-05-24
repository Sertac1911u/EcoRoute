using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class ApiCreateRouteDto
    {
        [Required]
        public string DriverId { get; set; } = string.Empty;

        [Required]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        public WasteType WasteType { get; set; }

        [Required]
        public OptimizationType OptimizationType { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [MinLength(1)]
        public List<Guid> WasteBinIds { get; set; } = new List<Guid>();

        public string? Notes { get; set; }
        public string? RouteName { get; set; }
    }
}
