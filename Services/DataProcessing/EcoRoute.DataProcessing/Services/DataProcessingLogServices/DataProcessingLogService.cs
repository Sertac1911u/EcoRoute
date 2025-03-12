using AutoMapper;
using EcoRoute.DataProcessing.Context;
using EcoRoute.DataProcessing.Dtos.DataProcessingLogDtos;
using EcoRoute.DataProcessing.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataProcessing.Services.DataProcessingLogServices
{
    public class DataProcessingLogService : IDataProcessingLogService
    {
        private readonly DataProcessingContext _context;
        private readonly IMapper _mapper;

        public DataProcessingLogService(DataProcessingContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateDataProcessingLogAsync(CreateDataProcessingLogDto dataProcessingLogDto)
        {
            var value = _mapper.Map<DataProcessingLog>(dataProcessingLogDto);
            await _context.dataProcessingLogs.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDataProcessingLogAsync(Guid id)
        {
            var value = await _context.dataProcessingLogs.FirstOrDefaultAsync(v => v.DataProcessingLogId == id);
            _context.dataProcessingLogs.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultDataProcessingLogDto>> GetAllDataProcessingLogAsync()
        {
            var value = await _context.dataProcessingLogs.ToListAsync();
            return _mapper.Map<List<ResultDataProcessingLogDto>>(value);
        }

        public async Task<GetByIdDataProcessingLogDto> GetByIdDataProcessingLogAsync(Guid id)
        {
            var value = await _context.dataProcessingLogs.FirstOrDefaultAsync(v => v.DataProcessingLogId == id);
            return _mapper.Map<GetByIdDataProcessingLogDto>(value);
        }
    }
}
