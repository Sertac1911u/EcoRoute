namespace EcoRoute.DataCollection.Entities
{
    public class WasteBin
    {
        public Guid WasteBinId { get; set; }
        public string Label { get; set; }
        public double? Location { get; set; }
        public bool? IsFilled { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string DeviceStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
        public virtual ICollection<BinLog> BinLogs { get; set; }
        public virtual ICollection<ProcessData> ProcessData { get; set; }
    }
}
