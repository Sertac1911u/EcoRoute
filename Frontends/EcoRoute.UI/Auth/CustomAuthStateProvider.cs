using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EcoRoute.UI.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken.ValidTo > DateTime.UtcNow)
                {
                    var originalClaims = jwtToken.Claims.ToList();

                    var roleClaims = jwtToken.Claims
                        .Where(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        .Select(c => new Claim(ClaimTypes.Role, c.Value))
                        .ToList();

                    var allClaims = new List<Claim>();
                    allClaims.AddRange(originalClaims);
                    allClaims.AddRange(roleClaims);

                    identity = new ClaimsIdentity(allClaims, "jwt");
                }
                else
                {
                    await _localStorage.RemoveItemAsync("authToken");
                }
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var originalClaims = jwtToken.Claims.ToList();

            var roleClaims = jwtToken.Claims
                .Where(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(c => new Claim(ClaimTypes.Role, c.Value))
                .ToList();

            var allClaims = new List<Claim>();
            allClaims.AddRange(originalClaims);
            allClaims.AddRange(roleClaims);

            var identity = new ClaimsIdentity(allClaims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(nobody)));
        }
    }
}
