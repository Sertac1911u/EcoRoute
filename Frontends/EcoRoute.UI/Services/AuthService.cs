using Blazored.LocalStorage;
using EcoRoute.DtoLayer.IdentityDtos;
using EcoRoute.UI.Auth;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<TokenResponseViewModel> LoginAsync(string username, string password)
        {
            var loginDto = new UserLoginDto
            {
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/Logins", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var token = await response.Content.ReadFromJsonAsync<TokenResponseViewModel>();
            return token;
        }

    }
}
