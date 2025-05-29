using EcoRoute.DataCollection.Dtos.SensorDtos;

namespace EcoRoute.DataCollection.Dtos.WasteBinDtos
{
    public class ResultWasteBinDto
    {
        public Guid Id { get; set; } // WasteBinId yerine Id
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? IsFilled { get; set; }
        public double? FillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public int SensorCount { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Sensör listesi
        public List<ResultSensorDto> Sensors { get; set; } = new();

        // Aktif sensör sayısı hesaplama için
        public int ActiveSensorCount => Sensors?.Count(s => s.IsActive) ?? 0;

        // Çalışan sensör sayısı hesaplama için  
        public int WorkingSensorCount => Sensors?.Count(s => s.IsActive && s.IsWorking) ?? 0;

    }
}
