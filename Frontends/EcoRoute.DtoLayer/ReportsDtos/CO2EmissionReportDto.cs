using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.ReportsDtos
{
    public class CO2StatsResponseDto
    {
        public List<CO2EmissionReportDto> DailyStats { get; set; } = new();
    }

    public class CO2EmissionReportDto
    {
        public DateTime Date { get; set; }
        public double CO2Kg { get; set; }
        public int RouteCount { get; set; }
    }
}
