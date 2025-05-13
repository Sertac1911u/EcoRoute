using EcoRoute.DtoLayer.RouteOptimizationDtos;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public interface IRouteService
    {
        Task<List<RouteResultDto>> GetAllRoutesAsync();
        Task<List<RouteResultDto>> GetMyRoutesAsync();
        Task<bool> CompleteRouteAsync(Guid routeId);
        Task<bool> CreateRouteAsync(CreateRouteDto dto);
        Task<bool> VehicleHasActiveRoute(Guid vehicleId);
        Task<bool> DriverHasActiveRoute(string driverId);
    }
}
