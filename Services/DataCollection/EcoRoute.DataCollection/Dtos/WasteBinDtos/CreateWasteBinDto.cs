namespace EcoRoute.DataCollection.Dtos.WasteBinDtos
{
    public class CreateWasteBinDto
    {
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? IsFilled { get; set; }
        public double? FillLevel { get; set; }
        public double? estimatedFillLevel { get; set; }

        public string DeviceStatus { get; set; }
        public int SensorCount { get; set; } = 1; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
