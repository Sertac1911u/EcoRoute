﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SettingDtos
{
    public class SystemSettingDto
    {
        public string UserId { get; set; }

        public bool DarkMode { get; set; }
        public string ThemeColor { get; set; }
        public bool LocationTracking { get; set; }
        public bool EnableAnimations { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int SessionTimeout { get; set; }
        public int ActiveSessionLimit { get; set; }


        public string AvatarUrl { get; set; }

        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool PushNotifications { get; set; }

        public string GoogleMapsApiKey { get; set; }
        public Guid? FontTypeId { get; set; }
        public string FontTypeName { get; set; }
        public string FontTypeCss { get; set; }
        public Guid? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public Guid? DateFormatId { get; set; }
        public string DateFormatString { get; set; }
        public string DateFormatSample { get; set; }

    }
}
