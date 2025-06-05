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
            var settings = string.IsNullOrEmpty(userId)
                ? await _context.SystemSettings.FirstOrDefaultAsync()
                : await _context.SystemSettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                settings = await CreateDefaultSettingsAsync(userId);
            }

            return _mapper.Map<SystemSettingDto>(settings);
        }
        public async Task<SystemSettingDto> UpdateSettingsAsync(UpdateSystemSettingDto updateDto, string userId = null)
        {
            var settings = string.IsNullOrEmpty(userId)
                ? await _context.SystemSettings.FirstOrDefaultAsync()
                : await _context.SystemSettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                settings = _mapper.Map<SystemSetting>(updateDto);
                settings.Id = Guid.NewGuid();
                settings.UserId = userId;
                _context.SystemSettings.Add(settings);
            }
            else
            {
                _mapper.Map(updateDto, settings);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<SystemSettingDto>(settings);
        }
        public async Task<SystemSettingDto> ResetToDefaultsAsync(string userId = null)
        {
            var settings = string.IsNullOrEmpty(userId)
                ? await _context.SystemSettings.FirstOrDefaultAsync()
                : await _context.SystemSettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings != null)
            {
                settings.DarkMode = false;
                settings.ThemeColor = "#2ba86d"; 
                settings.TwoFactorEnabled = false;
                settings.LocationTracking = false;
                settings.SessionTimeout = 0;
                settings.ActiveSessionLimit = 0;
                settings.EnableAnimations = true;
                settings.AvatarUrl = "https://api.dicebear.com/9.x/shapes/svg?seed=TEST";
                settings.EmailNotifications = true;
                settings.SmsNotifications = false;
                settings.PushNotifications = true;
                settings.GoogleMapsApiKey = "";
                settings.FontTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                settings.LanguageId = Guid.Parse("22222222-2222-2222-2222-222222222222");
                settings.DateFormatId = Guid.Parse("33333333-3333-3333-3333-333333333333");
                await _context.SaveChangesAsync();
            }
            else
            {
                settings = await CreateDefaultSettingsAsync(userId);
            }

            return _mapper.Map<SystemSettingDto>(settings);
        }
        public async Task<List<ThemeColorDto>> GetThemeColorsAsync()
        {
            var themeColors = await _context.ThemeColors.ToListAsync();

            return _mapper.Map<List<ThemeColorDto>>(themeColors);
        }

        public async Task<List<AvatarDto>> GetAvatarsAsync()
        {
            var avatars = await _context.Avatars.ToListAsync();

            return _mapper.Map<List<AvatarDto>>(avatars);
        }

        public async Task<bool> TestGoogleMapsApiAsync(string apiKey)
        {

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return false;
            }

            await Task.Delay(500); 

            return true;
        }

        private async Task<SystemSetting> CreateDefaultSettingsAsync(string userId = null)
        {
            var defaultSettings = new SystemSetting
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DarkMode = false,
                ThemeColor = "#2ba86d", 
                EnableAnimations = true,
                LocationTracking=false,
                SessionTimeout=0,
                ActiveSessionLimit=0,
                TwoFactorEnabled=false,
                AvatarUrl = "https://api.dicebear.com/9.x/shapes/svg?seed=TEST",
                EmailNotifications = true,
                SmsNotifications = false,
                PushNotifications = true,
                GoogleMapsApiKey = "",
                FontTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                LanguageId = Guid.Parse("22222222-2222-2222-2222-222222222222"), 
                DateFormatId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            };

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
