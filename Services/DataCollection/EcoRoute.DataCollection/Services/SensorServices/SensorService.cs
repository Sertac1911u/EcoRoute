using AutoMapper;
using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Dtos.SensorDtos;
using EcoRoute.DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Services.SensorServices
{
    public class SensorService : ISensorService
    {
        private readonly DataCollectionContext _context;
        private readonly IMapper _mapper;

        public SensorService(DataCollectionContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateSensorAsync(CreateSensorDto createSensorDto)
        {
            var sensor = _mapper.Map<Sensor>(createSensorDto);
            await _context.Sensors.AddAsync(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSensorAsync(Guid id)
        {
            var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == id);
            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultSensorDto>> GetAllSensorAsync()
        {
            var sensors = await _context.Sensors.ToListAsync(); 
            return _mapper.Map<List<ResultSensorDto>>(sensors);
        }

        public async Task<GetByIdSensorDto> GetByIdSensorAsync(Guid id)
        {
            var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == id);
            return _mapper.Map<GetByIdSensorDto>(sensor);
        }

        public async Task UpdateSensorAsync(UpdateSensorDto updateSensorDto)
        {
            var updateSensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == updateSensorDto.SensorId);
            _mapper.Map(updateSensorDto, updateSensor);
            _context.Sensors.Update(updateSensor);
            await _context.SaveChangesAsync();
        }
    }
}
