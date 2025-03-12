using AutoMapper;
using EcoRoute.RouteOptimization.Context;
using EcoRoute.RouteOptimization.Dtos.WaypointDtos;
using EcoRoute.RouteOptimization.Entities;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.RouteOptimization.Services.WaypointServices
{
    public class WaypointService : IWaypointService
    {
        private readonly RouteOptimizationContext _context;
        private readonly IMapper _mapper;
        public WaypointService(RouteOptimizationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateWaypointAsync(CreateWaypointDto createWaypointDto)
        {
            var log = _mapper.Map<Waypoint>(createWaypointDto);
            await _context.Waypoints.AddAsync(log);
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteWaypointAsync(Guid id)
        {
            var s = await _context.Waypoints.FirstOrDefaultAsync(s=>s.WaypointId == id);
            _context.Waypoints.Remove(s);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ResultWaypointDto>> GetAllWaypointAsync()
        {
            var s = await _context.Waypoints.ToListAsync();
            return _mapper.Map<List<ResultWaypointDto>>(s);
        }

        public async Task<GetByIdWaypointDto> GetByIdWaypointAsync(Guid id)
        {
            var s = await _context.Waypoints.FirstOrDefaultAsync(s=>s.WaypointId==id);
            return _mapper.Map<GetByIdWaypointDto>(s);
        }
        public async Task UpdateWaypointAsync(UpdateWaypointDto updateWaypointDto)
        {
            var s = await _context.Waypoints.FirstOrDefaultAsync(s => s.WaypointId == updateWaypointDto.WaypointId);
            _mapper.Map(updateWaypointDto, s);
            _context.Waypoints.Update(s);
            await _context.SaveChangesAsync();
        }

    }
}
