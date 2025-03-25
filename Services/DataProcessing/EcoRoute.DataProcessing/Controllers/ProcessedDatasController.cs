using EcoRoute.DataProcessing.Dtos.ProcessedDataDtos;
using EcoRoute.DataProcessing.Services.ProcessedDataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace EcoRoute.DataProcessing.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessedDatasController : ControllerBase
    {
        private readonly IProcessedDataService _processedDataService;

        public ProcessedDatasController(IProcessedDataService processedDataService)
        {
            _processedDataService = processedDataService;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessedDataList()
        {
            var val = await _processedDataService.GetAllProcessedDataAsync();
            return Ok(val);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcessedDataById(Guid id)
        {
            var val = await _processedDataService.GetByIdProcessedDataAsync(id);
            return Ok(val);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcessedData(CreateProcessedDataDto createProcessedDataDto)
        {
            await _processedDataService.CreateProcessedDataAsync(createProcessedDataDto);
            return Ok("ProcessedData Created");
        }
        [HttpDelete]
        public  async Task<IActionResult> DeleteProcessedData(Guid id)
        {
            await _processedDataService.DeleteProcessedDataAsync(id);
            return Ok("ProcessedData Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProcessedData(UpdateProcessedDataDto updateProcessedDataDto)
        {
            await _processedDataService.UpdateProcessedDataAsync(updateProcessedDataDto);
            return Ok("ProcessedData Updated");
        }
    }
}
