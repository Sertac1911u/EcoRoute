using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Services
{
    public interface IVehicleService
    {
        Task<List<ResultVehicleDto>> GetAllVehiclesAsync();
        Task<ResultVehicleDto> GetVehicleByIdAsync(Guid id);
        Task<ResultVehicleDto> CreateVehicleAsync(CreateVehicleDto dto);
        Task<bool> DeleteVehicleAsync(Guid id);
    }
}
