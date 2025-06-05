using EcoRoute.Reports.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EcoRoute.Reports.Services
{
    public class ReportService : IReportService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthHeader(HttpClient client)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }
        }

        public async Task<List<WasteBinReportDto>> GetWasteBinReportAsync()
        {
            var client = _httpClientFactory.CreateClient("DataCollectionClient");
            AddAuthHeader(client);

            var response = await client.GetAsync("services/datacollection/wastebins");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Atık kutuları alınamadı. Status: {(int)response.StatusCode} - {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<WasteBinReportDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        public async Task<List<RoutePerformanceReportDto>> GetRoutePerformanceReportAsync()
        {
            var client = _httpClientFactory.CreateClient("RouteOptimizationClient");
            AddAuthHeader(client);

            var response = await client.GetAsync("routeoptimization/route/performance-report");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Rota performans raporları alınamadı: {(int)response.StatusCode} - {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var reports = JsonSerializer.Deserialize<List<RoutePerformanceReportDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            var driverIds = reports.Select(r => r.DriverId).Distinct().ToList();
            var driverNames = await GetDriverNamesAsync(driverIds);

            foreach (var report in reports)
            {
                report.DriverName = driverNames.GetValueOrDefault(report.DriverId);
            }

            return reports;
        }

        public async Task<List<SensorReportDto>> GetSensorReportAsync()
        {
            var client = _httpClientFactory.CreateClient("SensorClient");
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }

            var response = await client.GetAsync("services/datacollection/sensors/");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Sensör raporları alınamadı: {(int)response.StatusCode} - {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<SensorReportDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }

        public async Task<List<CO2EmissionReportDto>> GetCO2EmissionReportAsync(int days = 30)
        {
            var client = _httpClientFactory.CreateClient("RouteOptimizationClient");
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }

            var response = await client.GetAsync($"routeoptimization/route/co2-stats?days={days}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"CO2 emisyon raporu alınamadı: {(int)response.StatusCode} - {error}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var wrapper = JsonSerializer.Deserialize<CO2StatsResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return wrapper?.DailyStats ?? new();

        }

        public async Task<List<UserActivityReportDto>> GetUserActivityReportAsync()
        {
            var client = _httpClientFactory.CreateClient("UserClient");
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }

            var response = await client.GetAsync("api/users/activity");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Kullanıcı aktivite raporu alınamadı: {(int)response.StatusCode} - {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserActivityReportDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }

        public async Task<List<RouteReportDto>> GetRouteReportAsync()
        {
            var client = _httpClientFactory.CreateClient("RouteOptimizationClient");
            AddAuthHeader(client);

            var response = await client.GetAsync("routeoptimization/route/all");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Rota raporları alınamadı: {(int)response.StatusCode} - {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var routes = JsonSerializer.Deserialize<List<RouteReportDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            var driverIds = routes.Select(r => r.DriverId).Distinct().ToList();
            var driverNames = await GetDriverNamesAsync(driverIds);

            foreach (var route in routes)
            {
                route.DriverName = driverNames.GetValueOrDefault(route.DriverId);
            }

            return routes;
        }
        private async Task<Dictionary<string, string>> GetDriverNamesAsync(IEnumerable<string> driverIds)
        {
            var client = _httpClientFactory.CreateClient("UserClient");
            AddAuthHeader(client);

            var query = string.Join("&", driverIds.Select(id => $"id={id}"));
            var response = await client.GetAsync($"api/users/getnamesbyids?{query}");

            if (!response.IsSuccessStatusCode)
                return new Dictionary<string, string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }
        public async Task<WasteBinStatsDto> GetWasteBinStatsAsync()
        {
            var bins = await GetWasteBinReportAsync();
            if (bins == null || bins.Count == 0)
            {
                return new WasteBinStatsDto
                {
                    AverageFill = 0,
                    MaxFill = 0,
                    MinFill = 0,
                    TotalBins = 0
                };
            }

            var fillLevels = bins
                .Where(b => b.FillLevel.HasValue)
                .Select(b => b.FillLevel.Value)
                .ToList();

            return new WasteBinStatsDto
            {
                AverageFill = fillLevels.Count > 0 ? fillLevels.Average() : 0,
                MaxFill = fillLevels.Count > 0 ? fillLevels.Max() : 0,
                MinFill = fillLevels.Count > 0 ? fillLevels.Min() : 0,
                TotalBins = bins.Count
            };
        }

    }
}
