using AutoMapper;
using EcoRoute.Settings.Dtos;
using EcoRoute.Settings.Entities;
using EcoRoute.Supports.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoRoute.Settings.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly SettingsContext _context;
        private readonly IMapper _mapper;

        public SettingsService(SettingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SystemSettingDto> GetSettingsAsync(string userId = null)
        {
            // Get settings for the specified user
            var settings = string.IsNullOrEmpty(userId)
                ? await _context.SystemSettings.FirstOrDefaultAsync()
                : await _context.SystemSettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                // If no settings exist, create default settings
                settings = await CreateDefaultSettingsAsync(userId);
            }

            // Map entity to DTO
            return _mapper.Map<SystemSettingDto>(settings);
        }
        public async Task<SystemSettingDto> UpdateSettingsAsync(UpdateSystemSettingDto updateDto, string userId = null)
        {
            // Get current settings for this user
            var settings = string.IsNullOrEmpty(userId)
                ? await _context.SystemSettings.FirstOrDefaultAsync()
                : await _context.SystemSettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                // Create new settings for this user if none exist
                settings = _mapper.Map<SystemSetting>(updateDto);
                settings.Id = Guid.NewGuid();
                settings.UserId = userId;
                _context.SystemSettings.Add(settings);
            }
            else
            {
                // Update existing settings
                _mapper.Map(updateDto, settings);
            }

            // Save changes
            await _context.SaveChangesAsync();

            // Return updated settings as DTO
            return _mapper.Map<SystemSettingDto>(settings);
        }
        public async Task<SystemSettingDto> ResetToDefaultsAsync(string userId = null)
        {
            // Get settings for the specified user
            var settings = string.IsNullOrEmpty(userId)
                ? await _context.SystemSettings.FirstOrDefaultAsync()
                : await _context.SystemSettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings != null)
            {
                // Reset to default values but keep the user ID
                settings.DarkMode = false;
                settings.ThemeColor = "#2ba86d"; // default blue
                settings.TwoFactorEnabled = false;
                settings.LocationTracking = false;
                settings.SessionTimeout = 0;
                settings.ActiveSessionLimit = 0;
                settings.EnableAnimations = true;
                settings.AvatarUrl = "https://ui-avatars.com/api/?name=Sertaç+Kara\r\n"; // default avatar
                settings.EmailNotifications = true;
                settings.SmsNotifications = false;
                settings.PushNotifications = true;
                settings.GoogleMapsApiKey = "";
                settings.FontTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Reset to default font
                settings.LanguageId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Reset to default language
                settings.DateFormatId = Guid.Parse("33333333-3333-3333-3333-333333333333"); // Reset to default date format
                // Save changes
                await _context.SaveChangesAsync();
            }
            else
            {
                // Create default settings
                settings = await CreateDefaultSettingsAsync(userId);
            }

            // Return settings as DTO
            return _mapper.Map<SystemSettingDto>(settings);
        }
        public async Task<List<ThemeColorDto>> GetThemeColorsAsync()
        {
            // Tüm tema renklerini getir
            var themeColors = await _context.ThemeColors.ToListAsync();

            // Entity listesini DTO listesine çevir
            return _mapper.Map<List<ThemeColorDto>>(themeColors);
        }

        public async Task<List<AvatarDto>> GetAvatarsAsync()
        {
            // Tüm avatarları getir
            var avatars = await _context.Avatars.ToListAsync();

            // Entity listesini DTO listesine çevir
            return _mapper.Map<List<AvatarDto>>(avatars);
        }

        public async Task<bool> TestGoogleMapsApiAsync(string apiKey)
        {
            // Bu metot Google Maps API'yi test etmek için kullanılır
            // Gerçek uygulamada burada API'ye istekte bulunulur

            // Basit bir kontrol yapalım
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return false;
            }

            // Gerçek uygulamada burada API'ye istek yapılır
            await Task.Delay(500); // API isteği simülasyonu

            return true;
        }

        private async Task<SystemSetting> CreateDefaultSettingsAsync(string userId = null)
        {
            // Create default settings
            var defaultSettings = new SystemSetting
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DarkMode = false,
                ThemeColor = "#2ba86d", // default blue
                EnableAnimations = true,
                LocationTracking=false,
                SessionTimeout=0,
                ActiveSessionLimit=0,
                TwoFactorEnabled=false,
                AvatarUrl = "https://ui-avatars.com/api/?name=Sertaç+Kara\r\n", // default avatar
                EmailNotifications = true,
                SmsNotifications = false,
                PushNotifications = true,
                GoogleMapsApiKey = "",
                FontTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Default font
                LanguageId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Default language
                DateFormatId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Default date format
            };

            // Add the new settings
            _context.SystemSettings.Add(defaultSettings);
            await _context.SaveChangesAsync();

            return defaultSettings;
        }

        public async Task<List<FontTypeDto>> GetFontTypesAsync()
        {
            var fontTypes = await _context.FontTypes.ToListAsync();
            return _mapper.Map<List<FontTypeDto>>(fontTypes);
        }

        public async Task<List<LanguageDto>> GetLanguagesAsync()
        {
            var languages = await _context.Languages.ToListAsync();
            return _mapper.Map<List<LanguageDto>>(languages);
        }

        public async Task<List<DateFormatDto>> GetDateFormatsAsync()
        {
            var formats = await _context.DateFormats.ToListAsync();
            return _mapper.Map<List<DateFormatDto>>(formats);
        }

    }

}
