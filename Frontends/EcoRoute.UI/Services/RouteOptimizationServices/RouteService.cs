using EcoRoute.DtoLayer.RouteOptimizationDtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace EcoRoute.UI.Services.RouteOptimizationServices
{
    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient;

        public RouteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateRouteAsync(ApiCreateRouteDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("routeoptimization/route", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating route: {ex.Message}");
                return false;
            }
        }

        public async Task<List<RouteResultDto>> GetAllRoutesAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<RouteResultDto>>("routeoptimization/route/all");

                if (result != null)
                {
                    foreach (var route in result)
                    {
                        route.DriverId = route.DriverId ?? string.Empty;
                        route.VehicleId = route.VehicleId ?? string.Empty;
                    }
                }

                return result ?? new List<RouteResultDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving routes: {ex.Message}");
                return new List<RouteResultDto>();
            }
        }

        public async Task<RouteResultDto> GetRouteByIdAsync(Guid routeId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<RouteResultDto>($"routeoptimization/route/{routeId}");
                return result ?? new RouteResultDto { Steps = new List<RouteStepDto>() };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving route by ID: {ex.Message}");
                return new RouteResultDto { Steps = new List<RouteStepDto>() };
            }
        }

        public async Task<List<RouteResultDto>> GetMyRoutesAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<RouteResultDto>>("routeoptimization/route/my");
                return result ?? new List<RouteResultDto>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving my routes: {ex.Message}");
                return new List<RouteResultDto>();
            }
        }

        public async Task<bool> CompleteRouteAsync(Guid routeId)
        {
            try
            {
                var response = await _httpClient.PutAsync($"routeoptimization/route/{routeId}/complete", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error completing route: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> VehicleHasActiveRoute(Guid vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<bool>($"routeoptimization/route/HasActiveRoute/{vehicleId}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error checking vehicle route status: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DriverHasActiveRoute(string driverId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<bool>($"routeoptimization/route/HasActiveRouteForDriver/{driverId}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error checking driver route status: {ex.Message}");
                return false;
            }
        }

        public async Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"routeoptimization/route/{routeId}/reoptimize", null);
                response.EnsureSuccessStatusCode();

                var updatedRoute = await GetRouteByIdAsync(routeId);
                return updatedRoute;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reoptimizing route: {ex.Message}");
                throw;
            }
        }

        public async Task<CO2StatsDto> GetCO2StatsAsync(int days)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<CO2StatsDto>($"routeoptimization/route/co2-stats?days={days}");
                return result ?? new CO2StatsDto { DailyStats = new List<DailyCO2Stat>() };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting CO2 stats: {ex.Message}");
                return new CO2StatsDto { DailyStats = new List<DailyCO2Stat>() };
            }
        }

        public async Task CompleteStepAsync(Guid stepId)
        {
            await _httpClient.PutAsync($"routeoptimization/route/steps/{stepId}/complete", null);
        }

        public async Task StartRouteSimulationAsync(Guid routeId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"routeoptimization/route/{routeId}/simulate", null);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting route simulation: {ex.Message}");
                throw;
            }
        }

        public async Task<SimulationStepResultDto> CompleteNextStepAsync(Guid routeId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"routeoptimization/route/{routeId}/complete-next-step", null);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<SimulationStepResultDto>();
                return result ?? new SimulationStepResultDto
                {
                    RouteId = routeId,
                    IsRouteCompleted = false,
                    TotalSteps = 0,
                    CompletedSteps = 0,
                    ProgressPercentage = 0
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error completing next step: {ex.Message}");
                throw;
            }
        }

        public async Task<int> SimulateAllRoutesAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("routeoptimization/route/simulate-all-routes", null);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);

                if (jsonDoc.RootElement.TryGetProperty("simulatedCount", out var countElement))
                {
                    return countElement.GetInt32();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error simulating all routes: {ex.Message}");
                return 0;
            }
        }
    }
}