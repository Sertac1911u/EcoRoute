namespace EcoRoute.DataCollection.Entities
{
    public class Sensor
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; } = "Light"; // Sabit değer - sadece ışık sensörü
        public bool IsActive { get; set; } = true;
        public bool IsWorking { get; set; } = true; // Çalışır durumda olup olmadığı
        public int SensorNumber { get; set; } // 1'den başlayarak sensör numarası
        public DateTime InstallationDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdate { get; set; }

        public virtual WasteBin WasteBin { get; set; }
    }
}
