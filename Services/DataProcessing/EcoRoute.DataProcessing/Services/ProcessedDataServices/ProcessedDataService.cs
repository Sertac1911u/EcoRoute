using AutoMapper;
using EcoRoute.DataProcessing.Context;
using EcoRoute.DataProcessing.Dtos.ProcessedDataDtos;
using EcoRoute.DataProcessing.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataProcessing.Services.ProcessedDataServices
{
    public class ProcessedDataService : IProcessedDataService
    {
        private readonly DataProcessingContext _context;
        private readonly IMapper _mapper;

        public ProcessedDataService(DataProcessingContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateProcessedDataAsync(CreateProcessedDataDto createProcessedDataDto)
        {
            var value = _mapper.Map<ProcessedData>(createProcessedDataDto);
            await _context.processedDatas.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProcessedDataAsync(Guid id)
        {
            var value = await _context.processedDatas.FirstOrDefaultAsync(x => x.ProcessedDataId == id);
            _context.processedDatas.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultProcessedDataDto>> GetAllProcessedDataAsync()
        {
            var value = await _context.processedDatas.ToListAsync();
            return _mapper.Map<List<ResultProcessedDataDto>>(value);    
        }

        public async Task<GetByIdProcessedDataDto> GetByIdProcessedDataAsync(Guid id)
        {
            var value = await _context.processedDatas.FirstOrDefaultAsync(x => x.ProcessedDataId == id);
            return _mapper.Map<GetByIdProcessedDataDto>(value);
        }

        public async Task UpdateProcessedDataAsync(UpdateProcessedDataDto updateProcessedDataDto)
        {
            var value = await _context.processedDatas.FirstOrDefaultAsync(x => x.ProcessedDataId == updateProcessedDataDto.ProcessedDataId);
            _mapper.Map(updateProcessedDataDto, value);
            _context.processedDatas.Update(value);
            await _context.SaveChangesAsync();
        }
    }
}
