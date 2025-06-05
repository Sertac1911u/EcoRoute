using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.RouteOptimizationDtos
{
    public class RouteStepDto
    {
        public Guid Id { get; set; }
        public Guid RouteTaskId { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }

        public Guid? WasteBinId { get; set; }

        public bool IsDepotStep => !WasteBinId.HasValue;
        public bool IsWasteBinStep => WasteBinId.HasValue;
        public string StepType => Order switch
        {
            0 => "START",
            _ when IsDepotStep => "END",
            _ when IsWasteBinStep => "WASTE_BIN",
            _ => "UNKNOWN"
        };
    }

    public class RouteResultDto
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public WasteType WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public RouteStatus Status { get; set; }
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }
        public string? OverviewPolyline { get; set; }
        public string? RouteName { get; set; }
        public string? Notes { get; set; }
        public List<RouteStepDto> Steps { get; set; } = new List<RouteStepDto>();

        public int TotalSteps => Steps?.Count ?? 0;
        public int CompletedSteps => Steps?.Count(s => s.IsCompleted) ?? 0;
        public int RemainingSteps => TotalSteps - CompletedSteps;
        public double ProgressPercentage => TotalSteps > 0 ? (CompletedSteps / (double)TotalSteps) * 100 : 0;


    }
    public enum RouteStatus
    {
        Scheduled = 0,
        Active = 1,
        Completed = 2,
        Cancelled = 3
    }

    public enum WasteType
    {
        Cop = 0,
        GeriDonusum = 1,
        Organik = 2,
        Cam = 3,
        Metal = 4,
        Elektronik = 5,
        Tehlikeli = 6
    }

    public enum OptimizationType
    {
        EnKisaMesafe = 0,
        DolulukOncelikli = 1
    }
}
