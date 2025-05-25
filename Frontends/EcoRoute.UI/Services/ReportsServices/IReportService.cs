using EcoRoute.DtoLayer.ReportsDtos;

namespace EcoRoute.UI.Services.ReportsServices
{
    public interface IReportService
    {
        Task<List<WasteBinReportDto>> GetWasteBinReportAsync();
        Task<List<SensorReportDto>> GetSensorReportAsync();
        Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync();
        Task<List<RouteReportDto>> GetRouteReportAsync();
        Task<List<UserActivityReportDto>> GetUserActivityReportAsync();
        Task<List<CO2EmissionReportDto>> GetCO2EmissionReportAsync(int days = 30);

        // Yardımcı metodlar
        Task<byte[]> ExportReportAsync(string reportType, string format);
        Task<WasteBinStatsDto> GetWasteBinStatsAsync();

    }
}
