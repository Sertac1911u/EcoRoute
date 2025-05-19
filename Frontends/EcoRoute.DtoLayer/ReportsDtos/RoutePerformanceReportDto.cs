using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.ReportsDtos
{
    public class RoutePerformanceReportDto
    {
        public Guid RouteId { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public double DistanceKm { get; set; }
        public TimeSpan Duration { get; set; }
        public double EfficiencyScore { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
