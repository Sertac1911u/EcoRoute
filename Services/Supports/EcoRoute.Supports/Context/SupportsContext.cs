using EcoRoute.Supports.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.Supports.Context
{
    public class SupportsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1909; initial Catalog=EcoRouteSupportsDb;User=sa; Password=Sertac1911s*;TrustServerCertificate= true;");
        }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }
    }
}
