using EcoRoute.RouteOptimization.Dtos.RouteDtos;
using EcoRoute.RouteOptimization.Services.MyRouteServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.RouteOptimization.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MyRoutesController : ControllerBase
    {
        private readonly IMyRouteService _myRouteService;

        public MyRoutesController(IMyRouteService myRouteService)
        {
            _myRouteService = myRouteService;
        }

        [HttpGet]
        public async Task<IActionResult> RouteList()
        {
            var values = await _myRouteService.GetAllRoutesAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRouteListById(Guid id)
        {
            var values = await _myRouteService.GetByIdRouteAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoute(CreateRouteDto createRouteDto)
        {
            await _myRouteService.CreateRouteAsync(createRouteDto);
            return Ok("Route Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRoute(Guid id)
        {
            await _myRouteService.DeleteRouteAsync(id);
            return Ok("Route Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRoute(UpdateRouteDto updateRouteDto)
        {
            await _myRouteService.UpdateRouteAsync(updateRouteDto);
            return Ok("Route Updated");
        }
    }
}
