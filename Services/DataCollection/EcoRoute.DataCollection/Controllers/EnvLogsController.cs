using EcoRoute.DataCollection.Dtos.EnvLogDtos;
using EcoRoute.DataCollection.Services.EnvLogServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataCollection.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnvLogsController : ControllerBase
    {
        private readonly IEnvLogService _envLogService;

        public EnvLogsController(IEnvLogService envLogService)
        {
            _envLogService = envLogService;
        }
        [HttpGet]
        public async Task<IActionResult> GetEnvLogList()
        {
            var values = await _envLogService.GetAllEnvLogAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnvLogById(Guid id)
        {
            var values = await _envLogService.GetByIdEnvLogAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEnvLog(CreateEnvLogDto createEnvLogDto)
        {
            await _envLogService.CreateEnvLogAsync(createEnvLogDto);
            return Ok("EnvLog Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteEnvLog(Guid id)
        {
            await _envLogService.DeleteEnvLogAsync(id);
            return Ok("EnvLog Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateEnvLog(UpdateEnvLogDto updateEnvLogDto)
        {
            await _envLogService.UpdateEnvLogAsync(updateEnvLogDto);
            return Ok("EnvLog Updated");
        }
    }
}
