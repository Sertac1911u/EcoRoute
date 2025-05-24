using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoRoute.RouteOptimization.Controllers
{
    // *** DÜZELTME: Route template değiştirildi ***
    [Route("api/[controller]")]  // "api/routeoptimization/[controller]" yerine "api/[controller]"
    [ApiController]
    [Authorize]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteService routeService, ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult<RouteResultDto>> CreateRoute([FromBody] CreateRouteDto dto)
        {
            try
            {
                _logger.LogInformation("Creating new route for driver {DriverId}", dto.DriverId);

                var result = await _routeService.CreateOptimizedRouteAsync(dto);

                _logger.LogInformation("Route created successfully with ID {RouteId}", result.Id);
                return CreatedAtAction(nameof(GetRoute), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating route");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult<List<RouteResultDto>>> GetAllRoutes()
        {
            try
            {
                _logger.LogInformation("Getting all routes");
                var routes = await _routeService.GetAllRoutesAsync();
                _logger.LogInformation("Retrieved {Count} routes", routes.Count);
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all routes");
                return StatusCode(500, new { message = "Rotalar alınırken bir hata oluştu" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RouteResultDto>> GetRoute(Guid id)
        {
            try
            {
                var route = await _routeService.GetRouteByIdAsync(id);
                return Ok(route);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving route {RouteId}", id);
                return NotFound(new { message = "Rota bulunamadı" });
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "Driver")]
        public async Task<ActionResult<List<RouteResultDto>>> GetMyRoutes()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var routes = await _routeService.GetAllRoutesAsync(userId);
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user routes");
                return StatusCode(500, new { message = "Rotalar alınırken bir hata oluştu" });
            }
        }

        [HttpGet("{id}/live-status")]
        public async Task<ActionResult<RouteLiveStatusDto>> GetLiveStatus(Guid id)
        {
            try
            {
                var status = await _routeService.GetLiveStatusAsync(id);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting live status for route {RouteId}", id);
                return NotFound(new { message = "Rota bulunamadı" });
            }
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "SuperAdmin,Manager,Driver")]
        public async Task<IActionResult> CompleteRoute(Guid id)
        {
            try
            {
                _logger.LogInformation("Completing route {RouteId}", id);

                await _routeService.CompleteRouteAsync(id);

                _logger.LogInformation("Route {RouteId} completed successfully", id);
                return Ok(new { message = "Rota başarıyla tamamlandı" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing route {RouteId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("steps/{stepId}/complete")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> CompleteStep(Guid stepId)
        {
            try
            {
                _logger.LogInformation("Completing step {StepId}", stepId);

                await _routeService.CompleteStepAsync(stepId);

                _logger.LogInformation("Step {StepId} completed successfully", stepId);
                return Ok(new { message = "Adım başarıyla tamamlandı" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing step {StepId}", stepId);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/reoptimize")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult<RouteResultDto>> ReoptimizeRoute(Guid id)
        {
            try
            {
                _logger.LogInformation("Reoptimizing route {RouteId}", id);

                var result = await _routeService.ReoptimizeRouteWithTrafficAsync(id);

                _logger.LogInformation("Route {RouteId} reoptimized successfully", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reoptimizing route {RouteId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("HasActiveRoute/{vehicleId}")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public ActionResult<bool> VehicleHasActiveRoute(string vehicleId)
        {
            try
            {
                var hasActiveRoute = _routeService.VehicleHasActiveRoute(vehicleId);
                return Ok(hasActiveRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking vehicle active route status");
                return StatusCode(500, false);
            }
        }

        [HttpGet("HasActiveRouteForDriver/{driverId}")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public ActionResult<bool> DriverHasActiveRoute(string driverId)
        {
            try
            {
                var hasActiveRoute = _routeService.DriverHasActiveRoute(driverId);
                return Ok(hasActiveRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking driver active route status");
                return StatusCode(500, false);
            }
        }

        [HttpGet("co2-stats")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult<CO2StatsDto>> GetCO2Stats([FromQuery] int days = 30)
        {
            try
            {
                var stats = await _routeService.GetCO2StatsAsync(days);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CO2 stats");
                return StatusCode(500, new { message = "CO2 istatistikleri alınırken bir hata oluştu" });
            }
        }

        [HttpGet("performance-report")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult<List<RoutePerformanceReportDto>>> GetPerformanceReport()
        {
            try
            {
                var report = await _routeService.GetRoutePerformanceReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting performance report");
                return StatusCode(500, new { message = "Performans raporu alınırken bir hata oluştu" });
            }
        }

        // *** SIMÜLASYON ENDPOINT'LERİ ***

        [HttpPost("{id}/simulate")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<IActionResult> StartRouteSimulation(Guid id)
        {
            try
            {
                _logger.LogInformation("Starting simulation for route {RouteId}", id);

                await _routeService.StartRouteSimulationAsync(id);

                _logger.LogInformation("Simulation started successfully for route {RouteId}", id);
                return Ok(new { message = "Rota simülasyonu başlatıldı", routeId = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting simulation for route {RouteId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/complete-next-step")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult<SimulationStepResultDto>> CompleteNextStep(Guid id)
        {
            try
            {
                _logger.LogInformation("Completing next step for route {RouteId}", id);

                var result = await _routeService.CompleteNextStepAsync(id);

                _logger.LogInformation("Next step completed for route {RouteId}. Progress: {Progress}%",
                    id, result.ProgressPercentage);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing next step for route {RouteId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("simulate-all-routes")]
        [Authorize(Roles = "SuperAdmin,Manager")]
        public async Task<ActionResult> SimulateAllRoutes()
        {
            try
            {
                _logger.LogInformation("Starting simulation for all active routes");

                var simulatedCount = await _routeService.SimulateAllRoutesAsync();

                _logger.LogInformation("Successfully simulated {Count} routes", simulatedCount);
                return Ok(new
                {
                    message = $"{simulatedCount} rota başarıyla simüle edildi",
                    simulatedCount = simulatedCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error simulating all routes");
                return StatusCode(500, new { message = "Rotalar simüle edilirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}