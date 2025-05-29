using EcoRoute.DataCollection.Dtos;
using EcoRoute.DataCollection.Dtos.WasteBinDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.DataCollection.Services
{
    public class DataCollectionNotificationService : IDataCollectionNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DataCollectionNotificationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DataCollectionNotificationService(
            HttpClient httpClient,
            ILogger<DataCollectionNotificationService> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private async Task SetBearerTokenAsync()
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task SendWasteBinCreatedNotificationAsync(object wasteBinDto)
        {
            await SetBearerTokenAsync();
            var notification = new CreateNotificationDto
            {
                Title = "Yeni Atık Kutusu Eklendi",
                Message = $"\"{GetLabel(wasteBinDto)}\" konumunda yeni bir atık kutusu oluşturuldu.",
                Type = "Info",
                UserId = "", // Herkese
                UserRole = "Manager,SuperAdmin,Driver", // Sadece yönetici
                Url = "/bins"
            };
            await _httpClient.PostAsJsonAsync("http://localhost:5008/api/Notifications", notification);
        }

        public async Task SendWasteBinUpdatedNotificationAsync(object wasteBinDto)
        {
            await SetBearerTokenAsync();
            var notification = new CreateNotificationDto
            {
                Title = "Atık Kutusu Güncellendi",
                Message = $"\"{GetLabel(wasteBinDto)}\" konumundaki atık kutusu güncellendi.",
                Type = "Info",
                UserId = "",
                UserRole = "Manager,SuperAdmin,Driver",
                Url = "/bins"
            };
            await _httpClient.PostAsJsonAsync("http://localhost:5008/api/Notifications", notification);
        }

        public async Task SendWasteBinDeletedNotificationAsync(object wasteBinDto)
        {
            await SetBearerTokenAsync();
            var notification = new CreateNotificationDto
            {
                Title = "Atık Kutusu Silindi",
                Message = $"\"{GetLabel(wasteBinDto)}\" konumundaki atık kutusu silindi.",
                Type = "Warning",
                UserId = "",
                UserRole = "Manager,SuperAdmin,Driver",
                Url = "/bins"
            };
            await _httpClient.PostAsJsonAsync("http://localhost:5008/api/Notifications", notification);
        }

        // Helper method to get label from dto (ResultWasteBinDto veya benzeri)
        private string GetLabel(object dto)
        {
            // Dinamik tip desteği, Label alanı varsa getir
            var labelProp = dto?.GetType().GetProperty("Label");
            return labelProp?.GetValue(dto)?.ToString() ?? "Atık Kutusu";
        }
    }
}
