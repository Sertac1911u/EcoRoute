using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.WasteBinDtos
{
    public class CreateWasteBinDto
    {
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? IsFilled { get; set; }
        public double? FillLevel { get; set; }  
        public string DeviceStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Guid> SensorIds { get; set; } = new(); 
    }
}
