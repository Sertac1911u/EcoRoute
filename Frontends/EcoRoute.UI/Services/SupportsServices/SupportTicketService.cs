using Blazored.LocalStorage;
using EcoRoute.DtoLayer.SupportDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services.SupportsServices
{
    public class SupportTicketService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public SupportTicketService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ResultSupportTicketDto>> GetAllSupportTicketsAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<List<ResultSupportTicketDto>>("services/supports/SupportTickets");
            return response ?? new List<ResultSupportTicketDto>();
        }

        public async Task<GetByIdSupportTicketDto> GetSupportTicketByIdAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<GetByIdSupportTicketDto>($"services/supports/SupportTickets/{id}");
            return response;
        }

        public async Task<Guid> CreateSupportTicketAsync(MultipartFormDataContent content)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync("services/supports/SupportTickets", content);

            if (response.IsSuccessStatusCode)
            {
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
                    Console.WriteLine($"Couldn't parse response content: {ex.Message}");
                    return Guid.NewGuid();
                }
            }

            throw new HttpRequestException($"Error creating support ticket: {response.StatusCode}");
        }

        public async Task<bool> AddResponseAsync(MultipartFormDataContent content)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync("services/supports/SupportTickets/reply", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new HttpRequestException($"Error adding response: {response.StatusCode}");
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new UpdateStatusDto { Id = id, Status = status };
            var response = await _httpClient.PutAsJsonAsync($"services/supports/SupportTickets/{id}/status", dto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new HttpRequestException($"Error updating status: {response.StatusCode}");
        }

        public async Task<bool> CloseTicketAsync(Guid id)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"services/supports/SupportTickets/{id}/close", null);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new HttpRequestException($"Error closing ticket: {response.StatusCode}");
        }
    }
}