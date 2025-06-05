namespace EcoRoute.DataCollection.Entities
{
    public class Sensor
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } = "Light"; 
        public bool IsActive { get; set; } = true;
        public bool IsWorking { get; set; } = true; 
        public int SensorNumber { get; set; } 
        public DateTime InstallationDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdate { get; set; }

        public virtual WasteBin WasteBin { get; set; }
    }
}
