using EcoRoute.DataCollection.Dtos.WasteBinDtos;
using EcoRoute.DataCollection.Services.WasteBinServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WasteBinsController : ControllerBase
    {
        private readonly IWasteBinService _wasteBinService;

        public WasteBinsController(IWasteBinService wasteBinService)
        {
            _wasteBinService = wasteBinService;
        }

        // SADECE GET İŞLEMİNE SCOPE VE ROLE OKUMA İZNİ
        [HttpGet]
        [Authorize(Policy = "DataCollectionReadAccess")]
        public async Task<IActionResult> WasteBinList()
        {
            var values = await _wasteBinService.GetAllWasteBinAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "DataCollectionReadAccess")]
        public async Task<IActionResult> GetWasteBinById(Guid id)
        {
            var values = await _wasteBinService.GetByIdWasteBinAsync(id);
            return Ok(values);
        }

        // CREATE - GÜNCELLEME - SİLME -> SADECE TAM YETKİLİLER
        [HttpPost]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> CreateWasteBin(CreateWasteBinDto createWasteBinDto)
        {
            await _wasteBinService.CreateWasteBinAsync(createWasteBinDto);
            return Ok("WasteBin Created");
        }

        [HttpDelete]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> DeleteWasteBin(Guid id)
        {
            await _wasteBinService.DeleteWasteBinAsync(id);
            return Ok("WasteBin Deleted");
        }

        [HttpPut]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> UpdateWasteBin(UpdateWasteBinDto updateWasteBinDto)
        {
            await _wasteBinService.UpdateWasteBinAsync(updateWasteBinDto);
            return Ok("WasteBin Updated");
        }
        [HttpGet("selected")]
        //[Authorize(Policy = "DataCollectionReadAccess")]
        public async Task<IActionResult> GetWasteBinsByIds([FromQuery] List<Guid> id)
        {
            var bins = await _wasteBinService.GetWasteBinsByIdsAsync(id);
            return Ok(bins);
        }

    }
}
