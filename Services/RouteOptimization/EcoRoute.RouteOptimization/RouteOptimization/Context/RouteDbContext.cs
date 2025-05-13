using EcoRoute.RouteOptimization.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.RouteOptimization.Context
{
    public class RouteDbContext : DbContext
    {
        public RouteDbContext(DbContextOptions<RouteDbContext> options) : base(options) { }

        public DbSet<RouteTask> RouteTasks { get; set; }
        public DbSet<RouteStep> RouteSteps { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RouteStep>()
                .HasOne(rs => rs.RouteTask)
                .WithMany(rt => rt.Steps)
                .HasForeignKey(rs => rs.RouteTaskId)
                .OnDelete(DeleteBehavior.Restrict);
        
            base.OnModelCreating(modelBuilder);

        }
    }
}
