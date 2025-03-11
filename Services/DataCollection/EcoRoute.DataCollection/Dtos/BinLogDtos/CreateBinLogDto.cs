namespace EcoRoute.DataCollection.Dtos.BinLogDtos
{
    public class CreateBinLogDto
    {
        public Guid WasteBinId { get; set; }
        public double? FillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
