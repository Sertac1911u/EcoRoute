namespace EcoRoute.DataCollection.Dtos.SensorDtos
{
    public class CreateSensorDto
    {
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } //sensör tipi
        public bool IsActive { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
