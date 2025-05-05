using AutoMapper;
using EcoRoute.Notifications.Dtos;
using EcoRoute.Notifications.Entities;

namespace EcoRoute.Notifications.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));

            CreateMap<Notification, ResultNotificationDto>();
        }
    }
}
