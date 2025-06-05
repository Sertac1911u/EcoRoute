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
        public DbSet<FontType> FontTypes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<DateFormat> DateFormats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var font1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var font2Id = Guid.Parse("11111111-1111-1111-1111-111111111112");
            var font3Id = Guid.Parse("11111111-1111-1111-1111-111111111113");
            var font4Id = Guid.Parse("11111111-1111-1111-1111-111111111114");

            var langTrId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var langEnId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            var langDeId = Guid.Parse("22222222-2222-2222-2222-222222222224");

            var fmt1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var fmt2Id = Guid.Parse("33333333-3333-3333-3333-333333333334");
            var fmt3Id = Guid.Parse("33333333-3333-3333-3333-333333333335");

            // FONT TYPES
            modelBuilder.Entity<FontType>().HasData(
                new FontType { Id = font1Id, Name = "Open Sans", CssValue = "'Open Sans', sans-serif" },
                new FontType { Id = font2Id, Name = "Georgia", CssValue = "Georgia, serif" },
                new FontType { Id = font3Id, Name = "Montserrat", CssValue = "'Montserrat', sans-serif" },
                new FontType { Id = font4Id, Name = "Playfair Display", CssValue = "'Playfair Display', serif" }
            );

            // LANGUAGES
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = langTrId, Name = "Türkçe", CultureCode = "tr-TR" },
                new Language { Id = langEnId, Name = "English", CultureCode = "en-US" },
                new Language { Id = langDeId, Name = "Deutsch", CultureCode = "de-DE" }
            );

            // DATE FORMATS
            modelBuilder.Entity<DateFormat>().HasData(
                new DateFormat { Id = fmt1Id, FormatString = "DD/MM/YYYY", Sample = "31/12/2025" },
                new DateFormat { Id = fmt2Id, FormatString = "MM/DD/YYYY", Sample = "12/31/2025" },
                new DateFormat { Id = fmt3Id, FormatString = "YYYY-MM-DD", Sample = "2025-12-31" }
            );

            // THEME COLORS
            modelBuilder.Entity<ThemeColor>().HasData(
                new ThemeColor { Id = Guid.NewGuid(), Name = "Orijinal", Value = "#2ba86d" },
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

            // AVATARLAR
            modelBuilder.Entity<Avatar>().HasData(
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 1", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Can" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 2", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Olivia" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 3", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Atlas" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 4", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Deniz" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 5", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Nova" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 6", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Emir" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 7", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Jasper" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 8", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Lina" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 9", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Arda" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 10", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Felix" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 11", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Zeynep" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 12", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Leo" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 13", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Selin" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 14", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Riley" },
                new Avatar { Id = Guid.NewGuid(), Name = "Avatar 15", Url = "https://api.dicebear.com/9.x/shapes/svg?seed=Ayaz" }
            );

            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    DarkMode = false,
                    ThemeColor = "#2ba86d",
                    EnableAnimations = true,
                    TwoFactorEnabled=false,
                    LocationTracking=false,
                    SessionTimeout=0,
                    ActiveSessionLimit=0,
                    AvatarUrl = "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton",
                    EmailNotifications = true,
                    SmsNotifications = false,
                    PushNotifications = true,
                    GoogleMapsApiKey = "",
                    FontTypeId = font1Id,
                    LanguageId = langTrId,
                    DateFormatId = fmt1Id
                }
            );

            modelBuilder.Entity<SystemSetting>()
                .Property(s => s.UserId)
                .IsRequired(false);
        }
    }
}
