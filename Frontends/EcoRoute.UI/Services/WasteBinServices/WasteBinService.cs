using EcoRoute.DtoLayer.WasteBinDtos;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

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
    }
}