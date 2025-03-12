using EcoRoute.RouteOptimization.Dtos.RouteOptimizationResultDtos;
using EcoRoute.RouteOptimization.Services.RouteOptimizationResultServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.RouteOptimization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteOptimizationResultsController : ControllerBase
    {
        private readonly IRouteOptimizationResultService _routeOptimizationResultService;

        public RouteOptimizationResultsController(IRouteOptimizationResultService routeOptimizationResultService)
        {
            _routeOptimizationResultService = routeOptimizationResultService;
        }

        [HttpGet]
        public async Task<IActionResult> RouteOptimizationResultList()
        {
            var s = await _routeOptimizationResultService.GetRouteOptimizationAsync();
            return Ok(s);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRouteOptimizationResultById(Guid id)
        {
            var s = await _routeOptimizationResultService.GetByIdRouteOptimizationResultAsync(id);
            return Ok(s);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRouteOptimizationResult(CreateRouteOptimizationResultDto dto)
        {
            await _routeOptimizationResultService.CreateRouteOptimizationResultAsync(dto);
            return Ok("Route Optimization Result Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRouteOptimizationResult(Guid id)
        {
            await _routeOptimizationResultService.DeleteRouteOptimizationResultAsync(id);
            return Ok("Route Optimization Result Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRouteOptimizationResult(UpdateRouteOptimizationResultDto dto)
        {
            await _routeOptimizationResultService.UpdateRouteOptimizationResultAsync(dto);
            return Ok("Route Optimization Result Updated");
        }
    }
}
