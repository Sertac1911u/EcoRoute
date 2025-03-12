using EcoRoute.DataProcessing.Dtos.ProcessedDataDtos;

namespace EcoRoute.DataProcessing.Services.ProcessedDataServices
{
    public interface IProcessedDataService
    {
        Task<List<ResultProcessedDataDto>> GetAllProcessedDataAsync();
        Task CreateProcessedDataAsync(CreateProcessedDataDto createProcessedDataDto);
        Task UpdateProcessedDataAsync(UpdateProcessedDataDto updateProcessedDataDto);
        Task DeleteProcessedDataAsync(Guid id);
        Task<GetByIdProcessedDataDto> GetByIdProcessedDataAsync(Guid id);
    }
}
