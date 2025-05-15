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
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public Guid WasteBinId { get; set; } 
    }

    public class RouteResultDto
    {
        public Guid Id { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public string WasteType { get; set; }
        public OptimizationType OptimizationType { get; set; }
        public DateTime StartTime { get; set; }
        public RouteStatus Status { get; set; }
        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public List<RouteStepDto> Steps { get; set; }
        public string? OverviewPolyline { get; set; }
        public double EstimatedFuelL { get; set; } // Yeni eklenen
        public double EstimatedCO2Kg { get; set; } // Yeni eklenen


    }
    public enum RouteStatus
    {
        Scheduled,
        Active,
        Completed
    }

    public enum OptimizationType
    {
        EnKisaMesafe,
        DolulukOncelikli
    }
}
