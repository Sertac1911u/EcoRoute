namespace EcoRoute.DataProcessing.Entities
{
    public class DataProcessingLog
    {
        public Guid DataProcessingLogId { get; set; }
        public string ProcessName { get; set; }
        public DateTime LogTime { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
    }
}
