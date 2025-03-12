using AutoMapper;
using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Dtos.EnvLogDtos;
using EcoRoute.DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Services.EnvLogServices
{
    public class EnvLogServices : IEnvLogService
    {
        private readonly DataCollectionContext _context;
        private readonly IMapper _mapper;

        public EnvLogServices(DataCollectionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateEnvLogAsync(CreateEnvLogDto createEnvLogDto)
        {
            var envLog = _mapper.Map<EnvLog>(createEnvLogDto);
            await _context.EnvLogs.AddAsync(envLog);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteEnvLogAsync(Guid id)
        {
            var envLog = await _context.EnvLogs.FirstOrDefaultAsync(e => e.EnvLogId == id);
            _context.EnvLogs.Remove(envLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultEnvLogDto>> GetAllEnvLogAsync()
        {
            var envLog = await _context.EnvLogs.ToListAsync();
            return _mapper.Map<List<ResultEnvLogDto>>(envLog);
        }

        public async Task<GetByIdEnvLogDto> GetByIdEnvLogAsync(Guid id)
        {
            var envLog = await _context.EnvLogs.FirstOrDefaultAsync(e => e.EnvLogId == id);
            return _mapper.Map<GetByIdEnvLogDto>(envLog);
        }

        public async Task UpdateEnvLogAsync(UpdateEnvLogDto updateEnvLogDto)
        {
            var updatedEnvLog = await _context.EnvLogs.FirstOrDefaultAsync(e => e.EnvLogId == updateEnvLogDto.EnvLogId);
            _mapper.Map(updateEnvLogDto, updatedEnvLog);
            _context.EnvLogs.Update(updatedEnvLog);
            await _context.SaveChangesAsync();
        }
    }
}
