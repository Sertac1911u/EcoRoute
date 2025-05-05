using EcoRoute.Notifications.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EcoRoute.Notifications.Context
{
    public class NotificationsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1912; initial Catalog=EcoRouteNotificationsDb;User=sa; Password=Sertac1911s*;TrustServerCertificate= true;");
        }

        public DbSet<Notification> Notifications { get; set; }

    }
}
