using EcoRoute.RouteOptimization.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.RouteOptimization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WasteBinDebugController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public WasteBinDebugController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestWasteBinFetch([FromQuery] List<Guid> id)
        {
            try
            {
                var bins = await _routeService.GetWasteBinsByIdsAsync(id);
                return Ok(bins);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
