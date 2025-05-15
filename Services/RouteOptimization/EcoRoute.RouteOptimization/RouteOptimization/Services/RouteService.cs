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

        // Constants for fuel and CO2 calculations
        private const double FUEL_CONSUMPTION_PER_KM = 0.15; // 100km'de 15L dizel tüketimi
        private const double CO2_PER_LITER_DIESEL = 2.68; // 1L dizel = 2.68 kg CO2

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
            }) ?? new List<WasteBinDto>());
        }

        public bool VehicleHasActiveRoute(string vehicleId)
        {
            return _context.RouteTasks.Any(r =>
                r.VehicleId == vehicleId &&
                r.Status != RouteStatus.Completed);
        }

        public bool DriverHasActiveRoute(string driverId)
        {
            return _context.RouteTasks.Any(r =>
                r.DriverId == driverId &&
                r.Status != RouteStatus.Completed);
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

        public async Task<RouteResultDto> GetRouteByIdAsync(Guid routeId)
        {
            var route = await _context.RouteTasks
                .Include(r => r.Steps.OrderBy(s => s.Order))
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null)
                throw new Exception("Rota bulunamadı.");

            if (route.Steps == null || !route.Steps.Any())
            {
                // Log this unexpected situation using Console instead of logger
                Console.Error.WriteLine($"Route {routeId} has no steps in database!");
            }

            return new RouteResultDto
            {
                Id = route.Id,
                DriverId = route.DriverId,
                VehicleId = route.VehicleId,
                WasteType = route.WasteType,
                OptimizationType = route.OptimizationType,
                StartTime = route.StartTime,
                TotalDistanceKm = route.TotalDistanceKm,
                EstimatedDurationMin = route.EstimatedDurationMin,
                Status = route.Status,
                OverviewPolyline = route.OverviewPolyline,
                EstimatedFuelL = route.EstimatedFuelL,
                EstimatedCO2Kg = route.EstimatedCO2Kg,
                Steps = route.Steps?.Select(s => new RouteStepDto
                {
                    Id = s.Id,
                    RouteTaskId = s.RouteTaskId,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Order = s.Order,
                    IsCompleted = s.IsCompleted,
                    WasteBinId = s.WasteBinId
                }).ToList() ?? new List<RouteStepDto>()
            };
        }
        public async Task CompleteRouteAsync(Guid routeId)
        {
            var route = await _context.RouteTasks
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null)
                throw new Exception("Rota bulunamadı.");

            if (route.Status == RouteStatus.Completed)
                throw new Exception("Bu rota zaten tamamlanmış.");

            // Store the original IDs before changing status
            var driverId = route.DriverId;
            var vehicleId = route.VehicleId;

            // Update route status
            route.Status = RouteStatus.Completed;

            // Ensure IDs are preserved
            route.DriverId = driverId ?? string.Empty;
            route.VehicleId = vehicleId ?? string.Empty;

            // Mark all steps as completed
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
                EstimatedFuelL = r.EstimatedFuelL,
                EstimatedCO2Kg = r.EstimatedCO2Kg,
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
                .AnyAsync(r => r.DriverId == dto.DriverId && r.Status != RouteStatus.Completed);

            if (driverBusy)
                throw new Exception("Bu sürücü zaten aktif bir rotada yer alıyor.");

            bool vehicleBusy = await _context.RouteTasks
                .AnyAsync(r => r.VehicleId == dto.VehicleId && r.Status != RouteStatus.Completed);

            if (vehicleBusy)
                throw new Exception("Bu araç şu anda başka bir rotada kullanılmakta.");

            // Save original waste bin order before any optimization
            var originalWasteBins = new List<WasteBinDto>(wasteBins);

            // Apply fill-level priority sorting if selected
            bool optimizeWaypoints = dto.OptimizationType == OptimizationType.EnKisaMesafe;

            if (dto.OptimizationType == OptimizationType.DolulukOncelikli)
            {
                wasteBins = wasteBins.OrderByDescending(b => b.FillLevel ?? 0).ToList();
                optimizeWaypoints = false; // Disable Google's optimization for fill-level priority
            }

            var usedBinIds = await _context.RouteSteps
                .Where(s => !s.IsCompleted)
                .Select(s => s.WasteBinId)
                .ToListAsync();

            bool binConflict = dto.WasteBinIds.Any(id => usedBinIds.Contains(id));
            if (binConflict)
                throw new Exception("Seçilen atık kutularından bazıları başka aktif rotalarda kullanılmakta.");

            // Fixed starting point 
            const double FIXED_START_LAT = 41.1634; // Çorlu
            const double FIXED_START_LNG = 27.7951;

            var origin = $"{FIXED_START_LAT},{FIXED_START_LNG}";
            var destination = origin; // Return to same point

            // Process waypoints (all waste bins)
            var waypoints = wasteBins.Select(b => $"{b.Latitude},{b.Longitude}").ToList();
            var waypointsParam = string.Join("|", waypoints);

            var optimizeParam = optimizeWaypoints ? "optimize:true|" : "";

            var apiKey = _googleMapsOptions.Value.ApiKey;
            var url = $"https://maps.googleapis.com/maps/api/directions/json?" +
                      $"origin={origin}&destination={destination}" +
                      $"&waypoints={optimizeParam}{waypointsParam}" +
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

            // Fix for issue #1: Make sure StartTime is not the default value
            var startTime = dto.StartTime;
            if (startTime == DateTime.MinValue)
            {
                startTime = DateTime.UtcNow;
            }

            // Create RouteTask entity
            var routeTask = new RouteTask
            {
                Id = Guid.NewGuid(),
                DriverId = dto.DriverId ?? string.Empty, // Ensure not null
                VehicleId = dto.VehicleId ?? string.Empty, // Ensure not null
                WasteType = dto.WasteType,
                OptimizationType = dto.OptimizationType,
                StartTime = startTime,
                Status = RouteStatus.Scheduled,
                TotalDistanceKm = Math.Round(totalDistance / 1000, 2),
                EstimatedFuelL = Math.Round(Math.Round(totalDistance / 1000, 2) * FUEL_CONSUMPTION_PER_KM, 2),
                EstimatedCO2Kg = Math.Round(Math.Round(totalDistance / 1000, 2) * FUEL_CONSUMPTION_PER_KM * CO2_PER_LITER_DIESEL, 2),
                EstimatedDurationMin = totalDuration / 60,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                OverviewPolyline = overviewPolyline // Ensure this is stored
            };

            // Create ordered waste bins list based on optimization method
            var orderedWasteBins = new List<WasteBinDto>();

            if (optimizeWaypoints)
            {
                // Get Google's waypoint order
                var waypointOrder = route.GetProperty("waypoint_order").EnumerateArray().Select(x => x.GetInt32()).ToList();

                // Add all points according to optimization
                foreach (var index in waypointOrder)
                {
                    if (index < wasteBins.Count)
                        orderedWasteBins.Add(wasteBins[index]);
                }
            }
            else
            {
                // For fill-level priority, use our pre-sorted list
                orderedWasteBins = wasteBins;
            }

            // Create route steps with waste bin IDs
            var stepEntities = new List<RouteStep>();
            for (int i = 0; i < steps.Count && i < orderedWasteBins.Count; i++)
            {
                var s = steps[i];
                stepEntities.Add(new RouteStep
                {
                    Id = Guid.NewGuid(),
                    RouteTaskId = routeTask.Id,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Order = s.Order,
                    IsCompleted = false,
                    WasteBinId = orderedWasteBins[i].Id
                });
            }

            // Save
            await _context.RouteTasks.AddAsync(routeTask);
            await _context.RouteSteps.AddRangeAsync(stepEntities);
            await _context.SaveChangesAsync();

            // Return DTO with all needed information
            return new RouteResultDto
            {
                Id = routeTask.Id,
                DriverId = routeTask.DriverId,
                VehicleId = routeTask.VehicleId,
                WasteType = routeTask.WasteType,
                OptimizationType = routeTask.OptimizationType,
                StartTime = routeTask.StartTime,
                Status = routeTask.Status,
                OverviewPolyline = routeTask.OverviewPolyline, // Include polyline for map rendering
                TotalDistanceKm = routeTask.TotalDistanceKm,
                EstimatedDurationMin = routeTask.EstimatedDurationMin,
                EstimatedFuelL = routeTask.EstimatedFuelL,
                EstimatedCO2Kg = routeTask.EstimatedCO2Kg,
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
                EstimatedFuelL = r.EstimatedFuelL,
                EstimatedCO2Kg = r.EstimatedCO2Kg,
                OverviewPolyline = r.OverviewPolyline,
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

        public async Task<RouteResultDto> ReoptimizeRouteWithTrafficAsync(Guid routeId)
        {
            var route = await _context.RouteTasks
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null)
                throw new Exception("Rota bulunamadı.");

            if (route.Status == RouteStatus.Completed)
                throw new Exception("Tamamlanmış rotalar yeniden optimize edilemez.");

            var remainingSteps = route.Steps.Where(s => !s.IsCompleted).ToList();
            var completedSteps = route.Steps.Where(s => s.IsCompleted).ToList();

            Console.WriteLine($"[REOPTIMIZE] routeId: {routeId}");
            Console.WriteLine($"[REOPTIMIZE] completedSteps: {completedSteps.Count}, remainingSteps: {remainingSteps.Count}");

            if (!remainingSteps.Any())
                throw new Exception("Tüm adımlar tamamlanmış, yeniden optimizasyon gerekmez.");

            var wasteBinIds = remainingSteps.Select(s => s.WasteBinId).ToList();
            var wasteBins = await GetWasteBinsByIdsAsync(wasteBinIds);

            var lastCompletedStep = completedSteps.OrderByDescending(s => s.Order).FirstOrDefault();

            double startLat = lastCompletedStep?.Latitude ?? 41.1634;
            double startLng = lastCompletedStep?.Longitude ?? 27.7951;

            var origin = $"{startLat},{startLng}";
            var destination = origin;

            var waypoints = wasteBins.Select(b => $"{b.Latitude},{b.Longitude}").ToList();
            var waypointsParam = string.Join("|", waypoints);

            bool optimizeWaypoints = route.OptimizationType == OptimizationType.EnKisaMesafe;
            var optimizeParam = optimizeWaypoints ? "optimize:true|" : "";

            var apiKey = _googleMapsOptions.Value.ApiKey;
            var url = $"https://maps.googleapis.com/maps/api/directions/json?" +
                      $"origin={origin}&destination={destination}" +
                      $"&waypoints={optimizeParam}{waypointsParam}" +
                      $"&departure_time=now&traffic_model=best_guess&key={apiKey}";

            Console.WriteLine($"[GOOGLE MAPS] Request URL: {url}");

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Google Maps API'dan rota alınamadı.");

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var routes = json.RootElement.GetProperty("routes");
            if (routes.GetArrayLength() == 0)
                throw new Exception("Google Maps uygun bir rota döndüremedi.");

            var googleRoute = routes[0];
            var legs = googleRoute.GetProperty("legs");
            Console.WriteLine($"[GOOGLE MAPS] Legs count: {legs.GetArrayLength()}");

            var overviewPolyline = googleRoute.GetProperty("overview_polyline").GetProperty("points").GetString();

            double totalDistance = 0;
            int totalDuration = 0;
            foreach (var leg in legs.EnumerateArray())
            {
                totalDistance += leg.GetProperty("distance").GetProperty("value").GetDouble();
                totalDuration += leg.TryGetProperty("duration_in_traffic", out var durationInTraffic)
                    ? durationInTraffic.GetProperty("value").GetInt32()
                    : leg.GetProperty("duration").GetProperty("value").GetInt32();
            }

            var orderedWasteBins = new List<WasteBinDto>();
            if (optimizeWaypoints && googleRoute.TryGetProperty("waypoint_order", out var waypointOrderElement))
            {
                var waypointOrder = waypointOrderElement.EnumerateArray().Select(x => x.GetInt32()).ToList();
                foreach (var index in waypointOrder)
                {
                    if (index < wasteBins.Count)
                        orderedWasteBins.Add(wasteBins[index]);
                }
            }
            else
            {
                orderedWasteBins = wasteBins.ToList();
            }

            Console.WriteLine($"[REOPTIMIZE] orderedWasteBins.Count: {orderedWasteBins.Count}");

            _context.RouteSteps.RemoveRange(remainingSteps);

            var newSteps = new List<RouteStep>();
            int order = completedSteps.Count;
            int stepIndex = 0;

            foreach (var leg in legs.EnumerateArray())
            {
                if (stepIndex >= orderedWasteBins.Count)
                    break;

                var bin = orderedWasteBins[stepIndex++];
                var endLocation = leg.GetProperty("end_location");
                var address = leg.GetProperty("end_address").GetString();
                double lat = endLocation.GetProperty("lat").GetDouble();
                double lng = endLocation.GetProperty("lng").GetDouble();

                var newStep = new RouteStep
                {
                    Id = Guid.NewGuid(),
                    RouteTaskId = route.Id,
                    Address = address ?? bin.Address,
                    Latitude = lat,
                    Longitude = lng,
                    Order = order++,
                    IsCompleted = false,
                    WasteBinId = bin.Id
                };

                Console.WriteLine($"[NEW STEP] WasteBinId={newStep.WasteBinId}, Lat={lat}, Lng={lng}, Order={newStep.Order}");
                newSteps.Add(newStep);
            }

            if (newSteps.Count == 0 && orderedWasteBins.Any())
            {
                foreach (var bin in orderedWasteBins)
                {
                    var newStep = new RouteStep
                    {
                        Id = Guid.NewGuid(),
                        RouteTaskId = route.Id,
                        Address = bin.Address,
                        Latitude = bin.Latitude,
                        Longitude = bin.Longitude,
                        Order = order++,
                        IsCompleted = false,
                        WasteBinId = bin.Id
                    };
                    Console.WriteLine($"[FALLBACK STEP] WasteBinId={newStep.WasteBinId}");
                    newSteps.Add(newStep);
                }
            }

            Console.WriteLine($"[REOPTIMIZE] newSteps.Count: {newSteps.Count}");

            try
            {
                await _context.RouteSteps.AddRangeAsync(newSteps);
                route.TotalDistanceKm = Math.Round(totalDistance / 1000, 2);
                route.EstimatedDurationMin = totalDuration / 60;
                route.OverviewPolyline = overviewPolyline;
                route.EstimatedFuelL = Math.Round(route.TotalDistanceKm * FUEL_CONSUMPTION_PER_KM, 2);
                route.EstimatedCO2Kg = Math.Round(route.EstimatedFuelL * CO2_PER_LITER_DIESEL, 2);

                await _context.SaveChangesAsync();
                Console.WriteLine("[SAVE] Yeni rota adımları başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ERROR] SaveChanges failed: {ex.Message}");
                throw;
            }

            var updatedRoute = await _context.RouteTasks
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (updatedRoute == null || updatedRoute.Steps.Count == 0)
                Console.Error.WriteLine("[ERROR] Route adımları boş görünüyor!");

            return new RouteResultDto
            {
                Id = updatedRoute.Id,
                DriverId = updatedRoute.DriverId,
                VehicleId = updatedRoute.VehicleId,
                WasteType = updatedRoute.WasteType,
                OptimizationType = updatedRoute.OptimizationType,
                StartTime = updatedRoute.StartTime,
                TotalDistanceKm = updatedRoute.TotalDistanceKm,
                EstimatedDurationMin = updatedRoute.EstimatedDurationMin,
                Status = updatedRoute.Status,
                OverviewPolyline = updatedRoute.OverviewPolyline,
                EstimatedFuelL = updatedRoute.EstimatedFuelL,
                EstimatedCO2Kg = updatedRoute.EstimatedCO2Kg,
                Steps = updatedRoute.Steps
                    .OrderBy(s => s.Order)
                    .Select(s => new RouteStepDto
                    {
                        Id = s.Id,
                        RouteTaskId = s.RouteTaskId,
                        Address = s.Address,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude,
                        Order = s.Order,
                        IsCompleted = s.IsCompleted,
                        WasteBinId = s.WasteBinId
                    })
                    .ToList()
            };
        }


        public async Task<CO2StatsDto> GetCO2StatsAsync(int days)
        {
            // Bitiş tarihi olarak bugünü al
            var endDate = DateTime.UtcNow.Date;
            // Başlangıç tarihi olarak days gün öncesini al
            var startDate = endDate.AddDays(-days);

            // Belirtilen tarih aralığındaki tamamlanmış rotaları al
            var routes = await _context.RouteTasks
                .Where(r => r.Status == RouteStatus.Completed && r.StartTime.Date >= startDate && r.StartTime.Date <= endDate)
                .ToListAsync();

            // Günlük CO2 tasarruflarını hesapla
            var dailyStats = new List<DailyCO2Stat>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dayRoutes = routes.Where(r => r.StartTime.Date == date).ToList();
                var co2Total = dayRoutes.Sum(r => r.EstimatedCO2Kg);

                dailyStats.Add(new DailyCO2Stat
                {
                    Date = date,
                    CO2Kg = co2Total,
                    RouteCount = dayRoutes.Count
                });
            }

            // Genel istatistikleri hesapla
            var totalCO2 = routes.Sum(r => r.EstimatedCO2Kg);
            var totalRoutes = routes.Count;
            var totalDistance = routes.Sum(r => r.TotalDistanceKm);

            return new CO2StatsDto
            {
                TotalCO2Kg = Math.Round(totalCO2, 2),
                TotalRoutes = totalRoutes,
                TotalDistanceKm = Math.Round(totalDistance, 2),
                AverageCO2PerRouteKg = totalRoutes > 0 ? Math.Round(totalCO2 / totalRoutes, 2) : 0,
                DailyStats = dailyStats
            };
        }
    }
}