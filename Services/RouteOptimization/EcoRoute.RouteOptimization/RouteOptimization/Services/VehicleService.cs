using EcoRoute.RouteOptimization.Context;
using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.RouteOptimization.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly RouteDbContext _context;

        public VehicleService(RouteDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResultVehicleDto>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles
                .Select(v => new ResultVehicleDto
                {
                    Id = v.Id,
                    Plate = v.Plate,
                    Description = v.Description
                })
                .ToListAsync();
        }

        public async Task<ResultVehicleDto> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                throw new Exception("Araç bulunamadı.");

            return new ResultVehicleDto
            {
                Id = vehicle.Id,
                Plate = vehicle.Plate,
                Description = vehicle.Description
            };
        }

        public async Task<ResultVehicleDto> CreateVehicleAsync(CreateVehicleDto dto)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Plate = dto.Plate,
                Description = dto.Description
            };

            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return new ResultVehicleDto
            {
                Id = vehicle.Id,
                Plate = vehicle.Plate,
                Description = vehicle.Description
            };
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return false;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
