using EcoRoute.RouteOptimization.Dtos.RouteOptimizationResultDtos;

namespace EcoRoute.RouteOptimization.Services.RouteOptimizationResultServices
{
    public interface IRouteOptimizationResultService
    {
        Task<List<ResultRouteOptimizationResultDto>> GetRouteOptimizationAsync();
        Task CreateRouteOptimizationResultAsync(CreateRouteOptimizationResultDto routeOptimizationResultDto);
        Task UpdateRouteOptimizationResultAsync(UpdateRouteOptimizationResultDto updateRouteOptimizationResultDto);
        Task DeleteRouteOptimizationResultAsync(Guid id);
        Task<GetByIdRouteOptimizationResultDto> GetByIdRouteOptimizationResultAsync(Guid id);
    }
}
