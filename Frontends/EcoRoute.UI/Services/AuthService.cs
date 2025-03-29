using Blazored.LocalStorage;
using EcoRoute.DtoLayer.IdentityDtos;
using EcoRoute.UI.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

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

            if (token != null)
            {
                await _localStorage.SetItemAsync("authToken", token.Token);

                // JWT içindeki roller çözümleniyor
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token.Token);

                var roles = jwt.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

                // İstersen roller listesi de kaydedebilirsin
                await _localStorage.SetItemAsync("userRoles", roles);
            }

            return token;
        }

    }
}
