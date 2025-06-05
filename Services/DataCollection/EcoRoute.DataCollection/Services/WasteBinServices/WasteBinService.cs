using AutoMapper;
using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Dtos.WasteBinDtos;
using EcoRoute.DataCollection.Entities;
using EcoRoute.DataCollection.Services.SensorServices;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Services.WasteBinServices
{
    public class WasteBinService : IWasteBinService
    {
        private readonly DataCollectionContext _context;
        private readonly IMapper _mapper;
        private readonly ISensorService _sensorService;
        private readonly IDataCollectionNotificationService _notificationService;
        public WasteBinService(DataCollectionContext context, IMapper mapper, ISensorService sensorService, IDataCollectionNotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _sensorService = sensorService;
            _notificationService = notificationService;
        }

        public async Task CreateWasteBinAsync(CreateWasteBinDto createWasteBinDto)
        {
            if (createWasteBinDto.SensorCount < 0 || createWasteBinDto.SensorCount > 20)
                throw new ArgumentException("Sensör adedi 0-20 arasında olmalıdır.");

            var wasteBin = _mapper.Map<WasteBin>(createWasteBinDto);
            wasteBin.WasteBinId = Guid.NewGuid();

            await _context.WasteBins.AddAsync(wasteBin);
            await _context.SaveChangesAsync();

            if (createWasteBinDto.SensorCount > 0)
            {
                await _sensorService.CreateSensorsForWasteBinAsync(wasteBin.WasteBinId, createWasteBinDto.SensorCount);
            }

            var resultDto = _mapper.Map<ResultWasteBinDto>(wasteBin);
            await _notificationService.SendWasteBinCreatedNotificationAsync(resultDto);
        }


        public async Task DeleteWasteBinAsync(Guid id)
        {
            var wasteBin = await _context.WasteBins.FirstOrDefaultAsync(w => w.WasteBinId == id);
            if (wasteBin != null)
            {
                await _sensorService.DeleteSensorsByWasteBinIdAsync(id);

                var resultDto = _mapper.Map<ResultWasteBinDto>(wasteBin);

                _context.WasteBins.Remove(wasteBin);
                await _context.SaveChangesAsync();

                await _notificationService.SendWasteBinDeletedNotificationAsync(resultDto);

            }
        }

        public async Task<List<ResultWasteBinDto>> GetAllWasteBinAsync()
        {
            var wasteBins = await _context.WasteBins
                .Include(w => w.Sensors.OrderBy(s => s.SensorNumber))
                .ToListAsync();
            return _mapper.Map<List<ResultWasteBinDto>>(wasteBins);
        }

        public async Task<GetByIdWasteBinDto> GetByIdWasteBinAsync(Guid id)
        {
            var wasteById = await _context.WasteBins
                .Include(w => w.Sensors.OrderBy(s => s.SensorNumber))
                .FirstOrDefaultAsync(w => w.WasteBinId == id);

            var result = _mapper.Map<GetByIdWasteBinDto>(wasteById);
            if (result != null)
            {
                result.Id = wasteById.WasteBinId; 
            }
            return result;
        }

        public async Task UpdateWasteBinAsync(UpdateWasteBinDto updateWasteBinDto)
        {
            if (updateWasteBinDto.SensorCount < 0 || updateWasteBinDto.SensorCount > 20)
                throw new ArgumentException("Sensör adedi 0-20 arasında olmalıdır.");

            var updateWasteBin = await _context.WasteBins
                .Include(w => w.Sensors)
                .FirstOrDefaultAsync(w => w.WasteBinId == updateWasteBinDto.WasteBinId);

            if (updateWasteBin != null)
            {
                var currentSensorCount = updateWasteBin.Sensors.Count;

                updateWasteBin.Label = updateWasteBinDto.Label;
                updateWasteBin.Address = updateWasteBinDto.Address;
                updateWasteBin.Latitude = updateWasteBinDto.Latitude;
                updateWasteBin.Longitude = updateWasteBinDto.Longitude;
                updateWasteBin.IsFilled = updateWasteBinDto.IsFilled;
                updateWasteBin.FillLevel = updateWasteBinDto.FillLevel;
                updateWasteBin.estimatedFillLevel = updateWasteBinDto.estimatedFillLevel;
                updateWasteBin.DeviceStatus = updateWasteBinDto.DeviceStatus;
                updateWasteBin.SensorCount = updateWasteBinDto.SensorCount;
                updateWasteBin.LastUpdated = updateWasteBinDto.LastUpdated;
                updateWasteBin.UpdatedAt = updateWasteBinDto.UpdatedAt;

                _context.WasteBins.Update(updateWasteBin);

                if (updateWasteBinDto.SensorCount != currentSensorCount)
                {
                    await _sensorService.UpdateSensorsForWasteBinAsync(updateWasteBinDto.WasteBinId, updateWasteBinDto.SensorCount);
                }

                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<ResultWasteBinDto>(updateWasteBin);
                await _notificationService.SendWasteBinUpdatedNotificationAsync(resultDto);

            }
        }

        public async Task<List<ResultWasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> ids)
        {
            var wasteBins = await _context.WasteBins
                .Include(w => w.Sensors.OrderBy(s => s.SensorNumber))
                .Where(w => ids.Contains(w.WasteBinId))
                .ToListAsync();
            return _mapper.Map<List<ResultWasteBinDto>>(wasteBins);
        }
    }
}