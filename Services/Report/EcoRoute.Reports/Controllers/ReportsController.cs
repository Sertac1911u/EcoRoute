using EcoRoute.Reports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcoRoute.Reports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("wastebins")]
        public async Task<IActionResult> GetWasteBinReport()
        {
            var result = await _reportService.GetWasteBinReportAsync();
            return Ok(result);
        }

        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("sensors")]
        public async Task<IActionResult> GetSensorReport()
        {
            var result = await _reportService.GetSensorReportAsync();
            return Ok(result);
        }

        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("routes/performance")]
        public async Task<IActionResult> GetRoutePerformanceReport()
        {
            var result = await _reportService.GetRoutePerformanceReportAsync();
            return Ok(result);
        }
        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("users/activity")]
        public async Task<IActionResult> GetUserActivityReport()
        {
            var result = await _reportService.GetUserActivityReportAsync();
            return Ok(result);
        }
        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("routes")]
        public async Task<IActionResult> GetRouteReport()
        {
            var result = await _reportService.GetRouteReportAsync();
            return Ok(result);
        }

        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("routes/co2-stats")]
        public async Task<IActionResult> GetCO2EmissionReport()
        {
            var result = await _reportService.GetCO2EmissionReportAsync();
            return Ok(result);
        }

        [Authorize(Policy = "ReportsAccess")]
        [HttpGet("wastebins/stats")]
        public async Task<IActionResult> GetWasteBinStats()
        {
            var result = await _reportService.GetWasteBinStatsAsync();
            return Ok(result);
        }

    }
}
