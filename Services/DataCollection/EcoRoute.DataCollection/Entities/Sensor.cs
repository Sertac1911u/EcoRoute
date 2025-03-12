namespace EcoRoute.DataCollection.Entities
{
    public class Sensor
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } //sensör tipi
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastUpdate {  get; set; }

        
        public virtual WasteBin WasteBin { get; set; }
        public virtual ICollection<EnvLog> EnvLogs { get; set; }
    }
}
