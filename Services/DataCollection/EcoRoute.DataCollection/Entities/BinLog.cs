namespace EcoRoute.DataCollection.Entities
{
    public class BinLog
    {
        public Guid BinLogId { get; set; }
        public Guid WasteBinId { get; set; }
        public double? FillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public DateTime CreatedAt { get; set; }


        public virtual WasteBin WasteBin { get; set; }
    }
}
