using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class ResultVehicleDto
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public string Description { get; set; }
    }
}
