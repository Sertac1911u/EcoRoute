using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.ReportsDtos
{
    public class WasteBinStatsDto
    {
        public double AverageFill { get; set; }
        public double MaxFill { get; set; }
        public double MinFill { get; set; }
        public int TotalBins { get; set; }
    }
}
