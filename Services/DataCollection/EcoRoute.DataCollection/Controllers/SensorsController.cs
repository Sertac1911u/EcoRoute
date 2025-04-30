using EcoRoute.DataCollection.Dtos.SensorDtos;
using EcoRoute.DataCollection.Services.SensorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataCollection.Controllers
{
    [Authorize(Policy = "DataCollectionFullAccess")] 
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
        public async Task<IActionResult> SensorList()
        {
            var sensors = await _sensorService.GetAllSensorAsync();
            return Ok(sensors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorById(Guid id)
        {
            var sensor = await _sensorService.GetByIdSensorAsync(id);
            if (sensor == null)
                return NotFound();
            return Ok(sensor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensor(CreateSensorDto createSensorDto)
        {
            await _sensorService.CreateSensorAsync(createSensorDto);
            return Ok("Sensor Created Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(Guid id)
        {
            await _sensorService.DeleteSensorAsync(id);
            return Ok("Sensor Deleted Successfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSensor(UpdateSensorDto updateSensorDto)
        {
            await _sensorService.UpdateSensorAsync(updateSensorDto);
            return Ok("Sensor Updated Successfully");
        }
    }
}
