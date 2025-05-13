using EcoRoute.DtoLayer.RouteOptimizationDtos;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient;

        public RouteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateRouteAsync(CreateRouteDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("routeoptimization/route", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<RouteResultDto>> GetAllRoutesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<RouteResultDto>>("routeoptimization/route/all");
            return result ?? new();
        }

        public async Task<List<RouteResultDto>> GetMyRoutesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<RouteResultDto>>("routeoptimization/route/my");
            return result ?? new();
        }

        public async Task<bool> CompleteRouteAsync(Guid routeId)
        {
            var response = await _httpClient.PutAsync($"routeoptimization/route/{routeId}/complete", null);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> VehicleHasActiveRoute(Guid vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"routeoptimization/route/HasActiveRoute/{vehicleId}");
        }

        public async Task<bool> DriverHasActiveRoute(string driverId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"routeoptimization/route/HasActiveRouteForDriver/{driverId}");
        }


    }
}
