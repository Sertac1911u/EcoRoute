using EcoRoute.DataProcessing.Dtos.DataProcessingLogDtos;
using EcoRoute.DataProcessing.Services.DataProcessingLogServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProcessingLogsController : ControllerBase
    {
        private readonly IDataProcessingLogService _dataProcessingLogService;

        public DataProcessingLogsController(IDataProcessingLogService dataProcessingLogService)
        {
            _dataProcessingLogService = dataProcessingLogService;
        }

        [HttpGet]
        public async Task<IActionResult> DataProcessingLogList()
        {
            var values = await _dataProcessingLogService.GetAllDataProcessingLogAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDataProcessingLogById(Guid id)
        {
            var values = await _dataProcessingLogService.GetByIdDataProcessingLogAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDataProcessingLog(CreateDataProcessingLogDto dataProcessingLogDto)
        {
            await _dataProcessingLogService.CreateDataProcessingLogAsync(dataProcessingLogDto);
            return Ok("DataProcessingLog Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDataProcessingLog(Guid id)
        {
            await _dataProcessingLogService.DeleteDataProcessingLogAsync(id);
            return Ok("Data ProcessingLog Deleted");
        }
    }
}
