namespace EcoRoute.DataCollection.Dtos.SensorDtos
{
    public class UpdateSensorDto
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } //sensör tipi
        public bool IsActive { get; set; }
        public DateTime? LastUpdate { get; set; } = DateTime.Now;

    }
}
