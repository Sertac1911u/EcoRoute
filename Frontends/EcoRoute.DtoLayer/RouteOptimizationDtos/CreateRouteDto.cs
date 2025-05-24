using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class CreateRouteDto
    {
        [Required]
        public string DriverId { get; set; } = string.Empty;

        [Required]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        public WasteType WasteType { get; set; } = WasteType.GeriDonusum;

        [Required]
        public OptimizationType OptimizationType { get; set; } = OptimizationType.EnKisaMesafe;

        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        [MinLength(1, ErrorMessage = "En az bir atık kutusu seçilmelidir")]
        public List<Guid> WasteBinIds { get; set; } = new List<Guid>();

        public string? Notes { get; set; }
        public string? RouteName { get; set; }

        // Fixed coordinates for Çorlu
        public double StartLatitude { get; set; } = 41.1634;
        public double StartLongitude { get; set; } = 27.7951;
        public double EndLatitude { get; set; } = 41.1634;
        public double EndLongitude { get; set; } = 27.7951;

        public bool AutoOptimize { get; set; } = true;
        public DateTime ScheduledStart { get; set; } = DateTime.Now;
    }

}
