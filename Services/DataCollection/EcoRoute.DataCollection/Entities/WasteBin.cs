namespace EcoRoute.DataCollection.Entities
{
    public class WasteBin
    {
        public Guid WasteBinId { get; set; }
        public string Label { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? IsFilled { get; set; }
        public double? FillLevel { get; set; }
        public double? estimatedFillLevel { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string DeviceStatus { get; set; }
        public int SensorCount { get; set; } = 1; 
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    }
}
