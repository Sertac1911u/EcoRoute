namespace EcoRoute.DataCollection.Dtos.WasteBinDtos
{
    public class GetByIdWasteBinDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? IsFilled { get; set; }
        public double? FillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public int SensorCount { get; set; } // Sensör adedi değiştirilirse otomatik ayarlama yapılacak
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
