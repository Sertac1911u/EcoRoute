namespace EcoRoute.DataCollection.Dtos.SensorDtos
{
    public class GetByIdSensorDto
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } = "Light";
        public bool IsActive { get; set; }
        public bool IsWorking { get; set; }
        public int SensorNumber { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
