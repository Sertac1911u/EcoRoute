using EcoRoute.RouteOptimization.Dtos.RouteDtos;

namespace EcoRoute.RouteOptimization.Services.MyRouteServices
{
    public interface IMyRouteService
    {
        Task<List<ResultRouteDto>> GetAllRoutesAsync();
        Task CreateRouteAsync(CreateRouteDto createRouteDto);
        Task UpdateRouteAsync(UpdateRouteDto updateRouteDto);
        Task DeleteRouteAsync(Guid id);
        Task<GetByIdRouteDto> GetByIdRouteAsync(Guid id);
    }
}
