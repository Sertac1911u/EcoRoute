using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Services
{
    public interface IRouteService
    {
        // Data retrieval
        Task<List<WasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> wasteBinIds);

        // Route operations
        Task<RouteResultDto> CreateOptimizedRouteAsync(CreateRouteDto dto);
        Task<List<RouteResultDto>> GetAllRoutesAsync(string? driverId = null);
        Task<RouteResultDto> GetRouteByIdAsync(Guid routeId);
        Task<List<RouteResultDto>> GetRoutesByRoleAsync(string role, string userId);
        Task<RouteLiveStatusDto> GetLiveStatusAsync(Guid routeId);

        // Route completion
        Task CompleteRouteAsync(Guid routeId);
        Task CompleteStepAsync(Guid stepId);

        // Route optimization
        Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId);

        // Status checks
        bool VehicleHasActiveRoute(string vehicleId);
        bool DriverHasActiveRoute(string driverId);

        // Statistics and reporting
        Task<CO2StatsDto> GetCO2StatsAsync(int days);
        Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync();

        // *** SIMÜLASYON METODLARI ***
        /// <summary>
        /// Rota simülasyonunu başlatır ve rotayı aktif duruma getirir
        /// </summary>
        /// <param name="routeId">Simüle edilecek rota ID'si</param>
        Task StartRouteSimulationAsync(Guid routeId);

        /// <summary>
        /// Simülasyonda bir sonraki adımı tamamlar
        /// </summary>
        /// <param name="routeId">Rota ID'si</param>
        /// <returns>Tamamlanan adım bilgileri ve rota durumu</returns>
        Task<SimulationStepResultDto> CompleteNextStepAsync(Guid routeId);

        /// <summary>
        /// Tüm aktif rotaları anında simüle eder (tüm adımları tamamlar)
        /// </summary>
        /// <returns>Simüle edilen rota sayısı</returns>
        Task<int> SimulateAllRoutesAsync();
    }
}
