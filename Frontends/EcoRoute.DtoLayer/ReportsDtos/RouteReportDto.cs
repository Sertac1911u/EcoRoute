using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.ReportsDtos
{
    public class RouteReportDto
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public int Status { get; set; }
        public DateTime StartTime { get; set; }
        public double TotalDistanceKm { get; set; }
        public double EstimatedCO2Kg { get; set; }
    }
}
