using EcoRoute.RouteOptimization.Dtos.WaypointDtos;
using EcoRoute.RouteOptimization.Services.WaypointServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.RouteOptimization.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WaypointsController : ControllerBase
    {
        private readonly IWaypointService _waypointService;

        public WaypointsController(IWaypointService waypointService)
        {
            _waypointService = waypointService;
        }
        [HttpGet]
        public async Task<IActionResult> WaypointList()
        {
            var ar = await _waypointService.GetAllWaypointAsync();
            return Ok(ar);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWaypointListById(Guid id)
        {
            var ae = await _waypointService.GetByIdWaypointAsync(id);
            return Ok(ae);
        }
        [HttpPost]
        public async Task<IActionResult> CreateWaypoint(CreateWaypointDto createWaypointDto)
        {
            await _waypointService.CreateWaypointAsync(createWaypointDto);
            return Ok("Waypoint Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteWaypoint(Guid id)
        {
            await _waypointService.DeleteWaypointAsync(id);
            return Ok("Waypoint Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateWaypoint(UpdateWaypointDto updateWaypointDto)
        {
            await _waypointService.UpdateWaypointAsync(updateWaypointDto);
            return Ok("Waypoint Updated");
        }
    }
}
