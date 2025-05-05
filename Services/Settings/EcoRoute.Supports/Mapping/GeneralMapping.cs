using AutoMapper;
using EcoRoute.Settings.Dtos;
using EcoRoute.Settings.Entities;


namespace EcoRoute.Supports.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<SystemSetting, SystemSettingDto>();
            CreateMap<ThemeColor, ThemeColorDto>();
            CreateMap<Avatar, AvatarDto>();

            // DTO -> Entity mappings
            CreateMap<UpdateSystemSettingDto, SystemSetting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
