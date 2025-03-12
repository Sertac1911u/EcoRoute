using EcoRoute.RouteOptimization.Dtos.WaypointDtos;

namespace EcoRoute.RouteOptimization.Services.WaypointServices
{
    public interface IWaypointService
    {
        Task<List<ResultWaypointDto>> GetAllWaypointAsync();
        Task CreateWaypointAsync(CreateWaypointDto createWaypointDto);
        Task UpdateWaypointAsync(UpdateWaypointDto updateWaypointDto);
        Task DeleteWaypointAsync(Guid id);
        Task<GetByIdWaypointDto> GetByIdWaypointAsync(Guid id);
    }
}
