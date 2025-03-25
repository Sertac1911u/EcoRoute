using EcoRoute.DataCollection.Dtos.SensorDtos;
using EcoRoute.DataCollection.Services.SensorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.DataCollection.Controllers
{
    [Authorize]
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
            var values = await _sensorService.GetAllSensorAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorById(Guid id)
        {
            var values = await _sensorService.GetByIdSensorAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSensor(CreateSensorDto createSensorDto)
        {
            await _sensorService.CreateSensorAsync(createSensorDto);
            return Ok("Sensor Created");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSensor(Guid id)
        {
            await _sensorService.DeleteSensorAsync(id);
            return Ok("Sensor Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSensor(UpdateSensorDto updateSensorDto)
        {
            await _sensorService.UpdateSensorAsync(updateSensorDto);
            return Ok("Sensor Updated");
        }
    }
}
