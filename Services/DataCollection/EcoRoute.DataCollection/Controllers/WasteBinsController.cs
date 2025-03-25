using EcoRoute.DataCollection.Dtos.WasteBinDtos;
using EcoRoute.DataCollection.Services.WasteBinServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataCollection.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WasteBinsController : ControllerBase
    {
        private readonly IWasteBinService _wasteBinService;

        public WasteBinsController(IWasteBinService wasteBinService)
        {
            _wasteBinService = wasteBinService;
        }

        [HttpGet]
        public async Task<IActionResult> WasteBinList()
        {
            var values = await _wasteBinService.GetAllWasteBinAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWasteBinById(Guid id)
        {
            var values = await _wasteBinService.GetByIdWasteBinAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateWasteBin(CreateWasteBinDto createWasteBinDto)
        {
            await _wasteBinService.CreateWasteBinAsync(createWasteBinDto);
            return Ok("WasteBin Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteWasteBin(Guid id)
        {
            await _wasteBinService.DeleteWasteBinAsync(id);
            return Ok("WasteBin Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateWasteBin(UpdateWasteBinDto updateWasteBinDto)
        {
            await _wasteBinService.UpdateWasteBinAsync(updateWasteBinDto);
            return Ok("WasteBin Updated");
        }
    }
}
