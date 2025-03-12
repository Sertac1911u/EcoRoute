namespace EcoRoute.DataProcessing.Dtos.DataProcessingLogDtos
{
    public class ResultDataProcessingLogDto
    {
        public Guid DataProcessingLogId { get; set; }
        public string ProcessName { get; set; }
        public DateTime LogTime { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
    }
}
