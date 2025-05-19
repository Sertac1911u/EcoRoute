namespace EcoRoute.Reports.Dtos
{
    public class WasteBinStatsDto
    {
        public double AverageFill { get; set; }
        public double MaxFill { get; set; }
        public double MinFill { get; set; }
        public int TotalBins { get; set; }
    }
}
