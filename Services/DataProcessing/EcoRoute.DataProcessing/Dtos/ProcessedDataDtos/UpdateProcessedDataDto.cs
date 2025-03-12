namespace EcoRoute.DataProcessing.Dtos.ProcessedDataDtos
{
    public class UpdateProcessedDataDto
    {
        public Guid ProcessedDataId { get; set; }
        public double AverageFillLevel { get; set; }
        public double PredictedFillLevel { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
