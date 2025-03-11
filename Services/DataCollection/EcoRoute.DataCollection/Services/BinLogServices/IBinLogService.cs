using EcoRoute.DataCollection.Dtos.BinLogDtos;

namespace EcoRoute.DataCollection.Services.BinLogServices
{
    public interface IBinLogService
    {
        Task<List<ResultBinLogDto>> GetAllBinLogAsync();
        Task CreateBinLogAsync(CreateBinLogDto createBinLogDto);
        Task UpdateBinLogAsync(UpdateBinLogDto updateBinLogDto);
        Task DeleteBinLogAsync(Guid id);
        Task<GetByIdBinLogDto> GetByIdBinLogAsync(Guid id);
    }
}
