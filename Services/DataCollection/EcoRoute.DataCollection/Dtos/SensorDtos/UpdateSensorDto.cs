namespace EcoRoute.DataCollection.Dtos.SensorDtos
{
    public class UpdateSensorDto
    {
        public Guid SensorId { get; set; }
        public bool IsActive { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? LastUpdate { get; set; } = DateTime.Now;

    }
}
