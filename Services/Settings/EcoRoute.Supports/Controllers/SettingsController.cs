using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EcoRoute.Settings.Dtos;
using EcoRoute.Settings.Services;
using System.Security.Claims;

namespace EcoRoute.Supports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ISettingsService settingsService, ILogger<SettingsController> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
        }
        private string GetCurrentUserId()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("Current user ID: {UserId}", userId);
            return userId;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SystemSettingDto>> GetSettings()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Getting settings for user: {UserId}", userId);

                var settings = await _settingsService.GetSettingsAsync(userId);
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting settings");
                return StatusCode(500, "Ayarlar alınırken bir hata oluştu");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SystemSettingDto>> UpdateSettings(UpdateSystemSettingDto updateDto)
        {
            if (updateDto == null)
            {
                _logger.LogWarning("Update DTO is null");
                return BadRequest("Ayar bilgileri boş olamaz");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Updating settings for user: {UserId}", userId);

                if (string.IsNullOrEmpty(updateDto.ThemeColor))
                {
                    updateDto.ThemeColor = "#3B82F6";
                }

                if (string.IsNullOrEmpty(updateDto.AvatarUrl))
                {
                    updateDto.AvatarUrl = "https://api.dicebear.com/9.x/shapes/svg?seed=TEST";
                }

                var updatedSettings = await _settingsService.UpdateSettingsAsync(updateDto, userId);
                return Ok(updatedSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating settings");
                return StatusCode(500, "Ayarlar güncellenirken bir hata oluştu");
            }
        }

        [HttpPost("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SystemSettingDto>> ResetToDefaults()
        {
            var userId = GetCurrentUserId();
            var defaultSettings = await _settingsService.ResetToDefaultsAsync(userId);
            return Ok(defaultSettings);
        }
        [HttpGet("theme-colors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ThemeColorDto>>> GetThemeColors()
        {
            var colors = await _settingsService.GetThemeColorsAsync();
            return Ok(colors);
        }


        [HttpGet("avatars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AvatarDto>>> GetAvatars()
        {
            var avatars = await _settingsService.GetAvatarsAsync();
            return Ok(avatars);
        }


        [HttpPost("test-google-maps")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> TestGoogleMapsApi([FromBody] string apiKey)
        {
            var result = await _settingsService.TestGoogleMapsApiAsync(apiKey);
            return Ok(result);
        }

        [HttpGet("font-types")]
        public async Task<ActionResult<List<FontTypeDto>>> GetFontTypes()
        {
            var fonts = await _settingsService.GetFontTypesAsync();
            return Ok(fonts);
        }

        [HttpGet("languages")]
        public async Task<ActionResult<List<LanguageDto>>> GetLanguages()
        {
            var langs = await _settingsService.GetLanguagesAsync();
            return Ok(langs);
        }

        [HttpGet("date-formats")]
        public async Task<ActionResult<List<DateFormatDto>>> GetDateFormats()
        {
            var formats = await _settingsService.GetDateFormatsAsync();
            return Ok(formats);
        }

    }
}
