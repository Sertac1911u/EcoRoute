using AutoMapper;
using EcoRoute.RouteOptimization.Context;
using EcoRoute.RouteOptimization.Dtos.RouteDtos;
using EcoRoute.RouteOptimization.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.RouteOptimization.Services.MyRouteServices
{
    public class MyRouteService : IMyRouteService
    {
        private readonly RouteOptimizationContext _context;
        private readonly IMapper _mapper;

        public MyRouteService(RouteOptimizationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateRouteAsync(CreateRouteDto createRouteDto)
        {
            var value = _mapper.Map<MyRoute>(createRouteDto);
            await _context.Routes.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRouteAsync(Guid id)
        {
            var value = await _context.Routes.FirstOrDefaultAsync(x => x.MyRouteId == id);
            _context.Routes.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultRouteDto>> GetAllRoutesAsync()
        {
            var value = await _context.Routes.ToListAsync();
            return _mapper.Map<List<ResultRouteDto>>(value);
        }

        public async Task<GetByIdRouteDto> GetByIdRouteAsync(Guid id)
        {
            var log = await _context.Routes.FirstOrDefaultAsync(b => b.MyRouteId == id);
            return _mapper.Map<GetByIdRouteDto>(log);
        }

        public async Task UpdateRouteAsync(UpdateRouteDto updateRouteDto)
        {
            var ur = await _context.Routes.FirstOrDefaultAsync(ur => ur.MyRouteId == updateRouteDto.RouteId);
            _mapper.Map(updateRouteDto, ur);
            _context.Routes.Update(ur);
            await _context.SaveChangesAsync();
        }
    }
}
