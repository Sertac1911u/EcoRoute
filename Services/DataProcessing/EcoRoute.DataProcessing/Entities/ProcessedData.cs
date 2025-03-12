namespace EcoRoute.DataProcessing.Entities
{
    public class ProcessedData
    {
        public Guid ProcessedDataId { get; set; }
        public Guid BinId { get; set; }
        public double AverageFillLevel { get; set; }
        public double PredictedFillLevel { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
