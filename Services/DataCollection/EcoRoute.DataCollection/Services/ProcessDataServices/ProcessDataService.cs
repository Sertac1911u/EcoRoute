using AutoMapper;
using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Dtos.ProcessDataDtos;
using EcoRoute.DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Services.ProcessDataServices
{
    public class ProcessDataService : IProcessDataService
    {
        private readonly DataCollectionContext _context;
        private readonly IMapper _mapper;

        public ProcessDataService(DataCollectionContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateProcessDataAsync(CreateProcessDataDto createProcessDataDto)
        {
            var processData = _mapper.Map<ProcessData>(createProcessDataDto);
            await _context.ProcessDatas.AddAsync(processData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProcessDataAsync(Guid id)
        {
            var processData = await _context.ProcessDatas.FirstOrDefaultAsync(p => p.ProcessDataId==id);
            _context.ProcessDatas.Remove(processData);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultProcessDataDto>> GetAllProcessDataAsync()
        {
            var processDatas = await _context.ProcessDatas.ToListAsync();   
            return _mapper.Map<List<ResultProcessDataDto>>(processDatas);
        }

        public async Task<GetByIdProcessDataDto> GetByIdProcessDataAsync(Guid id)
        {
            var processData = await _context.ProcessDatas.FirstOrDefaultAsync(p => p.ProcessDataId == id);
            return _mapper.Map<GetByIdProcessDataDto>(processData);
        }

        public async Task UpdateProcessDataAsync(UpdateProcessDataDto updateProcessDataDto)
        {
            var updatedProcessData = await _context.ProcessDatas.FirstOrDefaultAsync(p => p.ProcessDataId == updateProcessDataDto.ProcessDataId);
            _mapper.Map(updateProcessDataDto,updatedProcessData);
            _context.ProcessDatas.Update(updatedProcessData);
            await _context.SaveChangesAsync();
        }
    }
}
