using AutoMapper;
using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Dtos.WasteBinDtos;
using EcoRoute.DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Services.WasteBinServices
{
    public class WasteBinService : IWasteBinService
    {
        private readonly DataCollectionContext _context;
        private readonly IMapper _mapper;

        public WasteBinService(DataCollectionContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateWasteBinAsync(CreateWasteBinDto createWasteBinDto)
        {
            var wasteBin = _mapper.Map<WasteBin>(createWasteBinDto);
            await _context.WasteBins.AddAsync(wasteBin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWasteBinAsync(Guid id)
        {
            var wasteBin = await _context.WasteBins.FirstOrDefaultAsync(w => w.WasteBinId == id);
            _context.WasteBins.Remove(wasteBin);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultWasteBinDto>> GetAllWasteBinAsync()
        {
            var wasteBins = await _context.WasteBins.ToListAsync(); 
            return _mapper.Map<List<ResultWasteBinDto>>(wasteBins); 
        }

        public async Task<GetByIdWasteBinDto> GetByIdWasteBinAsync(Guid id)
        {
            var wastebyId = await _context.WasteBins.FirstOrDefaultAsync(w => w.WasteBinId == id);
            return _mapper.Map<GetByIdWasteBinDto>(wastebyId);
        }

        public async Task UpdateWasteBinAsync(UpdateWasteBinDto updateWasteBinDto)
        {
            var updateWasteBin = await _context.WasteBins.FirstOrDefaultAsync(w => w.WasteBinId == updateWasteBinDto.WasteBinId);
            _mapper.Map(updateWasteBinDto,updateWasteBin);
            _context.WasteBins.Update(updateWasteBin);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ResultWasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> ids)
        {
            var wasteBins = await _context.WasteBins
                                           .Where(w => ids.Contains(w.WasteBinId))
                                           .ToListAsync();
            return _mapper.Map<List<ResultWasteBinDto>>(wasteBins);
        }

    }
}
