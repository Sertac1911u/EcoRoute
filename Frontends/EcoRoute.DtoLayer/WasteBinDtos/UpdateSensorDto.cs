using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.WasteBinDtos
{
    public class UpdateSensorDto
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } //sensör tipi
        public bool IsActive { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
