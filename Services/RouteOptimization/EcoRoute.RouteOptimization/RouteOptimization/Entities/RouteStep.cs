﻿using System.ComponentModel.DataAnnotations;

namespace EcoRoute.RouteOptimization.Entities
{
    public class RouteStep
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RouteTaskId { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsCompleted { get; set; } = false;

        public Guid? WasteBinId { get; set; }

        public RouteTask RouteTask { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsDepotStep => !WasteBinId.HasValue;
        public bool IsWasteBinStep => WasteBinId.HasValue;

        public string StepType => Order switch
        {
            0 => "START",
            _ when IsDepotStep => "END",
            _ when IsWasteBinStep => "WASTE_BIN",
            _ => "UNKNOWN"
        };

        public override string ToString()
        {
            return $"RouteStep(Order={Order}, Type={StepType}, Address='{Address}', WasteBinId={WasteBinId}, Completed={IsCompleted})";
        }
    }
}
