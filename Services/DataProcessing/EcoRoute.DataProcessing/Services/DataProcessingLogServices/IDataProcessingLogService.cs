using EcoRoute.DataProcessing.Dtos.DataProcessingLogDtos;

namespace EcoRoute.DataProcessing.Services.DataProcessingLogServices
{
    public interface IDataProcessingLogService
    {
        Task<List<ResultDataProcessingLogDto>> GetAllDataProcessingLogAsync();
        Task CreateDataProcessingLogAsync(CreateDataProcessingLogDto dataProcessingLogDto);
        Task DeleteDataProcessingLogAsync(Guid id);
        Task<GetByIdDataProcessingLogDto> GetByIdDataProcessingLogAsync(Guid id);
    }
}
