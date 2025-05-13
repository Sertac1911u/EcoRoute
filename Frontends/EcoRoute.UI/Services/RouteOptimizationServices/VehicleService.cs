using Blazored.LocalStorage;
using EcoRoute.DtoLayer.RouteOptimizationDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public class VehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public VehicleService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ResultVehicleDto>> GetAllVehiclesAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetFromJsonAsync<List<ResultVehicleDto>>("services/routeoptimization/vehicle");
            return result ?? new();
        }
    }
}
