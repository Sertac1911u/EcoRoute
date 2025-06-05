using EcoRoute.DtoLayer.RouteOptimizationDtos;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public interface IRouteService
    {
        Task<bool> CreateRouteAsync(ApiCreateRouteDto dto);
        Task<List<RouteResultDto>> GetAllRoutesAsync();
        Task<RouteResultDto> GetRouteByIdAsync(Guid routeId);
        Task<List<RouteResultDto>> GetMyRoutesAsync();
        Task<bool> CompleteRouteAsync(Guid routeId);

        Task<bool> VehicleHasActiveRoute(Guid vehicleId);
        Task<bool> DriverHasActiveRoute(string driverId);

        Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId);

        Task<CO2StatsDto> GetCO2StatsAsync(int days);

        Task CompleteStepAsync(Guid stepId);

        Task StartRouteSimulationAsync(Guid routeId);

       
        Task<SimulationStepResultDto> CompleteNextStepAsync(Guid routeId);

      
        Task<int> SimulateAllRoutesAsync();
    }
}
