using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Dtos.TicketResponseDto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.Supports.Services
{
    public class SupportNotificationService : ISupportNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SupportNotificationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupportNotificationService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<SupportNotificationService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task SetAuthorizationHeader()
        {
            // Kullanıcının mevcut token'ını al
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("Authorization header set with user token");
            }
            else
            {
                _logger.LogWarning("No access token found in current context");
            }
        }

        public async Task SendTicketCreatedNotificationAsync(ResultSupportTicketDto ticket)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5008/");
                client.DefaultRequestHeaders.Authorization = _httpClient.DefaultRequestHeaders.Authorization;

                var notification = new
                {
                    Title = "Yeni Destek Talebi",
                    Message = $"{ticket.UserName} tarafından \"{ticket.Subject}\" konulu yeni bir destek talebi oluşturuldu.",
                    Type = "Info",
                    UserId = "", // Boş bırakıp...
                    UserRole = "Admin,SuperAdmin,Manager", // ...sadece yönetici rollerini belirt
                    Url = $"/supports/{ticket.Id}"
                };

                var response = await client.PostAsJsonAsync("api/Notifications", notification);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to send ticket created notification: {response.StatusCode}. Details: {content}");
                }
                else
                {
                    _logger.LogInformation("Ticket created notification sent successfully!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending ticket created notification");
            }
        }
        public async Task SendTicketResponseNotificationAsync(ResultTicketResponseDto responseDto, string ticketSubject, Guid ticketId, string recipientUserId)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5008/");
                client.DefaultRequestHeaders.Authorization = _httpClient.DefaultRequestHeaders.Authorization;

                var sender = responseDto.IsStaff ? "Destek Ekibi" : responseDto.UserName ?? "Bir kullanıcı";

                // Eğer personel yanıt veriyorsa, sadece ticket sahibine bildirim gönder
                if (responseDto.IsStaff)
                {
                    var notification = new
                    {
                        Title = "Destek Talebine Yanıt",
                        Message = $"{sender} \"{ticketSubject}\" konulu destek talebinize yanıt verdi.",
                        Type = "Info",
                        UserId = recipientUserId, // Ticket sahibinin ID'si
                        UserRole = "", // Rol değil, spesifik kullanıcıya gönder
                        Url = $"/supports/{ticketId}"
                    };

                    var response = await client.PostAsJsonAsync("api/Notifications", notification);

                    if (!response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Failed to send ticket response notification: {response.StatusCode}. Details: {content}");
                    }
                }
                // Eğer kullanıcı yanıt veriyorsa, sadece yöneticilere bildirim gönder
                else
                {
                    var notification = new
                    {
                        Title = "Destek Talebine Yanıt",
                        Message = $"{sender} \"{ticketSubject}\" konulu destek talebine yanıt verdi.",
                        Type = "Info",
                        UserId = "", // Spesifik kullanıcı değil
                        UserRole = "Admin,SuperAdmin,Manager", // Sadece yönetici rollerine
                        Url = $"/supports/{ticketId}"
                    };

                    var response = await client.PostAsJsonAsync("api/Notifications", notification);

                    if (!response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Failed to send ticket response notification: {response.StatusCode}. Details: {content}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending ticket response notification");
            }
        }
        public async Task SendTicketStatusChangedNotificationAsync(string ticketSubject, Guid ticketId, string status, string recipientUserId)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5008/");
                client.DefaultRequestHeaders.Authorization = _httpClient.DefaultRequestHeaders.Authorization;

                // Durum değişikliği bildirimi sadece talep sahibine gönderilmeli
                var notification = new
                {
                    Title = "Destek Talebi Durumu Değişti",
                    Message = $"\"{ticketSubject}\" konulu destek talebinizin durumu \"{status}\" olarak güncellendi.",
                    Type = status == "Kapatıldı" ? "Warning" : (status == "Çözüldü" ? "Success" : "Info"),
                    UserId = recipientUserId, // Sadece talep sahibine
                    UserRole = "", // Rol değil, spesifik kullanıcı
                    Url = $"/supports/{ticketId}"
                };

                var response = await client.PostAsJsonAsync("api/Notifications", notification);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to send ticket status changed notification: {response.StatusCode}. Details: {content}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending ticket status changed notification");
            }
        }
    }
}