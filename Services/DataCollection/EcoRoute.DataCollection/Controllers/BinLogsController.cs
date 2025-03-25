using EcoRoute.DataCollection.Dtos.BinLogDtos;
using EcoRoute.DataCollection.Services.BinLogServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace EcoRoute.DataCollection.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BinLogsController : ControllerBase
    {
        private readonly IBinLogService _binLogService;

        public BinLogsController(IBinLogService binLogService)
        {
            _binLogService = binLogService;
        }

        [HttpGet]
        public async Task<IActionResult> BinLogList()
        {
            var values = await _binLogService.GetAllBinLogAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBinLogById(Guid id)
        {
            var values = await _binLogService.GetByIdBinLogAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBinLog(CreateBinLogDto createBinLogDto)
        {
            await _binLogService.CreateBinLogAsync(createBinLogDto);
            return Ok("BinLog Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBinLog(Guid id)
        {
            await _binLogService.DeleteBinLogAsync(id);
            return Ok("BinLog Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBinLog(UpdateBinLogDto updateBinLogDto)
        {
            await _binLogService.UpdateBinLogAsync(updateBinLogDto);
            return Ok("BinLog Updated");
        }
    }
}
