using Blazored.LocalStorage;
using EcoRoute.DtoLayer.ReportsDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EcoRoute.UI.Services.ReportsServices
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            ILogger<ReportService> logger)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _logger = logger;
        }

        /// <summary>
        /// Sets the authorization header with JWT token from local storage
        /// </summary>
        private async Task SetAuthorizationHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        /// <summary>
        /// Generic method to get data from the API with proper error handling
        /// </summary>
        private async Task<TResult> GetAsync<TResult>(string endpoint) where TResult : new()
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Error: {StatusCode} - {Error}",
                        (int)response.StatusCode, errorContent);

                    return new TResult();
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TResult>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? new TResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling {Endpoint}: {Message}", endpoint, ex.Message);
                return new TResult();
            }
        }

        public async Task<List<WasteBinReportDto>> GetWasteBinReportAsync()
        {
            return await GetAsync<List<WasteBinReportDto>>("services/reports/wastebins");
        }

        public async Task<List<SensorReportDto>> GetSensorReportAsync()
        {
            return await GetAsync<List<SensorReportDto>>("services/reports/sensors");
        }

        public async Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync()
        {
            return await GetAsync<List<RoutePerformanceReportDto>>("services/reports/routes/performance");
        }

        public async Task<List<RouteReportDto>> GetRouteReportAsync()
        {
            return await GetAsync<List<RouteReportDto>>("services/reports/routes");
        }

        public async Task<List<UserActivityReportDto>> GetUserActivityReportAsync()
        {
            return await GetAsync<List<UserActivityReportDto>>("services/reports/users/activity");
        }

        public async Task<List<CO2EmissionReportDto>> GetCO2EmissionReportAsync(int days = 30)
        {
            return await GetAsync<List<CO2EmissionReportDto>>($"services/reports/routes/co2-stats?days={days}");
        }

        public async Task<WasteBinStatsDto> GetWasteBinStatsAsync()
        {
            return await GetAsync<WasteBinStatsDto>("services/reports/wastebins/stats");
        }

        public async Task<byte[]> ExportReportAsync(string reportType, string format)
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetAsync($"services/reports/export/{reportType}?format={format}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Export error: {StatusCode} - {Error}",
                    (int)response.StatusCode, errorContent);

                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting report: {Message}", ex.Message);
                return Array.Empty<byte>();
            }
        }
    }
}