using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.WasteBinDtos
{
    public class ResultWasteBinDto
    {
        public Guid Id { get; set; } 
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? IsFilled { get; set; }
        public double? FillLevel { get; set; }
        public double? estimatedFillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public int SensorCount { get; set; } 
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ResultSensorDto> Sensors { get; set; } = new();

        public int ActiveSensorCount => Sensors?.Count(s => s.IsActive) ?? 0;

        public int WorkingSensorCount => Sensors?.Count(s => s.IsActive && s.IsWorking) ?? 0;

    }
}
