using System.ComponentModel.DataAnnotations;

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

        // *** DÜZELTME: WasteBinId nullable yapıldı ***
        // Başlangıç ve bitiş noktaları için null olacak
        public Guid? WasteBinId { get; set; }

        // Navigation properties
        public RouteTask RouteTask { get; set; } = null!;

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Helper methods
        public bool IsDepotStep => !WasteBinId.HasValue;
        public bool IsWasteBinStep => WasteBinId.HasValue;

        public string StepType => Order switch
        {
            0 => "START",
            _ when IsDepotStep => "END",
            _ when IsWasteBinStep => "WASTE_BIN",
            _ => "UNKNOWN"
        };

        // For debugging and logging
        public override string ToString()
        {
            return $"RouteStep(Order={Order}, Type={StepType}, Address='{Address}', WasteBinId={WasteBinId}, Completed={IsCompleted})";
        }
    }
}
