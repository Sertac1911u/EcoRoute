using AutoMapper;
using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Dtos.BinLogDtos;
using EcoRoute.DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Services.BinLogServices
{
    public class BinLogService : IBinLogService
    {
        private readonly DataCollectionContext _context;
        private readonly IMapper _mapper;

        public BinLogService(DataCollectionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateBinLogAsync(CreateBinLogDto createBinLogDto)
        {
            var binLog = _mapper.Map<BinLog>(createBinLogDto);
            await _context.BinLogs.AddAsync(binLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBinLogAsync(Guid id)
        {
            var binLog = await _context.BinLogs.FirstOrDefaultAsync(c => c.BinLogId == id);
            _context.BinLogs.Remove(binLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultBinLogDto>> GetAllBinLogAsync()
        {
            var binLogs = await _context.BinLogs.ToListAsync();
            return _mapper.Map<List<ResultBinLogDto>>(binLogs);
        }

        public async Task<GetByIdBinLogDto> GetByIdBinLogAsync(Guid id)
        {
            var binLogs = await _context.BinLogs.FirstOrDefaultAsync(b => b.BinLogId == id);
            return _mapper.Map<GetByIdBinLogDto>(binLogs);
        }

        public async Task UpdateBinLogAsync(UpdateBinLogDto updateBinLogDto)
        {
            var updatedBinLog = await _context.BinLogs.FirstOrDefaultAsync(b => b.BinLogId == updateBinLogDto.BinLogId);
            _mapper.Map(updateBinLogDto, updatedBinLog);
            _context.BinLogs.Update(updatedBinLog);
            await _context.SaveChangesAsync();
        }
    }
}
