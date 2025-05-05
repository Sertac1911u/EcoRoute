using EcoRoute.Settings.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.Supports.Context
{
    public class SettingsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1910; initial Catalog=EcoRouteSettingsDb;User=sa; Password=Sertac1911s*;TrustServerCertificate= true;");
        }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<ThemeColor> ThemeColors { get; set; }
        public DbSet<Avatar> Avatars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemSetting>()
               .Property(s => s.UserId)
               .IsRequired(false);
            modelBuilder.Entity<ThemeColor>().HasData(
      new ThemeColor { Id = Guid.NewGuid(), Name = "Orijinal", Value = "#2ba86d" }, // EcoRoute orijinal rengi
      new ThemeColor { Id = Guid.NewGuid(), Name = "Mavi", Value = "#3B82F6" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Kırmızı", Value = "#EF4444" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Mor", Value = "#8B5CF6" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Pembe", Value = "#EC4899" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Sarı", Value = "#F59E0B" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Turkuaz", Value = "#06B6D4" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Indigo", Value = "#6366F1" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Lime", Value = "#84CC16" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Gri", Value = "#6B7280" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Turuncu", Value = "#F97316" },
      new ThemeColor { Id = Guid.NewGuid(), Name = "Teal", Value = "#14B8A6" }
  );

            // Avatar için seed data
            modelBuilder.Entity<Avatar>().HasData(
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 1", Url = "https://api.dicebear.com/9.x/adventurer/svg?seed=Luis" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 2", Url = "https://api.dicebear.com/9.x/adventurer/svg?seed=Avery" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 3", Url = "https://api.dicebear.com/9.x/adventurer/svg?seed=Jude" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 4", Url = "https://api.dicebear.com/9.x/adventurer/svg?seed=Jack" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 5", Url = "https://api.dicebear.com/9.x/adventurer/svg?seed=Jocelyn" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 6", Url = "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton" }
            );

            var defaultSettingId = Guid.NewGuid();
            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting
                {
                    Id = defaultSettingId,
                    DarkMode = false,
                    ThemeColor = "#2ba86d", // Orijinal yeşil renk
                    FontSize = 14,
                    EnableAnimations = true,
                    AvatarUrl = "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton", // varsayılan avatar
                    EmailNotifications = true,
                    SmsNotifications = false,
                    PushNotifications = true,
                    GoogleMapsApiKey = ""
                }
            );

        }
    }
}
