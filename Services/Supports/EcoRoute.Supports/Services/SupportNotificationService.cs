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
                    Url = $"/supports/"
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
        public async Task SendTicketResponseNotificationAsync(
        ResultTicketResponseDto responseDto,
        string ticketSubject,
        Guid ticketId,
        string recipientUserId)
        {
            try
            {
                // 1) Header’a token ekle
                await SetAuthorizationHeader();

                using var client = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5008/")
                };
                client.DefaultRequestHeaders.Authorization =
                    _httpClient.DefaultRequestHeaders.Authorization;

                // 2) Sunucuda gerçek rollerden tespit et
                var user = _httpContextAccessor.HttpContext?.User;
                var isSuperAdmin = user?.IsInRole("SuperAdmin") ?? false;
                var isManager = user?.IsInRole("Manager") ?? false;

                // 3) Gönderen adı
                var sender = (isSuperAdmin || isManager)
                    ? "Destek Ekibi"
                    : (responseDto.UserName ?? "Bir kullanıcı");

                // 4) Hedefleri belirle
                string targetUserId = "";
                string targetRoles = "";
                if (isSuperAdmin)
                {
                    targetUserId = recipientUserId;   // driver’a
                    targetRoles = "Manager";         // sadece Manager grubuna
                }
                else if (isManager)
                {
                    targetUserId = recipientUserId;   // driver’a
                    targetRoles = "SuperAdmin";      // sadece SuperAdmin grubuna
                }
                else
                {
                    // Driver yanıtı ise
                    targetUserId = "";
                    targetRoles = "SuperAdmin,Manager";
                }

                // 5) Payload
                var notification = new
                {
                    Title = "Destek Talebine Yanıt",
                    Message = $"{sender} \"{ticketSubject}\" konulu destek talebinize yanıt verdi.",
                    Type = "Info",
                    Url = $"/supports/{ticketId}",
                    UserId = targetUserId,
                    UserRole = targetRoles
                };

                // LOG için
                _logger.LogInformation(
                    "Reply notification payload: UserId={UserId}, UserRole={UserRole}",
                    targetUserId, targetRoles);

                // 6) Gönder
                var resp = await client.PostAsJsonAsync("api/Notifications", notification);
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    _logger.LogError(
                        "Reply notification failed ({StatusCode}): {Body}",
                        resp.StatusCode, body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendTicketResponseNotificationAsync");
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