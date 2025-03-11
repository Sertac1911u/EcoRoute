namespace EcoRoute.DataCollection.Dtos.BinLogDtos
{
    public class GetByIdBinLogDto
    {
        public Guid BinLogId { get; set; }
        public Guid WasteBinId { get; set; }
        public double? FillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
