using EcoRoute.Settings.Dtos;

namespace EcoRoute.Settings.Services
{
    public interface ISettingsService
    {
        // Get system settings
        Task<SystemSettingDto> GetSettingsAsync(string userId = null);

        // Update system settings
        Task<SystemSettingDto> UpdateSettingsAsync(UpdateSystemSettingDto updateDto, string userId = null);

        // Reset to default settings
        Task<SystemSettingDto> ResetToDefaultsAsync(string userId = null);

        // These methods don't need to change
        Task<List<ThemeColorDto>> GetThemeColorsAsync();
        Task<List<AvatarDto>> GetAvatarsAsync();
        Task<bool> TestGoogleMapsApiAsync(string apiKey);


        Task<List<FontTypeDto>> GetFontTypesAsync();
        Task<List<LanguageDto>> GetLanguagesAsync();
        Task<List<DateFormatDto>> GetDateFormatsAsync();
    }
}
