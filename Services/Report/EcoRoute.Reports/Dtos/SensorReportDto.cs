namespace EcoRoute.Reports.Dtos
{
    public class SensorReportDto
    {
        public Guid SensorId { get; set; }
        public Guid WasteBinId { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }

        public DateTime? InstallationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
