using EcoRoute.DataCollection.Dtos.ProcessDataDtos;
using EcoRoute.DataCollection.Services.ProcessDataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

namespace EcoRoute.DataCollection.Controllers
{
    [Authorize(Policy = "DataCollectionFullAccess")] // 🔥 Policy eklendi
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessDatasController : ControllerBase
    {
        private readonly IProcessDataService _processDataService;

        public ProcessDatasController(IProcessDataService processDataService)
        {
            _processDataService = processDataService;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessDataList()
        {
            var vales = await _processDataService.GetAllProcessDataAsync();
            return Ok(vales);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcessDataById(Guid id)
        {
            var values = await _processDataService.GetByIdProcessDataAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcessData(CreateProcessDataDto createProcessDataDto)
        {
            await _processDataService.CreateProcessDataAsync(createProcessDataDto);
            return Ok("Process Data Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProcessData(Guid id)
        {
            await _processDataService.DeleteProcessDataAsync(id);
            return Ok("Process Data Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProcessData(UpdateProcessDataDto updateProcessDataDto)
        {
            await _processDataService.UpdateProcessDataAsync(updateProcessDataDto);
            return Ok("Process Data Updated");
        }
    }
}
