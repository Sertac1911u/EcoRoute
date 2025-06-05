using Blazored.LocalStorage;
using EcoRoute.DtoLayer.NotificationDtos;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services.NotificationServices
{
    public class NotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private HubConnection _hubConnection;
        private List<ResultNotificationDto> _notifications = new List<ResultNotificationDto>();
        private bool _hasLoadedInitialNotifications = false;

        public event Action<ResultNotificationDto> OnNotificationReceived;
        public event Action OnNotificationsUpdated;

        public int UnreadCount { get; private set; } = 0;
        public List<ResultNotificationDto> Notifications => _notifications;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public NotificationService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                    return;

                await GetNotificationsAsync();

                Console.WriteLine("Initializing notification service with token");

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5008/notificationHub", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token);
                    })
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<ResultNotificationDto>("ReceiveNotification", (notification) =>
                {
                    if (_notifications.Any(n => n.Id == notification.Id))
                    {
                        return;
                    }

                    Console.WriteLine($"Notification received: {notification.Title}");

                    _notifications.Insert(0, notification);
                    UnreadCount++;

                    OnNotificationReceived?.Invoke(notification);
                    OnNotificationsUpdated?.Invoke();
                });

                _hubConnection.Closed += async (error) =>
                {
                    Console.WriteLine($"Connection closed with error: {error?.Message}");
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    try
                    {
                        await _hubConnection.StartAsync();
                        Console.WriteLine("Reconnected to SignalR hub");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to reconnect: {ex.Message}");
                    }
                };

                try
                {
                    Console.WriteLine("Starting SignalR connection...");
                    await _hubConnection.StartAsync();
                    Console.WriteLine("SignalR connection started successfully");

                    var userId = await GetUserIdFromTokenAsync(token);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await _hubConnection.InvokeAsync("JoinGroup", userId);
                        await _hubConnection.InvokeAsync("JoinGroup", "AllUsers");
                        Console.WriteLine($"Joined groups with user ID: {userId}");
                    }

                    var roles = await GetRolesFromTokenAsync(token);
                    foreach (var role in roles)
                    {
                        var groupName = $"Role_{role}";
                        await _hubConnection.InvokeAsync("JoinGroup", groupName);
                        Console.WriteLine($"Joined SignalR group: {groupName}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                    throw; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing notification service: {ex.Message}");
                throw; 
            }
        }
        private async Task<List<string>> GetRolesFromTokenAsync(string token)
        {
            try
            {
                var tokenParts = token.Split('.');
                if (tokenParts.Length != 3)
                    return new List<string>();

                var payload = tokenParts[1];
                var paddedPayload = payload;

                switch (payload.Length % 4)
                {
                    case 2: paddedPayload += "=="; break;
                    case 3: paddedPayload += "="; break;
                }

                var base64Payload = paddedPayload.Replace('-', '+').Replace('_', '/');
                var jsonPayload = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Payload));

                using var document = System.Text.Json.JsonDocument.Parse(jsonPayload);
                if (document.RootElement.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var roleElement))
                {
                    if (roleElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                        return roleElement.EnumerateArray().Select(r => r.GetString()).ToList();
                    else
                        return new List<string> { roleElement.GetString() };
                }

                return new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decoding roles: {ex.Message}");
                return new List<string>();
            }
        }

        private async Task<string> GetUserIdFromTokenAsync(string token)
        {
            try
            {
                var tokenParts = token.Split('.');
                if (tokenParts.Length != 3)
                    return null;

                var payload = tokenParts[1];
                var paddedPayload = payload;

                switch (payload.Length % 4)
                {
                    case 2: paddedPayload += "=="; break;
                    case 3: paddedPayload += "="; break;
                }

                var base64Payload = paddedPayload.Replace('-', '+').Replace('_', '/');
                var jsonPayload = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Payload));

                using var document = System.Text.Json.JsonDocument.Parse(jsonPayload);
                if (document.RootElement.TryGetProperty("nameid", out var nameIdElement))
                {
                    return nameIdElement.GetString();
                }
                if (document.RootElement.TryGetProperty("sub", out var subElement))
                {
                    return subElement.GetString();
                }

                Console.WriteLine("Could not find user ID in token");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decoding token: {ex.Message}");
                return null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                try
                {
                    if (_hubConnection.State == HubConnectionState.Connected)
                    {
                        var userId = await GetUserIdFromTokenAsync(await _localStorage.GetItemAsync<string>("authToken"));
                        if (!string.IsNullOrEmpty(userId))
                        {
                            await _hubConnection.InvokeAsync("LeaveGroup", userId);
                            await _hubConnection.InvokeAsync("LeaveGroup", "AllUsers");
                        }
                    }
                    await _hubConnection.DisposeAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error disposing hub connection: {ex.Message}");
                }
            }
        }

        public async Task GetNotificationsAsync(bool forceRefresh = false)
        {
            var cachedNotifications = await _localStorage.GetItemAsync<List<ResultNotificationDto>>("notifications");
            if (!forceRefresh && cachedNotifications != null && cachedNotifications.Any() && _hasLoadedInitialNotifications)
            {
                _notifications = cachedNotifications;
                UnreadCount = _notifications.Count(n => !n.IsRead);
                OnNotificationsUpdated?.Invoke();
                return;
            }

            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var notifications = await _httpClient.GetFromJsonAsync<List<ResultNotificationDto>>("services/notifications/Notifications");
                if (notifications != null)
                {
                    _notifications = notifications;
                    UnreadCount = _notifications.Count(n => !n.IsRead);
                    _hasLoadedInitialNotifications = true;

                    await _localStorage.SetItemAsync("notifications", _notifications);

                    OnNotificationsUpdated?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading notifications: {ex.Message}");
            }
        }
        public async Task<int> GetUnreadCountAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var count = await _httpClient.GetFromJsonAsync<int>("services/notifications/Notifications/count");
                UnreadCount = count;
                OnNotificationsUpdated?.Invoke();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting unread count: {ex.Message}");
                return 0;
            }
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsync($"services/notifications/Notifications/{id}/read", null);
                if (response.IsSuccessStatusCode)
                {
                    var notification = _notifications.FirstOrDefault(n => n.Id == id);
                    if (notification != null && !notification.IsRead)
                    {
                        notification.IsRead = true;
                        UnreadCount--;
                        OnNotificationsUpdated?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking notification as read: {ex.Message}");
            }
        }

        public async Task MarkAllAsReadAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsync("services/notifications/Notifications/read-all", null);
                if (response.IsSuccessStatusCode)
                {
                    await GetNotificationsAsync(forceRefresh: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking all notifications as read: {ex.Message}");
            }
        }
    }
}