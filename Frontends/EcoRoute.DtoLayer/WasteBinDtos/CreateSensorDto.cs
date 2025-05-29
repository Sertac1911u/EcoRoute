using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.WasteBinDtos
{
    public class CreateSensorDto
    {
        public Guid WasteBinId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsWorking { get; set; } = true;
        public int SensorNumber { get; set; }
        public DateTime InstallationDate { get; set; } = DateTime.Now;

    }
}
