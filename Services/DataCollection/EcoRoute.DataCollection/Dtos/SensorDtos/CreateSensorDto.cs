namespace EcoRoute.DataCollection.Dtos.SensorDtos
{
    public class CreateSensorDto
    {
        public Guid WasteBinId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsWorking { get; set; } = true;
        public int SensorNumber { get; set; }
        public DateTime InstallationDate { get; set; } = DateTime.Now;
    }
}
