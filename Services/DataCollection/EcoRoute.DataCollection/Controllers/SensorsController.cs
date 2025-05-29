using EcoRoute.DataCollection.Dtos.SensorDtos;
using EcoRoute.DataCollection.Services.SensorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensorsController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> SensorList()
        {
            var sensors = await _sensorService.GetAllSensorAsync();
            return Ok(sensors);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> GetSensorById(Guid id)
        {
            var sensor = await _sensorService.GetByIdSensorAsync(id);
            if (sensor == null)
                return NotFound();
            return Ok(sensor);
        }

        [HttpGet("wastebin/{wasteBinId}")]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> GetSensorsByWasteBinId(Guid wasteBinId)
        {
            var sensors = await _sensorService.GetSensorsByWasteBinIdAsync(wasteBinId);
            return Ok(sensors);
        }

        [HttpPut]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> UpdateSensor(UpdateSensorDto updateSensorDto)
        {
            await _sensorService.UpdateSensorAsync(updateSensorDto);
            return Ok("Sensor Updated Successfully");
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "DataCollectionFullAccess")]
        public async Task<IActionResult> UpdateSensorStatus(Guid id, [FromQuery] bool isActive, [FromQuery] bool isWorking)
        {
            await _sensorService.UpdateSensorStatusAsync(id, isActive, isWorking);
            return Ok("Sensor Status Updated Successfully");
        }
    }
}