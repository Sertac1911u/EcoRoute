using EcoRoute.DataCollection.Dtos.BinLogDtos;

namespace EcoRoute.DataCollection.Services.BinLogServices
{
    public class BinLogService : IBinLogService
    {
        private readonly 
        public Task CreateBinLogAsync(CreateBinLogDto createBinLogDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBinLogAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultBinLogDto>> GetAllBinLogAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetByIdBinLogDto> GetByIdBinLogAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBinLogAsync(UpdateBinLogDto updateBinLogDto)
        {
            throw new NotImplementedException();
        }
    }
}
