namespace EcoRoute.DataCollection.Dtos.EnvLogDtos
{
    public class UpdateEnvLogDto
    {
        public Guid EnvLogId { get; set; }
        public Guid SensorId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public byte? Severity { get; set; } //1=Bilgi , 2=Uyarı vs
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
