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

        [HttpPost]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> CreateWasteBin(CreateWasteBinDto createWasteBinDto)
        {
            try
            {
                await _wasteBinService.CreateWasteBinAsync(createWasteBinDto);
                return Ok("WasteBin Created with Sensors");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> DeleteWasteBin(Guid id)
        {
            await _wasteBinService.DeleteWasteBinAsync(id);
            return Ok("WasteBin and Sensors Deleted");
        }

        [HttpPut]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> UpdateWasteBin(UpdateWasteBinDto updateWasteBinDto)
        {
            try
            {
                await _wasteBinService.UpdateWasteBinAsync(updateWasteBinDto);
                return Ok("WasteBin Updated");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Service-to-service endpoint - geçici olarak anonymous
        [HttpGet("selected")]
        public async Task<IActionResult> GetWasteBinsByIds([FromQuery] List<Guid> id)
        {
            try
            {
                if (id == null || !id.Any())
                {
                    return BadRequest("En az bir ID belirtilmelidir.");
                }

                Console.WriteLine($"[DataCollection] GetWasteBinsByIds called with {id.Count} IDs");
                foreach (var guid in id)
                {
                    Console.WriteLine($"[DataCollection] Requested ID: {guid}");
                }

                var bins = await _wasteBinService.GetWasteBinsByIdsAsync(id);

                Console.WriteLine($"[DataCollection] Found {bins.Count} bins");

                return Ok(bins);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DataCollection] Error: {ex.Message}");
                Console.WriteLine($"[DataCollection] StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}