using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Services
{
    public interface IRouteService
    {
        Task<List<WasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> wasteBinIds);

        Task<RouteResultDto> CreateOptimizedRouteAsync(CreateRouteDto dto);
        Task<List<RouteResultDto>> GetAllRoutesAsync(string? driverId = null);
        Task<RouteResultDto> GetRouteByIdAsync(Guid routeId);
        Task<List<RouteResultDto>> GetRoutesByRoleAsync(string role, string userId);
        Task<RouteLiveStatusDto> GetLiveStatusAsync(Guid routeId);

        Task CompleteRouteAsync(Guid routeId);
        Task CompleteStepAsync(Guid stepId);

        Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId);

        bool VehicleHasActiveRoute(string vehicleId);
        bool DriverHasActiveRoute(string driverId);

        Task<CO2StatsDto> GetCO2StatsAsync(int days);
        Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync();

       
        Task StartRouteSimulationAsync(Guid routeId);

       
        Task<SimulationStepResultDto> CompleteNextStepAsync(Guid routeId);

      
        Task<int> SimulateAllRoutesAsync();
    }
}
