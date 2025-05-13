using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.RouteOptimization.Controllers
{
    [Authorize(Policy = "RouteOptimizationFullAccess")]
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RouteController(IRouteService routeService, IHttpContextAccessor httpContextAccessor)
        {
            _routeService = routeService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("my")] 
        public async Task<IActionResult> GetRoutes()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = user.FindFirst("sub")?.Value;
            var userRole = user.FindFirst("role")?.Value;

            var result = await _routeService.GetRoutesByRoleAsync(userRole, userId);
            return Ok(result);
        }

        [HttpGet("all")] 
        public async Task<IActionResult> GetAllRoutes([FromQuery] string? driverId)
        {
            var routes = await _routeService.GetAllRoutesAsync(driverId);
            return Ok(routes);
        }


        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteRoute(Guid id)
        {
            await _routeService.CompleteRouteAsync(id);
            return Ok("Rota başarıyla tamamlandı.");
        }

        [HttpGet("{id}/live")]
        public async Task<IActionResult> GetLiveStatus(Guid id)
        {
            var result = await _routeService.GetLiveStatusAsync(id);
            return Ok(result);
        }
        [HttpGet("HasActiveRouteForDriver/{driverId}")]
        public IActionResult DriverHasActiveRoute(string driverId)
        {
            var result = _routeService.DriverHasActiveRoute(driverId);
            return Ok(result);
        }
        [HttpGet("HasActiveRoute/{vehicleId}")]
        public IActionResult VehicleHasActiveRoute(Guid vehicleId)
        {
            var result = _routeService.VehicleHasActiveRoute(vehicleId.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] CreateRouteDto dto)
        {
            try
            {
                // Aktif rota kontrolü - Araç
                if (_routeService.VehicleHasActiveRoute(dto.VehicleId))
                {
                    return BadRequest(new { message = "Seçilen araç başka bir aktif rotada kullanılmaktadır." });
                }

                // Aktif rota kontrolü - Sürücü
                if (_routeService.DriverHasActiveRoute(dto.DriverId))
                {
                    return BadRequest(new { message = "Seçilen sürücü başka bir aktif rotaya atanmıştır." });
                }

                // Rota oluştur
                var result = await _routeService.CreateOptimizedRouteAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Detaylı hata mesajı
                return BadRequest(new { message = $"Rota oluşturulamadı: {ex.Message}" });
            }
        }

    }

}
