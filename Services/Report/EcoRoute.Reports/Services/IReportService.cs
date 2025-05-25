using EcoRoute.Reports.Dtos;

namespace EcoRoute.Reports.Services
{
    public interface IReportService
    {
        Task<List<WasteBinReportDto>> GetWasteBinReportAsync();
        Task<List<SensorReportDto>> GetSensorReportAsync();
        Task<List<RouteReportDto>> GetRouteReportAsync(); 
        Task<List<CO2EmissionReportDto>> GetCO2EmissionReportAsync(int days = 30);
        Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync(); 
        Task<List<UserActivityReportDto>> GetUserActivityReportAsync();
        Task<WasteBinStatsDto> GetWasteBinStatsAsync();

    }
}
