namespace EcoRoute.DataCollection.Dtos.SensorDtos
{
    public class ResultSensorDto
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } //sensör tipi
        public bool IsActive { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
