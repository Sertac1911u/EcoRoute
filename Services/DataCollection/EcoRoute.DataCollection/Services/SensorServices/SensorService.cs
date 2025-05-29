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

        public SensorService(DataCollectionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ResultSensorDto>> GetAllSensorAsync()
        {
            var sensors = await _context.Sensors.OrderBy(s => s.WasteBinId).ThenBy(s => s.SensorNumber).ToListAsync();
            return _mapper.Map<List<ResultSensorDto>>(sensors);
        }

        public async Task<List<ResultSensorDto>> GetSensorsByWasteBinIdAsync(Guid wasteBinId)
        {
            var sensors = await _context.Sensors
                .Where(s => s.WasteBinId == wasteBinId)
                .OrderBy(s => s.SensorNumber)
                .ToListAsync();
            return _mapper.Map<List<ResultSensorDto>>(sensors);
        }

        public async Task<GetByIdSensorDto> GetByIdSensorAsync(Guid id)
        {
            var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == id);
            return _mapper.Map<GetByIdSensorDto>(sensor);
        }

        public async Task UpdateSensorAsync(UpdateSensorDto updateSensorDto)
        {
            var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == updateSensorDto.SensorId);
            if (sensor != null)
            {
                sensor.IsActive = updateSensorDto.IsActive;
                sensor.IsWorking = updateSensorDto.IsWorking;
                sensor.LastUpdate = DateTime.Now;

                _context.Sensors.Update(sensor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSensorStatusAsync(Guid sensorId, bool isActive, bool isWorking)
        {
            var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == sensorId);
            if (sensor != null)
            {
                sensor.IsActive = isActive;
                sensor.IsWorking = isWorking;
                sensor.LastUpdate = DateTime.Now;

                _context.Sensors.Update(sensor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateSensorsForWasteBinAsync(Guid wasteBinId, int sensorCount)
        {
            if (sensorCount < 0 || sensorCount > 20)
                throw new ArgumentException("Sensör adedi 0-20 arasında olmalıdır.");

            var sensors = new List<Sensor>();
            for (int i = 1; i <= sensorCount; i++)
            {
                sensors.Add(new Sensor
                {
                    SensorId = Guid.NewGuid(),
                    WasteBinId = wasteBinId,
                    Type = "Light",
                    IsActive = true,
                    IsWorking = true,
                    SensorNumber = i,
                    InstallationDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                });
            }

            if (sensors.Any())
            {
                await _context.Sensors.AddRangeAsync(sensors);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSensorsForWasteBinAsync(Guid wasteBinId, int newSensorCount)
        {
            if (newSensorCount < 0 || newSensorCount > 20)
                throw new ArgumentException("Sensör adedi 0-20 arasında olmalıdır.");

            var existingSensors = await _context.Sensors
                .Where(s => s.WasteBinId == wasteBinId)
                .OrderBy(s => s.SensorNumber)
                .ToListAsync();

            var currentCount = existingSensors.Count;

            if (newSensorCount > currentCount)
            {
                // Yeni sensörler ekle
                var sensorsToAdd = new List<Sensor>();
                for (int i = currentCount + 1; i <= newSensorCount; i++)
                {
                    sensorsToAdd.Add(new Sensor
                    {
                        SensorId = Guid.NewGuid(),
                        WasteBinId = wasteBinId,
                        Type = "Light",
                        IsActive = true,
                        IsWorking = true,
                        SensorNumber = i,
                        InstallationDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
                }
                await _context.Sensors.AddRangeAsync(sensorsToAdd);
            }
            else if (newSensorCount < currentCount)
            {
                // Fazla sensörleri sil
                var sensorsToRemove = existingSensors.Skip(newSensorCount).ToList();
                _context.Sensors.RemoveRange(sensorsToRemove);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSensorsByWasteBinIdAsync(Guid wasteBinId)
        {
            var sensors = await _context.Sensors.Where(s => s.WasteBinId == wasteBinId).ToListAsync();
            _context.Sensors.RemoveRange(sensors);
            await _context.SaveChangesAsync();
        }
    }
}