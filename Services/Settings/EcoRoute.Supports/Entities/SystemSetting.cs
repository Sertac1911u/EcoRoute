using System.ComponentModel.DataAnnotations;

namespace EcoRoute.Settings.Entities
{
    public class SystemSetting
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public bool DarkMode { get; set; }
        public string ThemeColor { get; set; }
        public bool EnableAnimations { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LocationTracking { get; set; }
        public int SessionTimeout { get; set; } // dakika cinsinden (ör: 15, 30, 60, 0=asla)
        public int ActiveSessionLimit { get; set; } // ör: 1,2,3,0=limitsiz

        // Profil Avatarı
        public string AvatarUrl { get; set; }

        // Bildirim Tercihleri
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool PushNotifications { get; set; }

        // Sistem Entegrasyonları
        public string GoogleMapsApiKey { get; set; }


        public Guid? FontTypeId { get; set; }
        public FontType FontType { get; set; }
        public Guid? LanguageId { get; set; }
        public Language Language { get; set; }
        public Guid? DateFormatId { get; set; }
        public DateFormat DateFormat { get; set; }

    }
}
