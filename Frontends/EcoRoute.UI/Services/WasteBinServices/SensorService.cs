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

        public async Task<List<ResultSensorDto>> GetAvailableSensorsAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<List<ResultSensorDto>>("services/datacollection/Sensors/available");
            return response ?? new List<ResultSensorDto>();
        }

        public async Task<List<ResultSensorDto>> GetSensorsByWasteBinIdAsync(Guid wasteBinId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<List<ResultSensorDto>>($"services/datacollection/Sensors/bybin/{wasteBinId}");
            return response ?? new List<ResultSensorDto>();
        }

        public async Task<bool> CreateSensorAsync(CreateSensorDto createSensorDto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("services/datacollection/Sensors", createSensorDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSensorAsync(UpdateSensorDto updateSensorDto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync("services/datacollection/Sensors", updateSensorDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSensorAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"services/datacollection/Sensors?id={id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AssignSensorToWasteBinAsync(Guid sensorId, Guid wasteBinId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"services/datacollection/Sensors/assign?sensorId={sensorId}&wasteBinId={wasteBinId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UnassignSensorFromWasteBinAsync(Guid sensorId)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"services/datacollection/Sensors/unassign?sensorId={sensorId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSensorStatusAsync(Guid sensorId, bool isActive)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"services/datacollection/Sensors/status?sensorId={sensorId}&isActive={isActive}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
