using EcoRoute.DtoLayer.RouteOptimizationDtos;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public interface IRouteService
    {
        // CRUD Operations
        Task<bool> CreateRouteAsync(ApiCreateRouteDto dto);
        Task<List<RouteResultDto>> GetAllRoutesAsync();
        Task<RouteResultDto> GetRouteByIdAsync(Guid routeId);
        Task<List<RouteResultDto>> GetMyRoutesAsync();
        Task<bool> CompleteRouteAsync(Guid routeId);

        // Status Checks
        Task<bool> VehicleHasActiveRoute(Guid vehicleId);
        Task<bool> DriverHasActiveRoute(string driverId);

        // Route Optimization
        Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId);

        // Statistics and Reporting
        Task<CO2StatsDto> GetCO2StatsAsync(int days);

        // Step Management
        Task CompleteStepAsync(Guid stepId);

        // *** SIMÜLASYON METODLARI ***
        /// <summary>
        /// Rota simülasyonunu başlatır
        /// </summary>
        /// <param name="routeId">Simüle edilecek rota ID'si</param>
        Task StartRouteSimulationAsync(Guid routeId);

        /// <summary>
        /// Simülasyonda bir sonraki adımı tamamlar
        /// </summary>
        /// <param name="routeId">Rota ID'si</param>
        /// <returns>Tamamlanan adım bilgileri</returns>
        Task<SimulationStepResultDto> CompleteNextStepAsync(Guid routeId);

        /// <summary>
        /// Tüm aktif rotaları simüle eder (hızlı tamamlama)
        /// </summary>
        /// <returns>Simüle edilen rota sayısı</returns>
        Task<int> SimulateAllRoutesAsync();
    }
}
