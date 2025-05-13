using EcoRoute.DataCollection.Dtos.WasteBinDtos;

namespace EcoRoute.DataCollection.Services.WasteBinServices
{
    public interface IWasteBinService
    {
        Task<List<ResultWasteBinDto>> GetAllWasteBinAsync();
        Task CreateWasteBinAsync(CreateWasteBinDto createWasteBinDto);
        Task UpdateWasteBinAsync(UpdateWasteBinDto updateWasteBinDto);
        Task DeleteWasteBinAsync(Guid id);
        Task<GetByIdWasteBinDto> GetByIdWasteBinAsync(Guid id);
        Task<List<ResultWasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> ids);

    }
}
