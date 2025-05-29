using EcoRoute.DataCollection.Dtos.SensorDtos;

namespace EcoRoute.DataCollection.Services.SensorServices
{
    public interface ISensorService
    {
        Task<List<ResultSensorDto>> GetAllSensorAsync();
        Task<List<ResultSensorDto>> GetSensorsByWasteBinIdAsync(Guid wasteBinId);
        Task<GetByIdSensorDto> GetByIdSensorAsync(Guid id);
        Task UpdateSensorAsync(UpdateSensorDto updateSensorDto);
        Task UpdateSensorStatusAsync(Guid sensorId, bool isActive, bool isWorking);
        Task CreateSensorsForWasteBinAsync(Guid wasteBinId, int sensorCount);
        Task UpdateSensorsForWasteBinAsync(Guid wasteBinId, int newSensorCount);
        Task DeleteSensorsByWasteBinIdAsync(Guid wasteBinId);

    }
}
