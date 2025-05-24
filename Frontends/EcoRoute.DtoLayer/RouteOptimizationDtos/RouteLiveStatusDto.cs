using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class RouteLiveStatusDto
    {
        public int TotalSteps { get; set; }
        public int CompletedSteps { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentAddress { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int EstimatedTimeRemainingMin { get; set; }
        public int ElapsedTimeMin { get; set; }
    }
}
