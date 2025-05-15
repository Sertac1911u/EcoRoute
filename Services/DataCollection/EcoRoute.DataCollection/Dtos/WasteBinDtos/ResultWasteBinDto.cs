using EcoRoute.DataCollection.Dtos.BinLogDtos;
using EcoRoute.DataCollection.Dtos.SensorDtos;

namespace EcoRoute.DataCollection.Dtos.WasteBinDtos
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
        public string DeviceStatus { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 

        public List<ResultSensorDto> Sensors { get; set; } = new();
        public List<ResultBinLogDto> BinLogs { get; set; } = new();
        public List<Guid> SensorIds { get; set; } = new();



    }
}
