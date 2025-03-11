namespace EcoRoute.DataCollection.Dtos.WasteBinDtos
{
    public class ResultWasteBinDto
    {
        public Guid WasteBinId { get; set; }
        public string Label { get; set; }
        public double? Location { get; set; }
        public bool? IsFilled { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string DeviceStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
