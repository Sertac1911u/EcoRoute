using EcoRoute.DtoLayer.RouteOptimizationDtos;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public interface IRouteService
    {
        Task<List<RouteResultDto>> GetAllRoutesAsync();
        Task<List<RouteResultDto>> GetMyRoutesAsync();
        Task<bool> CompleteRouteAsync(Guid routeId);
        Task<bool> CreateRouteAsync(ApiCreateRouteDto dto);
        Task<bool> VehicleHasActiveRoute(Guid vehicleId);
        Task<bool> DriverHasActiveRoute(string driverId);
        Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId);
        Task<CO2StatsDto> GetCO2StatsAsync(int days);
        Task<RouteResultDto> GetRouteByIdAsync(Guid routeId);
    }
}
