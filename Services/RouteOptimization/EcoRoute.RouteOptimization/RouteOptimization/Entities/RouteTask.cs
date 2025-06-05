using System.ComponentModel.DataAnnotations;

namespace EcoRoute.RouteOptimization.Entities
{
    public class RouteTask
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(450)]
        public string DriverId { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        public WasteType WasteType { get; set; }

        [Required]
        public OptimizationType OptimizationType { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public RouteStatus Status { get; set; } = RouteStatus.Scheduled;

        public double TotalDistanceKm { get; set; }
        public int EstimatedDurationMin { get; set; }
        public double EstimatedFuelL { get; set; }
        public double EstimatedCO2Kg { get; set; }

        [StringLength(10000)]
        public string? OverviewPolyline { get; set; }

        [StringLength(200)]
        public string? RouteName { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public ICollection<RouteStep> Steps { get; set; } = new List<RouteStep>();

        public int TotalSteps => Steps?.Count ?? 0;
        public int CompletedSteps => Steps?.Count(s => s.IsCompleted) ?? 0;
        public int RemainingSteps => TotalSteps - CompletedSteps;
        public double ProgressPercentage => TotalSteps > 0 ? (CompletedSteps / (double)TotalSteps) * 100 : 0;

        public bool IsCompleted => Status == RouteStatus.Completed;
        public bool IsActive => Status == RouteStatus.Active;
        public bool IsScheduled => Status == RouteStatus.Scheduled;

        public void MarkAsCompleted()
        {
            Status = RouteStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            foreach (var step in Steps)
            {
                step.IsCompleted = true;
                step.UpdatedAt = DateTime.UtcNow;
            }
        }

        public void MarkAsActive()
        {
            if (Status == RouteStatus.Scheduled)
            {
                Status = RouteStatus.Active;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public RouteStep? GetNextStep()
        {
            return Steps?
                .Where(s => !s.IsCompleted)
                .OrderBy(s => s.Order)
                .FirstOrDefault();
        }

        public RouteStep? GetCurrentStep()
        {
            var nextStep = GetNextStep();
            if (nextStep == null)
                return Steps?.OrderByDescending(s => s.Order).FirstOrDefault();

            return nextStep;
        }

        public List<RouteStep> GetCompletedSteps()
        {
            return Steps?
                .Where(s => s.IsCompleted)
                .OrderBy(s => s.Order)
                .ToList() ?? new List<RouteStep>();
        }

        public List<RouteStep> GetRemainingSteps()
        {
            return Steps?
                .Where(s => !s.IsCompleted)
                .OrderBy(s => s.Order)
                .ToList() ?? new List<RouteStep>();
        }

        public override string ToString()
        {
            return $"RouteTask(Id={Id}, Status={Status}, Steps={TotalSteps}, Progress={ProgressPercentage:F1}%, Driver={DriverId}, Vehicle={VehicleId})";
        }
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