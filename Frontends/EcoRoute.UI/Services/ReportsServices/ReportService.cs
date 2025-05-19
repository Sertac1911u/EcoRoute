using Blazored.LocalStorage;
using EcoRoute.DtoLayer.ReportsDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EcoRoute.UI.Services.ReportsServices
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ReportService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<WasteBinReportDto>> GetWasteBinReportAsync()
        {
            await SetAuthorizationHeader();

            try
            {
                // Ocelot konfigürasyonuna göre doğru endpoint
                var response = await _httpClient.GetFromJsonAsync<List<WasteBinReportDto>>("services/reports/wastebins");
                return response ?? new List<WasteBinReportDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Atık kutuları raporu alınamadı: {ex.Message}");
                return new List<WasteBinReportDto>();
            }
        }

        public async Task<List<SensorReportDto>> GetSensorReportAsync()
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<SensorReportDto>>("services/reports/sensors");
                return response ?? new List<SensorReportDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Sensör raporu alınamadı: {ex.Message}");
                return new List<SensorReportDto>();
            }
        }

        public async Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync()
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RoutePerformanceReportDto>>("services/reports/routes/performance");
                return response ?? new List<RoutePerformanceReportDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Rota performans raporu alınamadı: {ex.Message}");
                return new List<RoutePerformanceReportDto>();
            }
        }

        public async Task<List<RouteReportDto>> GetRouteReportAsync()
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RouteReportDto>>("services/reports/routes");
                return response ?? new List<RouteReportDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Rota raporu alınamadı: {ex.Message}");
                return new List<RouteReportDto>();
            }
        }

        public async Task<List<UserActivityReportDto>> GetUserActivityReportAsync()
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<UserActivityReportDto>>("services/reports/users/activity");
                return response ?? new List<UserActivityReportDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Kullanıcı aktivite raporu alınamadı: {ex.Message}");
                return new List<UserActivityReportDto>();
            }
        }

        public async Task<List<CO2EmissionReportDto>> GetCO2EmissionReportAsync()
        {
            await SetAuthorizationHeader();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CO2EmissionReportDto>>("services/reports/routes/co2");
                return response ?? new List<CO2EmissionReportDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CO2 emisyon raporu alınamadı: {ex.Message}");
                return new List<CO2EmissionReportDto>();
            }
        }

        // Ekstra yardımcı metodlar

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

                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Rapor dışa aktarılamadı: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        public async Task<WasteBinStatsDto> GetWasteBinStatsAsync()
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.GetFromJsonAsync<WasteBinStatsDto>("services/reports/wastebins/stats");
                return response ?? new WasteBinStatsDto();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Atık kutusu istatistikleri alınamadı: {ex.Message}");
                return new WasteBinStatsDto();
            }
        }

    }
}
