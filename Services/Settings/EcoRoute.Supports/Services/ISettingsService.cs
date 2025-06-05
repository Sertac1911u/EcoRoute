using EcoRoute.Settings.Dtos;

namespace EcoRoute.Settings.Services
{
    public interface ISettingsService
    {
        Task<SystemSettingDto> GetSettingsAsync(string userId = null);

        Task<SystemSettingDto> UpdateSettingsAsync(UpdateSystemSettingDto updateDto, string userId = null);

        Task<SystemSettingDto> ResetToDefaultsAsync(string userId = null);

        Task<List<ThemeColorDto>> GetThemeColorsAsync();
        Task<List<AvatarDto>> GetAvatarsAsync();
        Task<bool> TestGoogleMapsApiAsync(string apiKey);


        Task<List<FontTypeDto>> GetFontTypesAsync();
        Task<List<LanguageDto>> GetLanguagesAsync();
        Task<List<DateFormatDto>> GetDateFormatsAsync();
    }
}
