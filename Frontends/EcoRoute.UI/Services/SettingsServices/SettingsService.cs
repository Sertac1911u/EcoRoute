using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using EcoRoute.DtoLayer.SettingDtos;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EcoRoute.UI.Services.SettingsServices
{

    public class SettingsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public SettingsService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        private async Task<string> GetAuthToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        private async Task SetAuthHeader()
        {
            var token = await GetAuthToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            try
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine($"Current user ID from auth state: {userId}");

                return userId ?? "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting current user ID: {ex.Message}");
                return "";
            }
        }

        public async Task<SystemSettingDto> GetSettingsAsync()
        {
            await SetAuthHeader();

            try
            {
                var userId = await GetCurrentUserIdAsync();
                Console.WriteLine($"Getting settings for user: {userId}");

                var response = await _httpClient.GetFromJsonAsync<SystemSettingDto>("services/settings/Settings");

                if (response != null)
                {
                    Console.WriteLine("Settings loaded successfully");
                    return response;
                }
                else
                {
                    Console.WriteLine("Settings response was null, using defaults");
                    return await GetDefaultSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting settings: {ex.Message}");
                return await GetDefaultSettings();
            }
        }
        public async Task<bool> UpdateSettingsAsync(UpdateSystemSettingDto settings)
        {
            await SetAuthHeader();

            try
            {
                Console.WriteLine($"Auth header present: {_httpClient.DefaultRequestHeaders.Authorization != null}");

                var response = await _httpClient.PutAsJsonAsync("services/settings/Settings", settings);

                Console.WriteLine($"Update settings response status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error content: {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception updating settings: {ex.Message}");
                return false;
            }
        }
    
        public async Task<bool> ResetToDefaultsAsync()
        {
            await SetAuthHeader();

            try
            {
                var response = await _httpClient.PostAsync("services/settings/Settings/reset", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ayarlar sıfırlanırken hata oluştu: {ex.Message}");
                return false;
            }
        }

      
        public async Task<List<ThemeColorDto>> GetThemeColorsAsync()
        {
            await SetAuthHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ThemeColorDto>>("services/settings/Settings/theme-colors");

                if (response != null && response.Any())
                {
                    var originalColor = response.FirstOrDefault(c => c.Name == "Orijinal" || c.Value == "#2ba86d");
                    if (originalColor != null)
                    {
                        response.Remove(originalColor);
                        response.Insert(0, originalColor);
                    }

                    foreach (var color in response)
                    {
                        AddTailwindShades(color);
                    }
                    return response;
                }

                return GetDefaultThemeColors();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tema renkleri alınırken hata oluştu: {ex.Message}");
                return GetDefaultThemeColors();
            }
        }
        private void AddTailwindShades(ThemeColorDto color)
        {
            switch (color.Name)
            {
                case "Mavi":
                    color.Shades = new Dictionary<string, string>
            {
                { "50", "#e8f4fd" },
                { "100", "#c7e3fb" },
                { "200", "#a1d0f8" },
                { "300", "#7abef5" },
                { "400", "#54acf2" },
                { "500", color.Value },
                { "600", "#246fb3" },
                { "700", "#19568f" },
                { "800", "#0f3d6c" },
                { "900", "#062448" }
            };
                    break;
                case "Yeşil":
                    color.Shades = new Dictionary<string, string>
            {
                { "50", "#e7f8ef" },
                { "100", "#c5edd8" },
                { "200", "#9fe3be" },
                { "300", "#74d8a3" },
                { "400", "#4acd87" },
                { "500", color.Value },
                { "600", "#1f8c58" },
                { "700", "#166e44" },
                { "800", "#0e5031" },
                { "900", "#06321d" }
            };
                    break;
                case "Kırmızı":
                    color.Shades = new Dictionary<string, string>
            {
                { "50", "#fef2f2" },
                { "100", "#fee2e2" },
                { "200", "#fecaca" },
                { "300", "#fca5a5" },
                { "400", "#f87171" },
                { "500", color.Value },
                { "600", "#dc2626" },
                { "700", "#b91c1c" },
                { "800", "#991b1b" },
                { "900", "#7f1d1d" }
            };
                    break;
                case "Mor":
                    color.Shades = new Dictionary<string, string>
            {
                { "50", "#f5f3ff" },
                { "100", "#ede9fe" },
                { "200", "#ddd6fe" },
                { "300", "#c4b5fd" },
                { "400", "#a78bfa" },
                { "500", color.Value },
                { "600", "#7c3aed" },
                { "700", "#6d28d9" },
                { "800", "#5b21b6" },
                { "900", "#4c1d95" }
            };
                    break;
                default:
                    color.Shades = GenerateGenericShades(color.Value);
                    break;
            }
        }
        private Dictionary<string, string> GenerateGenericShades(string baseColor)
        {
            return new Dictionary<string, string>
    {
        { "50", LightenColor(baseColor, 0.85) },
        { "100", LightenColor(baseColor, 0.7) },
        { "200", LightenColor(baseColor, 0.5) },
        { "300", LightenColor(baseColor, 0.3) },
        { "400", LightenColor(baseColor, 0.15) },
        { "500", baseColor },
        { "600", DarkenColor(baseColor, 0.15) },
        { "700", DarkenColor(baseColor, 0.3) },
        { "800", DarkenColor(baseColor, 0.45) },
        { "900", DarkenColor(baseColor, 0.6) }
    };
        }

        private string LightenColor(string hexColor, double factor)
        {
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            r = (int)Math.Min(255, r + (255 - r) * factor);
            g = (int)Math.Min(255, g + (255 - g) * factor);
            b = (int)Math.Min(255, b + (255 - b) * factor);

            return $"#{r:X2}{g:X2}{b:X2}";
        }

        private string DarkenColor(string hexColor, double factor)
        {
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            r = (int)Math.Max(0, r * (1 - factor));
            g = (int)Math.Max(0, g * (1 - factor));
            b = (int)Math.Max(0, b * (1 - factor));

            return $"#{r:X2}{g:X2}{b:X2}";
        }
       
        public async Task<List<AvatarDto>> GetAvatarsAsync()
        {
            await SetAuthHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AvatarDto>>("services/settings/Settings/avatars");
                return response ?? GetDefaultAvatars();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Avatarlar alınırken hata oluştu: {ex.Message}");
                return GetDefaultAvatars();
            }
        }

        public async Task<bool> TestGoogleMapsApiAsync(string apiKey)
        {
            await SetAuthHeader();

            try
            {
                var stringContent = new StringContent(
                    JsonSerializer.Serialize(apiKey),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("services/settings/Settings/test-google-maps", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google Maps API testi sırasında hata oluştu: {ex.Message}");
                return false;
            }
        }

        private Task<SystemSettingDto> GetDefaultSettings()
        {
            return Task.FromResult(new SystemSettingDto
            {
                DarkMode = false,
                ThemeColor = "#3B82F6",
                TwoFactorEnabled=false,
                LocationTracking=false,
                ActiveSessionLimit=0,
                SessionTimeout=0,
                EnableAnimations = true,
                AvatarUrl = "https://api.dicebear.com/9.x/shapes/svg?seed=TEST",
                EmailNotifications = true,
                SmsNotifications = false,
                PushNotifications = true,
                GoogleMapsApiKey = ""
            });
        }

        private List<ThemeColorDto> GetDefaultThemeColors()
        {
            var colors = new List<ThemeColorDto>
    {
        new ThemeColorDto { Name = "Orijinal", Value = "#2ba86d" },     
        
        // Diğer renkler
        new ThemeColorDto { Name = "Mavi", Value = "#3B82F6" },           
        new ThemeColorDto { Name = "Kırmızı", Value = "#EF4444" },       
        new ThemeColorDto { Name = "Mor", Value = "#8B5CF6" },            
        new ThemeColorDto { Name = "Pembe", Value = "#EC4899" },        
        new ThemeColorDto { Name = "Sarı", Value = "#F59E0B" },          
        new ThemeColorDto { Name = "Turkuaz", Value = "#06B6D4" },      
        new ThemeColorDto { Name = "Indigo", Value = "#6366F1" },         
        new ThemeColorDto { Name = "Lime", Value = "#84CC16" },           
        new ThemeColorDto { Name = "Gri", Value = "#6B7280" },          
        new ThemeColorDto { Name = "Turuncu", Value = "#F97316" },      
        new ThemeColorDto { Name = "Teal", Value = "#14B8A6" }           
    };

            foreach (var color in colors)
            {
                AddTailwindShades(color);
            }

            return colors;
        }
        private List<AvatarDto> GetDefaultAvatars()
        {
            return new List<AvatarDto>
            {
                new AvatarDto { Name = "Avatar 1", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=TEST" }
            };
        }
        public async Task<List<FontTypeDto>> GetFontTypesAsync()
        {
            await SetAuthHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<FontTypeDto>>("services/settings/Settings/font-types");
                return response ?? new List<FontTypeDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Font tipleri alınırken hata oluştu: {ex.Message}");
                return new List<FontTypeDto>();
            }
        }

        public async Task<List<LanguageDto>> GetLanguagesAsync()
        {
            await SetAuthHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<LanguageDto>>("services/settings/Settings/languages");
                return response ?? new List<LanguageDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Diller alınırken hata oluştu: {ex.Message}");
                return new List<LanguageDto>();
            }
        }

        public async Task<List<DateFormatDto>> GetDateFormatsAsync()
        {
            await SetAuthHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<DateFormatDto>>("services/settings/Settings/date-formats");
                return response ?? new List<DateFormatDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tarih formatları alınırken hata oluştu: {ex.Message}");
                return new List<DateFormatDto>();
            }
        }

    }
}
