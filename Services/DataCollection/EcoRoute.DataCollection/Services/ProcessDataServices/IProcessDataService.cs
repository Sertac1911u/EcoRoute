using EcoRoute.DataCollection.Dtos.ProcessDataDtos;

namespace EcoRoute.DataCollection.Services.ProcessDataServices
{
    public interface IProcessDataService
    {
        Task<List<ResultProcessDataDto>> GetAllProcessDataAsync();
        Task CreateProcessDataAsync(CreateProcessDataDto createProcessDataDto);
        Task UpdateProcessDataAsync(UpdateProcessDataDto updateProcessDataDto);
        Task DeleteProcessDataAsync(Guid id);
        Task<GetByIdProcessDataDto> GetByIdProcessDataAsync(Guid id);
    }
}
