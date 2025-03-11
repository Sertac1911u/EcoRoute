namespace EcoRoute.DataCollection.Dtos.ProcessDataDtos
{
    public class ResultProcessDataDto
    {
        public Guid ProcessDataId { get; set; }
        public Guid WasteBinId { get; set; }
        public double? AverageFillLevel { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
