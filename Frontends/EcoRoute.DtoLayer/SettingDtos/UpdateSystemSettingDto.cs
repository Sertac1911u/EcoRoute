using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SettingDtos
{
    public class UpdateSystemSettingDto
    {

        public bool DarkMode { get; set; }
        public string ThemeColor { get; set; }
        public int FontSize { get; set; }
        public bool EnableAnimations { get; set; }

        // Profil Avatarı
        public string AvatarUrl { get; set; }

        // Bildirim Tercihleri
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool PushNotifications { get; set; }

        // Sistem Entegrasyonları
        public string GoogleMapsApiKey { get; set; }
    }
}
