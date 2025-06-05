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

            SeedVehicles(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedVehicles(ModelBuilder modelBuilder)
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Plate = "59 ABC 1001", Description = "1. Araç" },
                new Vehicle { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Plate = "59 DEF 1002", Description = "2. Araç" },
                new Vehicle { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Plate = "59 GHI 1003", Description = "3. Araç" },
                new Vehicle { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Plate = "59 JKL 1004", Description = "4. Araç" },
                new Vehicle { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Plate = "59 MNO 1005", Description = "5. Araç" },
                new Vehicle { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Plate = "59 PQR 1006", Description = "6. Araç" },
                new Vehicle { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Plate = "59 STU 1007", Description = "7. Araç" },
                new Vehicle { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Plate = "59 VWX 1008", Description = "8. Araç" },
                new Vehicle { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Plate = "59 YZA 1009", Description = "9. Araç" },
                new Vehicle { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Plate = "59 BCD 1010", Description = "10. Araç" }
            };

            modelBuilder.Entity<Vehicle>().HasData(vehicles);
        }
    }
}