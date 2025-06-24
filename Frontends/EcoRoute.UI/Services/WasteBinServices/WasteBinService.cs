using EcoRoute.DtoLayer.WasteBinDtos;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using System.Text;

namespace EcoRoute.UI.Services.WasteBinServices
{
    public class WasteBinService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public WasteBinService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ResultWasteBinDto>> GetAllWasteBinsAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<List<ResultWasteBinDto>>("services/datacollection/WasteBins");
            return response ?? new List<ResultWasteBinDto>();
        }

        public async Task<GetByIdWasteBinDto> GetWasteBinByIdAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<GetByIdWasteBinDto>($"services/datacollection/WasteBins/{id}");
            return response;
        }

        public async Task<bool> CreateWasteBinAsync(CreateWasteBinDto createWasteBinDto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("services/datacollection/WasteBins", createWasteBinDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateWasteBinAsync(UpdateWasteBinDto updateWasteBinDto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync("services/datacollection/WasteBins", updateWasteBinDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteWasteBinAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"services/datacollection/WasteBins/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ResultWasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> ids)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var idsString = string.Join("&id=", ids);
            var response = await _httpClient.GetFromJsonAsync<List<ResultWasteBinDto>>($"services/datacollection/WasteBins/selected?id={idsString}");
            return response ?? new List<ResultWasteBinDto>();
        }

        public async Task<double?> GetEstimatedFillLevelAsync(List<double> fillLevels)
        {
            // Flask API adresin
            var flaskUrl = "http://127.0.0.1:4002/predict";

            // Body hazırla (fillLevels array olarak)
            var requestBody = new
            {
                fillLevels = fillLevels
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var http = new HttpClient();
            var response = await http.PostAsync(flaskUrl, content);

            if (!response.IsSuccessStatusCode)
                return null;

            // Dönen JSON'u oku ve estimatedFillLevel'i al
            var jsonString = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
            if (doc.RootElement.TryGetProperty("estimatedFillLevel", out var estimatedFill))
                return estimatedFill.GetDouble();

            return null;
        }

        public async Task<double?> GetFillLevelFromSensorAsync()
        {
            var flaskUrl = "http://192.168.137.169:4003/fill-level";

            using var http = new HttpClient();
            try
            {
                var response = await http.GetFromJsonAsync<FillLevelResponse>(flaskUrl);
                return response?.fill_percent;
            }
            catch
            {
                return null;
            }
        }

        // Yardımcı DTO
        private class FillLevelResponse
        {
            public double fill_percent { get; set; }
            public string message { get; set; }
            public List<bool> raw_light_states { get; set; }
        }

    }
}