namespace EcoRoute.DataCollection.Entities
{
    public class ProcessData
    {
        public Guid ProcessDataId { get; set; }
        public Guid WasteBinId { get; set; }
        public double? AverageFillLevel { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual WasteBin WasteBin { get; set; }
    }
}
