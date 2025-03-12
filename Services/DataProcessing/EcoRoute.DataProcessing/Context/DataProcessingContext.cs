using EcoRoute.DataProcessing.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.DataProcessing.Context
{
    public class DataProcessingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1906; initial Catalog=EcoRouteDataProcessingDb;User=sa; Password=Sertac1911s*;TrustServerCertificate= true;");
        }
        public DbSet<DataProcessingLog> dataProcessingLogs { get; set; }
        public DbSet<ProcessedData> processedDatas { get; set; }
    }
}
