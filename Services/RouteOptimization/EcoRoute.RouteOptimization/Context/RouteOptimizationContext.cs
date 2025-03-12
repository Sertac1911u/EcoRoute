using EcoRoute.RouteOptimization.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.RouteOptimization.Context
{
    public class RouteOptimizationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1907; initial Catalog=EcoRouteRouteOptimizationDb;User=sa; Password=Sertac1911s*;TrustServerCertificate= true;");
        }

        public DbSet<MyRoute> Routes { get; set; }
        public DbSet<RouteOptimizationResult> Results { get; set; }
        public DbSet<Waypoint> Waypoints { get; set; }
    }
}
