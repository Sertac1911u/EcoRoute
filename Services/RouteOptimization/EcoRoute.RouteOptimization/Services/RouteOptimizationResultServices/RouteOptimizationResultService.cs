using AutoMapper;
using EcoRoute.RouteOptimization.Context;
using EcoRoute.RouteOptimization.Dtos.RouteOptimizationResultDtos;
using EcoRoute.RouteOptimization.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.RouteOptimization.Services.RouteOptimizationResultServices
{
    public class RouteOptimizationResultService : IRouteOptimizationResultService
    {
        private readonly RouteOptimizationContext _context;
        private readonly IMapper _mapper;
        public RouteOptimizationResultService(RouteOptimizationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateRouteOptimizationResultAsync(CreateRouteOptimizationResultDto createRouteOptimizationResultDto)
        {
            var log = _mapper.Map<RouteOptimizationResult>(createRouteOptimizationResultDto);
            await _context.Results.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRouteOptimizationResultAsync(Guid id)
        {
            var s = await _context.Results.FirstOrDefaultAsync(s=>s.RouteOptimizationResultId == id);
            _context.Results.Remove(s);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ResultRouteOptimizationResultDto>> GetRouteOptimizationAsync()
        {
            var s = await _context.Results.ToListAsync();
            return _mapper.Map<List<ResultRouteOptimizationResultDto>>(s);
        }

        public async Task<GetByIdRouteOptimizationResultDto> GetByIdRouteOptimizationResultAsync(Guid id)
        {
            var s = await _context.Results.FirstOrDefaultAsync(s => s.RouteOptimizationResultId == id);
            return _mapper.Map<GetByIdRouteOptimizationResultDto>(s);
        }
        public async Task UpdateRouteOptimizationResultAsync(UpdateRouteOptimizationResultDto updateRouteOptimizationResultDto)
        {
            var s = await _context.Results.FirstOrDefaultAsync(s => s.RouteOptimizationResultId == updateRouteOptimizationResultDto.ResultId);
            _mapper.Map(updateRouteOptimizationResultDto, s);
            _context.Results.Update(s);
            await _context.SaveChangesAsync();
        }
    }
}
