using EcoRoute.RouteOptimization.Context;
using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EcoRoute.RouteOptimization.Services
{
    public class RouteService : IRouteService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<GoogleMapsOptions> _googleMapsOptions;
        private readonly RouteDbContext _context;

        public RouteService(IHttpClientFactory httpClientFactory, IOptions<GoogleMapsOptions> googleMapsOptions, RouteDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _googleMapsOptions = googleMapsOptions;
            _context = context;
        }
        public async Task<List<WasteBinDto>> GetWasteBinsByIdsAsync(List<Guid> wasteBinIds)
        {
            var client = _httpClientFactory.CreateClient("DataCollectionClient");

            var query = string.Join("&", wasteBinIds.Select(id => $"id={id}"));
            var response = await client.GetAsync($"/api/wastebins/selected?{query}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("WasteBin verileri alınamadı.");

            var content = await response.Content.ReadAsStringAsync();

            return await Task.Run(() =>
            JsonSerializer.Deserialize<List<WasteBinDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }));

        }
        public bool VehicleHasActiveRoute(string vehicleId)
        {
            return _context.RouteTasks.Any(r =>
                r.VehicleId == vehicleId &&
                r.Status != "Completed");
        }

        public bool DriverHasActiveRoute(string driverId)
        {
            return _context.RouteTasks.Any(r =>
                r.DriverId == driverId &&
                r.Status != "Completed");
        }
        public async Task<RouteLiveStatusDto> GetLiveStatusAsync(Guid routeId)
        {
            var route = await _context.RouteTasks
                .Include(r => r.Steps.OrderBy(s => s.Order))
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null)
                throw new Exception("Rota bulunamadı.");

            var steps = route.Steps.ToList();
            int totalSteps = steps.Count;
            int completedSteps = steps.Count(s => s.IsCompleted);

            int progress = (int)((double)completedSteps / Math.Max(1, totalSteps) * 100);
            var currentStep = steps.FirstOrDefault(s => !s.IsCompleted) ?? steps.Last();

            return new RouteLiveStatusDto
            {
                TotalSteps = totalSteps,
                CompletedSteps = completedSteps,
                ProgressPercentage = progress,
                CurrentAddress = currentStep.Address,
                Latitude = currentStep.Latitude,
                Longitude = currentStep.Longitude,
                EstimatedTimeRemainingMin = route.EstimatedDurationMin - (completedSteps * 5), 
                ElapsedTimeMin = completedSteps * 5 
            };
        }

        public async Task CompleteRouteAsync(Guid routeId)
        {
            var route = await _context.RouteTasks
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null)
                throw new Exception("Rota bulunamadı.");

            if (route.Status == "Completed")
                throw new Exception("Bu rota zaten tamamlanmış.");

            
            route.Status = "Completed";

            foreach (var step in route.Steps)
            {
                step.IsCompleted = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<RouteResultDto>> GetAllRoutesAsync(string? driverId = null)
        {
            var query = _context.RouteTasks
                .Include(r => r.Steps)
                .AsQueryable();

            if (!string.IsNullOrEmpty(driverId))
                query = query.Where(r => r.DriverId == driverId);

            var routes = await query.ToListAsync();

            return routes.Select(r => new RouteResultDto
            {
                Id = r.Id,
                DriverId = r.DriverId,
                VehicleId = r.VehicleId,
                WasteType = r.WasteType,
                OptimizationType = r.OptimizationType,
                StartTime = r.StartTime,
                TotalDistanceKm = r.TotalDistanceKm,
                EstimatedDurationMin = r.EstimatedDurationMin,
                OverviewPolyline = r.OverviewPolyline, 
                Status = r.Status,
                Steps = r.Steps.Select(s => new RouteStepDto
                {
                    Id = s.Id,
                    RouteTaskId = s.RouteTaskId,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Order = s.Order,
                    IsCompleted = s.IsCompleted,
                    WasteBinId = s.WasteBinId
                }).ToList()
            }).ToList();
        }

        public async Task<RouteResultDto> CreateOptimizedRouteAsync(CreateRouteDto dto)
        {
            var wasteBins = await GetWasteBinsByIdsAsync(dto.WasteBinIds);
            if (wasteBins.Count < 2)
                throw new Exception("En az 2 atık kutusu seçilmelidir.");

            bool driverBusy = await _context.RouteTasks
                .AnyAsync(r => r.DriverId == dto.DriverId && r.Status != "Completed");
            if (driverBusy)
                throw new Exception("Bu sürücü zaten aktif bir rotada yer alıyor.");

            bool vehicleBusy = await _context.RouteTasks
                .AnyAsync(r => r.VehicleId == dto.VehicleId && r.Status != "Completed");
            if (vehicleBusy)
                throw new Exception("Bu araç şu anda başka bir rotada kullanılmakta.");


            var usedBinIds = await _context.RouteSteps
                .Where(s => !s.IsCompleted)
                .Select(s => s.WasteBinId)
                .ToListAsync();

            bool binConflict = dto.WasteBinIds.Any(id => usedBinIds.Contains(id));
            if (binConflict)
                throw new Exception("Seçilen atık kutularından bazıları başka aktif rotalarda kullanılmakta.");

            var origin = $"{wasteBins[0].Latitude},{wasteBins[0].Longitude}";
            var destination = origin;

            var waypoints = wasteBins.Skip(1)
                .Select(b => $"{b.Latitude},{b.Longitude}")
                .ToList();
            var waypointsParam = string.Join("|", waypoints);

            var apiKey = _googleMapsOptions.Value.ApiKey;
            var url = $"https://maps.googleapis.com/maps/api/directions/json?" +
                      $"origin={origin}&destination={destination}" +
                      $"&waypoints=optimize:true|{waypointsParam}" +
                      $"&key={apiKey}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Google Maps API'dan rota alınamadı.");

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var routes = json.RootElement.GetProperty("routes");
            if (routes.GetArrayLength() == 0)
                throw new Exception("Google Maps uygun bir rota döndüremedi.");

            var route = routes[0];
            var overviewPolyline = route.GetProperty("overview_polyline").GetProperty("points").GetString();

            var legs = route.GetProperty("legs");

            double totalDistance = 0;
            int totalDuration = 0;
            var steps = new List<RouteStepDto>();
            int order = 0;

            foreach (var leg in legs.EnumerateArray())
            {
                var endLocation = leg.GetProperty("end_location");
                var address = leg.GetProperty("end_address").GetString();
                double lat = endLocation.GetProperty("lat").GetDouble();
                double lng = endLocation.GetProperty("lng").GetDouble();

                totalDistance += leg.GetProperty("distance").GetProperty("value").GetDouble(); // metre
                totalDuration += leg.GetProperty("duration").GetProperty("value").GetInt32();  // saniye

                steps.Add(new RouteStepDto
                {
                    Address = address,
                    Latitude = lat,
                    Longitude = lng,
                    Order = order++
                });
            }

            // 6. RouteTask entity’si oluştur
            var routeTask = new RouteTask
            {
                Id = Guid.NewGuid(),
                DriverId = dto.DriverId,
                VehicleId = dto.VehicleId,
                WasteType = dto.WasteType,
                OptimizationType = dto.OptimizationType,
                StartTime = dto.StartTime,
                Status = "Scheduled",
                TotalDistanceKm = Math.Round(totalDistance / 1000, 2),
                EstimatedDurationMin = totalDuration / 60,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                OverviewPolyline = overviewPolyline 
            };

            var waypointOrder = route.GetProperty("waypoint_order").EnumerateArray().Select(x => x.GetInt32()).ToList();

            var orderedWasteBins = new List<WasteBinDto>();
            orderedWasteBins.Add(wasteBins[0]); 

            foreach (var index in waypointOrder)
            {
                orderedWasteBins.Add(wasteBins[index + 1]);     
            }
            var stepEntities = steps.Select((s, index) => new RouteStep
            {
                Id = Guid.NewGuid(),
                RouteTaskId = routeTask.Id,
                Address = s.Address,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Order = s.Order,
                IsCompleted = false,
                WasteBinId = orderedWasteBins[index].Id // 🔒 Artık güvenli
            }).ToList();

            // 8. Kaydet
            await _context.RouteTasks.AddAsync(routeTask);
            await _context.RouteSteps.AddRangeAsync(stepEntities);
            await _context.SaveChangesAsync();

            // 9. DTO olarak dön
            return new RouteResultDto
            {
                Id = routeTask.Id,
                DriverId = routeTask.DriverId,
                VehicleId = routeTask.VehicleId,
                WasteType = routeTask.WasteType,
                OptimizationType = routeTask.OptimizationType,
                StartTime = routeTask.StartTime,
                Status = routeTask.Status,
                OverviewPolyline = routeTask.OverviewPolyline,
                TotalDistanceKm = routeTask.TotalDistanceKm,
                EstimatedDurationMin = routeTask.EstimatedDurationMin,
                Steps = stepEntities.Select(s => new RouteStepDto
                {
                    Id = s.Id,
                    RouteTaskId = s.RouteTaskId,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Order = s.Order,
                    IsCompleted = s.IsCompleted,
                    WasteBinId = s.WasteBinId
                }).ToList()

            };
        }
        public async Task<List<RouteResultDto>> GetRoutesByRoleAsync(string role, string userId)
        {
            var query = _context.RouteTasks
                .Include(r => r.Steps)
                .AsQueryable();

            if (role == "Driver")
            {
                query = query.Where(r => r.DriverId == userId);
            }

            var routes = await query.ToListAsync();

            return routes.Select(r => new RouteResultDto
            {
                Id = r.Id,
                DriverId = r.DriverId,
                VehicleId = r.VehicleId,
                WasteType = r.WasteType,
                OptimizationType = r.OptimizationType,
                StartTime = r.StartTime,
                Status = r.Status,
                TotalDistanceKm = r.TotalDistanceKm,
                EstimatedDurationMin = r.EstimatedDurationMin,
                Steps = r.Steps.Select(s => new RouteStepDto
                {
                    Id = s.Id,
                    RouteTaskId = s.RouteTaskId,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Order = s.Order,
                    IsCompleted = s.IsCompleted
                }).ToList()
            }).ToList();
        }
    }
}
