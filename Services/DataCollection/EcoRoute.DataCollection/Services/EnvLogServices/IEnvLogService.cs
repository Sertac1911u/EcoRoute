using EcoRoute.DataCollection.Dtos.EnvLogDtos;

namespace EcoRoute.DataCollection.Services.EnvLogServices
{
    public interface IEnvLogService
    {
        Task<List<ResultEnvLogDto>> GetAllEnvLogAsync();
        Task CreateEnvLogAsync(CreateEnvLogDto createEnvLogDto);
        Task UpdateEnvLogAsync(UpdateEnvLogDto updateEnvLogDto);
        Task DeleteEnvLogAsync(Guid id);
        Task<GetByIdEnvLogDto> GetByIdEnvLogAsync(Guid id);
    }
}
