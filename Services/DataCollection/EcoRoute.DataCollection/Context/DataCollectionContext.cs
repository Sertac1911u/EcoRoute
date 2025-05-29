using EcoRoute.DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataCollection.Context
{
    public class DataCollectionContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1905; initial Catalog=EcoRouteDataCollectionDb;User=sa; Password=Sertac1911s*;TrustServerCertificate= true;");
        }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<WasteBin> WasteBins { get; set; }
    }
}
