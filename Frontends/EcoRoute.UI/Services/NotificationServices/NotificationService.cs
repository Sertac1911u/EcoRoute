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

        // Events
        public event Action<ResultNotificationDto> OnNotificationReceived;
        public event Action OnNotificationsUpdated;

        // Properties
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
                // Get token
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                    return;

                await GetNotificationsAsync();

                Console.WriteLine("Initializing notification service with token");

                // Build hub connection - Using the gateway URL
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5008/notificationHub", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token);
                    })
                    .WithAutomaticReconnect()
                    .Build();

                // Register for notifications
                _hubConnection.On<ResultNotificationDto>("ReceiveNotification", (notification) =>
                {
                    Console.WriteLine($"Notification received: {notification.Title}");

                    // Add to our local list
                    _notifications.Insert(0, notification);
                    UnreadCount++;

                    // Trigger events
                    OnNotificationReceived?.Invoke(notification);
                    OnNotificationsUpdated?.Invoke();
                });

                // Set up reconnect handler
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

                // Connect to hub
                try
                {
                    Console.WriteLine("Starting SignalR connection...");
                    await _hubConnection.StartAsync();
                    Console.WriteLine("SignalR connection started successfully");

                    // Add user to proper groups
                    var userId = await GetUserIdFromTokenAsync(token);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await _hubConnection.InvokeAsync("JoinGroup", userId);
                        await _hubConnection.InvokeAsync("JoinGroup", "AllUsers");
                        Console.WriteLine($"Joined groups with user ID: {userId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                    throw; // Rethrow to see the detailed exception in the browser console
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing notification service: {ex.Message}");
                throw; // Rethrow so we can see the detailed error
            }
        }

        private async Task<string> GetUserIdFromTokenAsync(string token)
        {
            try
            {
                // Simple JWT decoder to extract user ID
                var tokenParts = token.Split('.');
                if (tokenParts.Length != 3)
                    return null;

                var payload = tokenParts[1];
                var paddedPayload = payload;

                // Add padding if needed
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

        public async Task GetNotificationsAsync()
        {
            // Cache'den bildirimleri yükle
            var cachedNotifications = await _localStorage.GetItemAsync<List<ResultNotificationDto>>("notifications");
            if (cachedNotifications != null && cachedNotifications.Any() && _hasLoadedInitialNotifications)
            {
                _notifications = cachedNotifications;
                UnreadCount = _notifications.Count(n => !n.IsRead);
                OnNotificationsUpdated?.Invoke();
                return;
            }

            try
            {
                // API'den bildirimleri getir
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var notifications = await _httpClient.GetFromJsonAsync<List<ResultNotificationDto>>("services/notifications/Notifications");
                if (notifications != null)
                {
                    _notifications = notifications;
                    UnreadCount = _notifications.Count(n => !n.IsRead);
                    _hasLoadedInitialNotifications = true;

                    // Cache'e kaydet
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
                    foreach (var notification in _notifications)
                    {
                        notification.IsRead = true;
                    }
                    UnreadCount = 0;
                    OnNotificationsUpdated?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking all notifications as read: {ex.Message}");
            }
        }
    }
}