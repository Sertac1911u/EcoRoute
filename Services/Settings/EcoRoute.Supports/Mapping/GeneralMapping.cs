using AutoMapper;
using EcoRoute.Settings.Dtos;
using EcoRoute.Settings.Entities;


namespace EcoRoute.Supports.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<UpdateSystemSettingDto, SystemSetting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ThemeColor, ThemeColorDto>();
            CreateMap<Avatar, AvatarDto>();

            // DTO -> Entity mappings
            CreateMap<SystemSetting, SystemSettingDto>()
                .ForMember(dest => dest.FontTypeId, opt => opt.MapFrom(src => src.FontTypeId))
                .ForMember(dest => dest.FontTypeName, opt => opt.MapFrom(src => src.FontType != null ? src.FontType.Name : null))
                .ForMember(dest => dest.FontTypeCss, opt => opt.MapFrom(src => src.FontType != null ? src.FontType.CssValue : null))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language != null ? src.Language.Name : null))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.Language != null ? src.Language.CultureCode : null))
                .ForMember(dest => dest.DateFormatId, opt => opt.MapFrom(src => src.DateFormatId))
                .ForMember(dest => dest.DateFormatString, opt => opt.MapFrom(src => src.DateFormat != null ? src.DateFormat.FormatString : null))
                .ForMember(dest => dest.DateFormatSample, opt => opt.MapFrom(src => src.DateFormat != null ? src.DateFormat.Sample : null))
                .ForMember(dest => dest.ThemeColor, opt => opt.MapFrom(src => src.ThemeColor));

            CreateMap<DateFormat, DateFormatDto>();
            CreateMap<FontType, FontTypeDto>();
            CreateMap<Language, LanguageDto>();
        }
    }
}
