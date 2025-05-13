using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Services
{
    public interface IRouteService
    {
        Task<RouteResultDto> CreateOptimizedRouteAsync(CreateRouteDto dto);
        Task<List<WasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> wasteBinIds);
        Task<List<RouteResultDto>> GetAllRoutesAsync(string? driverId = null);
        Task CompleteRouteAsync(Guid routeId);
        Task<RouteLiveStatusDto> GetLiveStatusAsync(Guid routeId);
        Task<List<RouteResultDto>> GetRoutesByRoleAsync(string role, string userId);
        bool DriverHasActiveRoute(string driverId);
        bool VehicleHasActiveRoute(string vehicleId);


    }
}
