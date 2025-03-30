using Blazored.LocalStorage;
using EcoRoute.DtoLayer.IdentityDtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EcoRoute.UI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ResultUserDto>> GetAllUsersAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return new List<ResultUserDto>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5001/api/users/GetAllUserList");
            if (!response.IsSuccessStatusCode)
                return new List<ResultUserDto>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ResultUserDto>>(json) ?? new List<ResultUserDto>();
        }
        public async Task<bool> UpdateUserAsync(UpdateUserDto user)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("http://localhost:5001/api/users/UpdateUser", content);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> CreateUserAsync(CreateUserDto user)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5001/api/users/CreateUser", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return false;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5001/api/users/DeleteUser/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
