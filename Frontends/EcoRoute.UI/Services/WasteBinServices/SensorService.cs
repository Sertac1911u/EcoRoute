using Blazored.LocalStorage;
using EcoRoute.DtoLayer.WasteBinDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services.WasteBinServices
{
    public class SensorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public SensorService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ResultSensorDto>> GetAllSensorsAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<List<ResultSensorDto>>("services/datacollection/Sensors");
            return response ?? new List<ResultSensorDto>();
        }

        public async Task<ResultSensorDto> GetSensorByIdAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<ResultSensorDto>($"services/datacollection/Sensors/{id}");
            return response;
        }

        public async Task<List<ResultSensorDto>> GetSensorsByWasteBinIdAsync(Guid wasteBinId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<List<ResultSensorDto>>($"services/datacollection/Sensors/wastebin/{wasteBinId}");
            return response ?? new List<ResultSensorDto>();
        }

        public async Task<bool> UpdateSensorAsync(UpdateSensorDto updateSensorDto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync("services/datacollection/Sensors", updateSensorDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSensorStatusAsync(Guid sensorId, bool isActive, bool isWorking)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"services/datacollection/Sensors/{sensorId}/status?isActive={isActive}&isWorking={isWorking}", null);
            return response.IsSuccessStatusCode;
        }
    }
}