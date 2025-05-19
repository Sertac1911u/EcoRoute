namespace EcoRoute.Reports.Dtos
{
    public class SensorReportDto
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } // Aktif/Pasif olarak dönüştürülmüş metin
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
