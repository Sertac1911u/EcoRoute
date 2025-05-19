using Blazored.LocalStorage;
using EcoRoute.DtoLayer.SupportDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EcoRoute.UI.Services.SupportsServices
{
    public class SupportTicketService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<SupportTicketService> _logger;

        public SupportTicketService(HttpClient httpClient, ILocalStorageService localStorage, ILogger<SupportTicketService> logger)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _logger = logger;
        }

        public async Task<List<ResultSupportTicketDto>> GetAllSupportTicketsAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ResultSupportTicketDto>>("services/supports/SupportTickets");
                return response ?? new List<ResultSupportTicketDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting support tickets");
                throw;
            }
        }

        public async Task<GetByIdSupportTicketDto> GetSupportTicketByIdAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetByIdSupportTicketDto>($"services/supports/SupportTickets/{id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting support ticket with ID {id}");
                throw;
            }
        }

        public async Task<Guid> CreateSupportTicketAsync(MultipartFormDataContent content)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.PostAsync("services/supports/SupportTickets", content);

                response.EnsureSuccessStatusCode();

                // Try to get ID from Location header first
                var locationHeader = response.Headers.Location;
                if (locationHeader != null)
                {
                    var segments = locationHeader.Segments;
                    if (segments.Length > 0)
                    {
                        var idStr = segments[segments.Length - 1];
                        if (Guid.TryParse(idStr, out Guid id))
                        {
                            return id;
                        }
                    }
                }

                // If Location header doesn't work, try to parse the response body
                try
                {
                    var result = await response.Content.ReadFromJsonAsync<Guid>();
                    return result;
                }
                catch (Exception ex)
                {
                    // If we got a 201 but couldn't extract an ID, just create a new GUID
                    _logger.LogWarning($"Couldn't parse response content: {ex.Message}");
                    return Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating support ticket");
                throw;
            }
        }

        public async Task<bool> AddResponseAsync(MultipartFormDataContent content)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                // Extract the ticket ID for debugging
                var supportTicketId = content.FirstOrDefault(c => c.Headers.ContentDisposition?.Name?.Trim('"') == "SupportTicketId");
                var ticketIdValue = supportTicketId != null ? await supportTicketId.ReadAsStringAsync() : "Not found";

                // Log the content being sent
                _logger.LogInformation("Sending reply to ticket ID: {TicketId}", ticketIdValue);
                _logger.LogInformation("Content keys: {Keys}",
                    string.Join(", ", content.Select(c => c.Headers.ContentDisposition?.Name?.Trim('"'))));

                var response = await _httpClient.PostAsync("services/supports/SupportTickets/reply", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error adding response. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, errorContent);

                    throw new HttpRequestException(
                        $"Error adding response: {response.StatusCode}. Server response: {errorContent}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding response");
                throw;
            }
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var dto = new UpdateStatusDto { Id = id, Status = status };
                _logger.LogInformation("Updating status for ticket {Id} to {Status}", id, status);

                var response = await _httpClient.PutAsJsonAsync($"services/supports/SupportTickets/{id}/status", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error updating status. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, errorContent);

                    throw new HttpRequestException($"Error updating status: {response.StatusCode}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating status for ticket {id}");
                throw;
            }
        }

        public async Task<bool> CloseTicketAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                // Check if ID is valid before making the request
                if (id == Guid.Empty)
                {
                    _logger.LogError("Cannot close ticket with empty GUID");
                    throw new ArgumentException("Invalid ticket ID (empty GUID)", nameof(id));
                }

                _logger.LogInformation("Closing ticket with ID: {Id}", id);
                var response = await _httpClient.PutAsync($"services/supports/SupportTickets/{id}/close", null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error closing ticket. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, errorContent);

                    throw new HttpRequestException($"Error closing ticket: {response.StatusCode}. Server response: {errorContent}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error closing ticket {id}");
                throw;
            }
        }
    }
}