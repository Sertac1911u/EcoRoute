using EcoRoute.DataCollection.Dtos.SensorDtos;

namespace EcoRoute.DataCollection.Services.SensorServices
{
    public interface ISensorService
    {
        Task<List<ResultSensorDto>> GetAllSensorAsync();
        Task CreateSensorAsync(CreateSensorDto createSensorDto);
        Task UpdateSensorAsync(UpdateSensorDto updateSensorDto);
        Task DeleteSensorAsync(Guid id);
        Task<GetByIdSensorDto> GetByIdSensorAsync(Guid id);
    }
}
